/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Cinch;
using HelixToolkit;
using MEFedMVVM.Common;
using MEFedMVVM.ViewModelLocator;
using Microsoft.Xaml.Behaviors;
using Techne.Compat;
using Techne.Importer;
using Techne.Manager;
using Techne.Model;
using Techne.Models;
using Techne.Plugins;
using Techne.Plugins.FileHandler.Texturemap;
using Techne.Plugins.FileHandler.TurboModelThingy;
using Techne.Plugins.Interfaces;
using Techne.Plugins.Shapes;
using Techne.Plugins.ValueConverter;
using Techne.ViewModel;
using EventTrigger = Microsoft.Xaml.Behaviors.EventTrigger;

namespace Techne
{
    [ExportViewModel("MainWindowViewModel")]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class MainWindowViewModel : ViewModelBase
    {
        #region Constructor
        [ImportingConstructor]
        public MainWindowViewModel(IViewAwareStatus viewAwareStatusService, IOpenFileService openFileService, ISaveFileService saveFileService, IUIVisualizerService visualizerService, IMessageBoxService messageBoxService)
        {
            CheckDeployment();

            AsignServices(viewAwareStatusService, openFileService, saveFileService, visualizerService, messageBoxService);

            techneModel = new TechneModel();
            activeModel = new SaveModel
                          {
                              Geometry = new DispatcherNotifiedObservableCollection<ITechneVisual>()
                          };
            techneModel.Models.Add(activeModel);

            Model = new DispatcherNotifiedObservableCollection<Visual3D>();
            TechneModel = new DispatcherNotifiedObservableCollection<ITechneVisual>();

            SettingsManager.Manager = new SettingsManager(this);
            settingsManager = SettingsManager.Manager;
            updateManager = new UpdateManager(this);
            selectedModel = new SelectedVisualModel();
            selectedVisual = new SelectedVisualViewModel(selectedModel, this);

            watcher = new FileSystemWatcher();
            watcher.Changed += LoadedTextureChanged;

            //Linking commands
            InitializeCommands();

            ConfigureUI();
            RegisterPlugins();
            ConfigureHelixView();
            ConfigureTreeView();
            model = new ModelVisual3D();
        }

        private void RegisterPlugins()
        {
            RegisterShapes();
            RegisterImporters();
            RegisterExporters();
        }

        private void RegisterExporters()
        {
            ExportPluginMenuItems.Add(new ExportPluginMenuItemViewModel
                                      {
                                          Command = new SimpleCommand<object, object>(x => ExecuteExportCommand(new TexturemapExproterPlugin())),
                                          Header = "Texturemap"
                                      });

            ExportPluginMenuItems.Add(new ExportPluginMenuItemViewModel
                                      {
                                          Command = new SimpleCommand<object, object>(x => ExecuteExportCommand(new JavaExporter())),
                                          Header = "Turbo Model Thingy"
                                      });

            // TODO: Move this to a plugin somehow.
            /*
            string templatePath = "./Exporter/Templates/";
            if (IsDeployed)
            {
                templatePath = Path.GetFullPath(Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, @"Exporter\Templates"));
            }

            var files = Directory.GetFiles(templatePath, "*.rtpl", SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                var tmp = file;

                ExportPluginMenuItems.Add(new ExportPluginMenuItemViewModel
                                          {
                                              Command = new SimpleCommand<object, object>(x =>
                                                                                          ExecuteExportCommand(new RazorExporter(tmp))),
                                              Header = Path.GetFileNameWithoutExtension(file)
                                          });
            }*/
        }

        private void RegisterImporters()
        {
        }

        private void RegisterShapes()
        {
            ExtensionManager.RegisterShape(new TechneCubeVisual3DPlugin());
            ExtensionManager.RegisterShape(new TurboModelThingyCubeVisual3DPlugin());
            ExtensionManager.RegisterShape(new TurboModelThingyConeShapePlugin());
            ExtensionManager.RegisterShape(new TurboModelThingyCylinderShapePlugin());
            ExtensionManager.RegisterShape(new TurboModelThingySphereShapePlugin());
        }

        private void ConfigureUI()
        {
            ExportPluginMenuItems = new DispatcherNotifiedObservableCollection<ExportPluginMenuItemViewModel>();
            ShapeToolBarItems = new DispatcherNotifiedObservableCollection<ShapeToolItemViewModel>();
            ImportPluginMenuItems = new DispatcherNotifiedObservableCollection<ImportPluginMenuItemViewModel>();
        }

        private void ConfigureTreeView()
        {
            ModelTreeViewModel = new ModelTreeViewModel(activeModel.Geometry, this);
        }

        private void CheckDeployment()
        {
            //IsDeployed = true;
            try
            {
                if (ApplicationDeployment.CurrentDeployment.IsNetworkDeployed || ApplicationDeployment.CurrentDeployment.CurrentVersion != null)
                {
                    IsDeployed = true;
                }
                else
                    IsDeployed = false;
            }
            catch
            {
                IsDeployed = false;
            }
        }

        private void AsignServices(IViewAwareStatus viewAwareStatusService, IOpenFileService openFileService, ISaveFileService saveFileService, IUIVisualizerService visualizerService, IMessageBoxService messageBoxService)
        {
            this.viewAwareStatusService = viewAwareStatusService;
            this.openFileService = openFileService;
            this.saveFileService = saveFileService;
            this.visualizerService = visualizerService;
            this.messageBoxService = messageBoxService;

            this.viewAwareStatusService.ViewLoaded += ViewAwareStatusService_ViewLoaded;
        }

        private void ConfigureHelixView()
        {
            helixView = new HelixView3D();
            
            helixView.Name = "helixView";
            helixView.SnapsToDevicePixels = true;
            helixView.IsHeadLightEnabled = true;

            LoadHelixConfiguration();

            //helixView.ShowCoordinateSystem = true;
            //helixView.ShowViewCube = true;
            helixView.ModelUpDirection = new Vector3D(0, -1, 0);

            EventTrigger mouseLeftButtonDownTrigger = new EventTrigger("MouseLeftButtonDown");
            mouseLeftButtonDownTrigger.Actions.Add(new EventToCommandTrigger
                                                   {
                                                       Command = MouseDownCommand
                                                   });
            mouseLeftButtonDownTrigger.Attach(helixView);

            EventTrigger mouseLeftButtonUpTrigger = new EventTrigger("MouseLeftButtonUp");
            mouseLeftButtonUpTrigger.Actions.Add(new EventToCommandTrigger
                                                 {
                                                     Command = MouseButtonUpCommand
                                                 });
            mouseLeftButtonUpTrigger.Attach(helixView);

            EventTrigger mouseMoveTrigger = new EventTrigger("MouseMove");
            mouseMoveTrigger.Actions.Add(new EventToCommandTrigger
                                         {
                                             Command = MouseMoveCommand
                                         });
            mouseMoveTrigger.Attach(helixView);

            Interaction.GetTriggers(helixView).Add(mouseLeftButtonDownTrigger);

            helixView.Camera = new PerspectiveCamera(new Point3D(5, -30, -40), new Vector3D(-5, 40, 40), new Vector3D(0, -1, 0), 45);

            //Binding modelBinding = new Binding("Model");
            //modelBinding.Source = this;

            helixView.ItemsSource = Model;

            selectionCues = new ModelVisual3D();
        }
        #endregion

        #region EventHandler
        private void ViewAwareStatusService_ViewLoaded()
        {
            if (Designer.IsInDesignMode)
                return;
            try
            {
                if (IsDeployed && settingsManager.Settings.AskedPermission == false)
                {
                    var result =
                        MessageBox.Show(
                            "This Techne version supports the automatic reporting of crashes.\r\nThe only information that is going to be sent is the exception which caused the crash\r\nThe reports are also stored in %user%\\AppData\\local\\Techne\r\nIt will considerably help Techne if you allow those reports to be sent\r\nIf you consent to it being sent press Yes, otherwise No\r\nYou can always change this setting in the options menu",
                            "Allow Techne to send crash-reports?",
                            MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        settingsManager.Settings.AskedPermission = true;
                        settingsManager.Settings.AutoReportErrors = true;
                    }
                    else
                    {
                        settingsManager.Settings.AskedPermission = true;
                        settingsManager.Settings.AutoReportErrors = false;
                    }
                    settingsManager.Save();
                }

                //insert everything before something could go wrong ;)
                InsertStandardModels();

                //NotifyPropertyChanged(ShapeToolBarItems);
                NotifyPropertyChanged("ToolToolBarItems");
                NotifyPropertyChanged(exportPluginMenuItemsChangeArgs);

                updateManager.CheckForUpdates(settingsManager.Version, IsDeployed);

                backupManager = new BackupManager();

                if (Application.Current != null && !String.IsNullOrEmpty(Application.Current.Properties["FileName"] as String))
                {
                    try
                    {
                        OpenModel(Application.Current.Properties["FileName"] as String);
                    }
                    catch (Exception x)
                    {
                        MessageBox.Show("I'm sorry Dave, I'm afraid I can't do that.\r\nSomething went wrong loading that file");
                        App.SendErrorReport(x, "Loading file after startup");
                    }
                }
                else
                {
                    if (settingsManager.TechneSettings.ShowNewProjectDialogOnStartup)
                        OpenDialog(new NewProjectViewModel(this), "NewProjectView");
                    else
                        OpenModel(new TechneModel
                                  {
                                      Author = "ZeuX",
                                      HasChanged = false,
                                      ProjectName = "New",
                                      Name = "New",
                                      ProjectType = ProjectType.Minecraft,
                                      Models = new List<SaveModel>
                                               {
                                                   new SaveModel
                                                   {
                                                       BaseClass = "ModelBase",
                                                       Name = "New"
                                                   }
                                               }
                                  });
                }

                backupManager.Start(this);

                hitTestManager = new HitTestManager(this);

                AddVisual(model);
            }
            catch (Exception x)
            {
                throw;
            }
        }

        private bool CheckCrashstate(string crashFile)
        {
            return File.Exists(crashFile);
        }

        internal void NewVersionAvailable(ChangelogViewModel log)
        {
            if (log == null)
            {
                return;
            }

            ShowUpdateDialog(log);

            if (IsDeployed)
            {
                settingsManager.Version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
            }
            else
            {
                settingsManager.Version = updateManager.LatestVersion ?? settingsManager.Version;
            }

            settingsManager.Save();
        }
        #endregion

        #region Methods

        #region private
        private void LoadHelixConfiguration()
        {
            helixView.ShowCameraTarget = settingsManager.HelixViewSettings.ShowCameraTarget;
            helixView.ShowViewCube = settingsManager.HelixViewSettings.ShowViewCube;
            helixView.ShowFieldOfView = settingsManager.HelixViewSettings.ShowFieldOfView;
            helixView.ShowFrameRate = settingsManager.HelixViewSettings.ShowFrameRate;
            helixView.ShowCoordinateSystem = settingsManager.HelixViewSettings.ShowCoordinateSystem;
            helixView.CameraInertiaFactor = settingsManager.HelixViewSettings.CameraInertiaFactor;
            helixView.CameraMode = settingsManager.HelixViewSettings.CameraMode;
            helixView.CameraRotationMode = settingsManager.HelixViewSettings.CameraRotationMode;
            helixView.InfiniteSpin = settingsManager.HelixViewSettings.InfiniteSpin;
            helixView.Orthographic = settingsManager.HelixViewSettings.Orthographic;
            helixView.RotationSensitivity = settingsManager.HelixViewSettings.RotationSensitivity;
        }

        private void ShowUpdateDialog(ChangelogViewModel log)
        {
            visualizerService.ShowDialog("ChangelogView", log);
        }

        private void OpenDialog(ViewModelBase viewModel, string viewName)
        {
            visualizerService.ShowDialog(viewName, viewModel);
        }

        private void OpenWindow(ViewModelBase viewModel, string viewName)
        {
            visualizerService.Show(viewName, viewModel, true, null);
        }

        public void FocusSelectedVisual()
        {
            if (SelectedVisual != null)
                helixView.LookAt(selectedModel.Center, 50, 500);
        }

        public void AddShape(string guidString)
        {
            ITechneVisual visual = ExtensionManager.GetShape(guidString);

            visual.Texture = Texture;
            visual.TextureSize = textureViewModel.TextureSize;
            visual.Name = GetUniqueName();

            AddVisual(visual);

            ChangeOpacity(1, TechneModel);
            if (Keyboard.Modifiers != ModifierKeys.Shift)
            {
                SelectedVisual.SelectVisual(visual);
            }
            else
            {
                SelectedVisual.AddVisual(visual);
            }
            ChangeOpacity(0.5, TechneModel);
            UpdateTextureOverlay();
        }

        internal string GetUniqueName(string baseName = "Shape")
        {
            IList<string> names = GetNames(model.Children.Cast<ITechneVisual>());
            int i = 1;

            while (names.Contains(baseName + i.ToString()))
            {
                i++;
            }

            return baseName + i.ToString();
        }

        private IList<string> GetNames(IEnumerable<ITechneVisual> visuals)
        {
            return visuals.Select(y => y.Name).ToList();
        }

        internal void ShowWireframeAndAnchor()
        {
            if (settingsManager.TechneSettings.ShowAnchorPoint || settingsManager.TechneSettings.ShowWireFrame)
            {
                foreach (var item in SelectedVisual.SelectedVisuals)
                {
                    if (settingsManager.TechneSettings.ShowAnchorPoint && !(item is TechneVisualFolder))
                    {
                        var offsetSphere = new SphereVisual3D
                        {
                            Radius = settingsManager.TechneSettings.OffsetSphereRadius,
                        };

                        if (item is TechneVisualCollection)
                        {
                            offsetSphere.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
                        }

                        BindingOperations.SetBinding(offsetSphere,
                             SphereVisual3D.CenterProperty,
                             new Binding("CombinedPosition")
                             {
                                 Source = item,
                                 Converter = new Vector3DToPoint3DConverter()
                             });

                        selectionCues.Children.Add(offsetSphere);
                    }

                    if (settingsManager.TechneSettings.ShowWireFrame && !(item is ITechneVisualCollection))
                    {
                        BoundingBoxVisual3D boxWireframe = new BoundingBoxVisual3D();

                        MultiBinding rectBinding = new MultiBinding();
                        rectBinding.Bindings.Add(new Binding("Center")
                                                 {
                                                     Source = item
                                                 });
                        rectBinding.Bindings.Add(new Binding("Size")
                                                 {
                                                     Source = item
                                                 });
                        rectBinding.Converter = new Vector3DToRect3DConverter();

                        Binding transformBinding = new Binding("Transform")
                        {
                            Source = item
                        };

                        BindingOperations.SetBinding(boxWireframe, BoundingBoxVisual3D.BoundingBoxProperty, rectBinding);
                        BindingOperations.SetBinding(boxWireframe, ModelVisual3D.TransformProperty, transformBinding);

                        selectionCues.Children.Add(boxWireframe);
                    }

                    //if (hitTestManager.ThreeAxisControlVisual3D != null && !Model.Contains(hitTestManager.ThreeAxisControlVisual3D))
                    //{
                    //    BindingOperations.SetBinding(hitTestManager.ThreeAxisControlVisual3D, BoundingBoxVisual3D.TransformProperty, new Binding("Transform") { Source = SelectedVisual });
                    //    var position = SelectedVisual.Center.ToVector3D();
                    //    //position.X += SelectedVisual.Width / 2;
                    //    //position.Y += SelectedVisual.Length / 2;
                    //    //position.Z += SelectedVisual.Height / 2;
                    //    hitTestManager.ThreeAxisControlVisual3D.Position = position;
                    //    Model.Add(hitTestManager.ThreeAxisControlVisual3D);
                    //}

                }
            }
        }

        private void InsertStandardModels()
        {
            var defaultLight = new DefaultLightsVisual3D();
            (defaultLight.Content as Model3DGroup).Children.Add(new DirectionalLight(Color.FromRgb(180, 180, 180), new Vector3D(1, 1, 1)));
            (defaultLight.Content as Model3DGroup).Children.Add(new DirectionalLight(Color.FromRgb(20, 20, 20), new Vector3D(0, 1, 0)));
            
            Model.Add(defaultLight);

            gridLines = new GridLinesVisual3D
                        {
                            Center = new Point3D(8, 24, 8),
                            Thickness = 0.05,
                            Length = 100,
                            Width = 100,
                            Normal = new Vector3D(0, 1, 0),
                            MajorDistance = 16,
                            MinorDistance = 1
                        };

            Model.Add(gridLines);
            Model.Add(selectionCues);

            CreateScenery();
        }

        private void CreateScenery()
        {
            ground = new ModelVisual3D();

            MeshBuilder b = new MeshBuilder();
            b.AddBox(new Point3D(0, 24 + 8, 0), 16, 16, 16);
            var mesh = b.ToMesh();

            //if ((faces & BoxFaces.Front) == BoxFaces.Front)
            //    AddCubeFace(center, new Vector3D(0, 1, 0), new Vector3D(0, 0, 1), xLength, yLength, zLength);
            //if ((faces & BoxFaces.Left) == BoxFaces.Left)
            //    AddCubeFace(center, new Vector3D(-1, 0, 0), new Vector3D(0, 0, 1), yLength, xLength, zLength);
            //if ((faces & BoxFaces.Right) == BoxFaces.Right)
            //    AddCubeFace(center, new Vector3D(1, 0, 0), new Vector3D(0, 0, 1), yLength, xLength, zLength);
            //if ((faces & BoxFaces.Back) == BoxFaces.Back)
            //    AddCubeFace(center, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), xLength, yLength, zLength);
            //if ((faces & BoxFaces.Top) == BoxFaces.Top)
            //    AddCubeFace(center, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0), zLength, yLength, xLength);
            //if ((faces & BoxFaces.Bottom) == BoxFaces.Bottom)
            //    AddCubeFace(center, new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), zLength, yLength, xLength);

            //top
            //new Point(0, 0),
            //new Point(0.25, 0),
            //new Point(0.25, 1),
            //new Point(0, 1),

            //side
            //new Point(0.75, 0),
            //new Point(1, 0),
            //new Point(1, 1),
            //new Point(0.75, 1),

            //bottom
            //new Point(0.5, 0),
            //new Point(0.75, 0),
            //new Point(0.75, 1),
            //new Point(0.5, 1),

            //mesh.TextureCoordinates = new PointCollection(new List<Point>()
            //{
            //    new Point(0.5, 0),
            //    new Point(0.75, 0),
            //    new Point(0.75, 1),
            //    new Point(0.5, 1),

            //    new Point(0.75, 0),
            //    new Point(1, 0),
            //    new Point(1, 1),
            //    new Point(0.75, 1),

            //    new Point(0.75, 0),
            //    new Point(1, 0),
            //    new Point(1, 1),
            //    new Point(0.75, 1),

            //    new Point(0, 0),
            //    new Point(0.25, 0),
            //    new Point(0.25, 1),
            //    new Point(0, 1),

            //    new Point(0.75, 0),
            //    new Point(1, 0),
            //    new Point(1, 1),
            //    new Point(0.75, 1),

            //    new Point(0.75, 0),
            //    new Point(1, 0),
            //    new Point(1, 1),
            //    new Point(0.75, 1),

            //});

            //mesh.TextureCoordinates = new PointCollection(new List<Point>()
            //{
            //    new Point(16, 0),
            //    new Point(32, 0),
            //    new Point(32, 16),
            //    new Point(16, 16),

            //    new Point(48, 0),
            //    new Point(64, 0),
            //    new Point(64, 16),
            //    new Point(48, 16),

            //    new Point(48, 0),
            //    new Point(64, 0),
            //    new Point(64, 16),
            //    new Point(48, 16),

            //    new Point(0, 0),
            //    new Point(16, 0),
            //    new Point(16, 16),
            //    new Point(0, 16),

            //    new Point(48, 0),
            //    new Point(64, 0),
            //    new Point(64, 16),
            //    new Point(48, 16),

            //    new Point(48, 0),
            //    new Point(64, 0),
            //    new Point(64, 16),
            //    new Point(48, 16),

            //});

            Image image = new Image
                          {
                              Source = new BitmapImage(new Uri("./resources/earth.png", UriKind.Relative))
                          };
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
            var brush = new VisualBrush();

            brush.Visual = image;

            brush.ViewboxUnits = BrushMappingMode.Absolute;
            brush.Viewbox = new Rect(0, 0, 16, 16);
            brush.ViewportUnits = BrushMappingMode.RelativeToBoundingBox;
            brush.Viewport = new Rect(0, 0, 1, 1);
            //brush.TileMode = TileMode.Tile;

            //ImageBrush imageBrush = new ImageBrush(new System.Windows.Media.Imaging.BitmapImage(new Uri("./resources/earth.png", UriKind.Relative)));
            //RenderOptions.SetBitmapScalingMode(imageBrush, BitmapScalingMode.NearestNeighbor);

            //imageBrush.ViewboxUnits = BrushMappingMode.Absolute;
            //imageBrush.Viewbox = new Rect(0, 0, 16, 16);


            ground.Content = new GeometryModel3D(mesh, new DiffuseMaterial(brush));

            Model.Add(ground);
        }
        #endregion

        #region internal

        #region Open/Save
        internal void OpenModel(TechneModel techneModel)
        {
            ClearModel();

            if (techneModel.Name.StartsWith("Model"))
                techneModel.Name = techneModel.Name.Substring(0, 5);

            this.techneModel = techneModel;
            OnTechneModelChanged();

            ActiveModel = techneModel.Models.FirstOrDefault();

            if (activeModel != null)
            {
                if (activeModel.TextureSize.X < 16)
                    activeModel.TextureSize = new Vector(64, activeModel.TextureSize.Y);
                if (activeModel.TextureSize.Y < 16)
                    activeModel.TextureSize = new Vector(activeModel.TextureSize.X, 32);

                textureViewModel.TextureSize = activeModel.TextureSize;
                activeModel.TextureSize = activeModel.TextureSize;
                Texture = null;
            }

            foreach (var saveModel in techneModel.Models)
            {
                IEnumerable<ITechneVisual> imports = null;

                if (!string.IsNullOrEmpty(saveModel.BaseClass))
                {
                    if (saveModel.BaseClass.Equals("ModelBase"))
                    {
                    }
                    else
                    {
                        if (saveModel.BaseClass.StartsWith("Model"))
                        {
                            string baseModelPath = "./resources/Models/" + saveModel.BaseClass + ".tcn";

                            if (IsDeployed)
                                baseModelPath = Path.GetFullPath(Path.Combine(ApplicationDeployment.CurrentDeployment.DataDirectory, @"Resources\Models", saveModel.BaseClass + ".tcn"));

                            if (File.Exists(baseModelPath))
                            {
                                DecorationImporter importer = new DecorationImporter();

                                imports = importer.Import(ExtensionManager.ShapePlugins, baseModelPath);
                            }
                        }
                        else if (File.Exists(saveModel.BaseClass))
                        {
                            DecorationImporter importer = new DecorationImporter();
                            imports = importer.Import(ExtensionManager.ShapePlugins, saveModel.BaseClass);
                        }
                    }
                }

                if (imports != null)
                {
                    var first = imports.FirstOrDefault();

                    if (first == null)
                        continue;

                    if (first is ITechneVisualCollection)
                    {
                        first.Name = saveModel.BaseClass;
                        first.Texture = Texture;
                        first.TextureSize = textureViewModel.TextureSize;
                        AddVisual(first);
                        AddItemsToModel(first.Children);
                    }
                    else
                    {
                        TechneVisualFolder baseClass = new TechneVisualFolder
                                                           {
                                                               Name = saveModel.BaseClass
                                                           };

                        foreach (var item in imports)
                        {
                            baseClass.AddChild(item);
                        }

                        baseClass.Texture = Texture;
                        baseClass.TextureSize = textureViewModel.TextureSize;

                        AddVisual((Visual3D)baseClass);
                        AddItemsToModel(baseClass.Children);
                    }
                }

                saveModel.BaseClass = "ModelBase";
                saveModel.Name = techneModel.Name;
            }
        }

        internal void OnTechneModelChanged()
        {
            ShapeToolBarItems.Clear();
            Assembly myAssembly = Assembly.GetExecutingAssembly();

            switch (techneModel.ProjectType)
            {
                case ProjectType.Minecraft:
                    {
                        AddShapeToolBarItem(myAssembly, "D9E621F7-957F-4B77-B1AE-20DCD0DA7751", "Techne.Resources.Cube.png", "Cube");
                    }
                    break;
                case ProjectType.TurboModelThingy:
                    {
                        AddShapeToolBarItem(myAssembly, "D9E621F7-957F-4B77-B1AE-20DCD0DA7751", "Techne.Resources.Cube.png", "Cube");
                        AddShapeToolBarItem(myAssembly, "DE81AA14-BD60-4228-8D8D-5238BCD3CAAA", "Techne.Resources.AddCube.png", "TMT-Cube");
                        AddShapeToolBarItem(myAssembly, "E1957603-6C07-4A1E-9047-BB1F45E57CEA", "Techne.Resources.AddSphere.png", "Sphere");
                        AddShapeToolBarItem(myAssembly, "0900DE04-664F-4789-8562-07FFE1043E90", "Techne.Resources.AddCone.png", "Cone");
                        AddShapeToolBarItem(myAssembly, "B94B0064-E61C-4517-8F99-ADB273F1B33E", "Techne.Resources.AddCylinder.png", "Cylinder");
                    }
                    break;
                default:
                    break;
            }
        }

        private void AddShapeToolBarItem(Assembly myAssembly, string guid, string resource, string name)
        {
            var command = new SimpleCommand<object, Object>(x => AddShape(guid));
            ShapeToolItemViewModel menuItem;
            var bitmap = new BitmapImage();
            using (Stream stream = myAssembly.GetManifestResourceStream(resource))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                bitmap.Freeze();
            }

            menuItem = new ShapeToolItemViewModel(name, command)
                       {
                           Icon = bitmap
                       };
            ShapeToolBarItems.Add(menuItem);
        }

        public void OpenModel(string filename)
        {
            if (!File.Exists(Path.GetFullPath(filename)))
                return;

            if (textureStream != null)
            {
                textureStream.Close();
                textureStream.Dispose();
            }

            ClearModel();
            if (watcher != null)
                watcher.EnableRaisingEvents = false;

            try
            {
                techneModel = FileManager.Load(filename, ExtensionManager.ShapePlugins);
            }
            catch (InvalidDataException)
            {
                MessageBox.Show("Oho, looks like you tried to load a file that isn't a zip.\r\nIf you are in fact loading a tcn-file, please contact ZeuX\r\nUse the \"Send Feedback\"-dialogue or the forum-thread");
                return;
            }

            if (techneModel == null)
            {
                MessageBox.Show("Loading this file failed.\r\nIf you keep getting this error please contact ZeuX\r\nUse the \"Send Feedback\"-dialogue or the forum-thread");
                return;
            }
            if (techneModel.Models != null)
            {
                ActiveModel = techneModel.Models.FirstOrDefault();

                if (ActiveModel != null && ActiveModel.Geometry != null)
                {
                    AddItemsToModel(ActiveModel.Geometry.ToList());

                    if (ActiveModel.TextureSize.X == 0 || ActiveModel.TextureSize.Y == 0)
                    {
                        ActiveModel.TextureSize = new Vector(64, 32);
                    }

                    textureViewModel = new TextureViewerViewModel
                        {
                            TextureSize = ActiveModel.TextureSize
                        };

                    ActiveModel.TextureSize = ActiveModel.TextureSize;
                    Texture = ActiveModel.Texture;
                    textureStream = (MemoryStream)ActiveModel.TextureStream;
                }
            }
            OnTechneModelChanged();
            UpdateTextureOverlay();
        }

        private void AddItemsToModel(IEnumerable<ITechneVisual> list)
        {
            foreach (var visual in list)
            {
                if (visual is ITechneVisualCollection)
                {
                    AddItemsToModel(((ITechneVisualCollection)visual).Children.ToList());
                }
                else
                {
                    model.Children.Add((Visual3D)visual);
                }
            }
        }

        internal void LoadedTextureChanged(object sender, FileSystemEventArgs e)
        {
            if (!UISynchronizationContext.Instance.InvokeRequired)
            {
                LoadTexture(e.FullPath);
            }
            else
            {
                UISynchronizationContext.Instance.InvokeAndBlockUntilCompletion(() => LoadTexture(e.FullPath));
            }
        }

        public void LoadTexture(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    return;

                if (textureStream != null)
                {
                    textureStream.Close();
                    textureStream.Dispose();
                    //textureStream = new MemoryStream();
                }

                var texture = new BitmapImage();

                Thread.Sleep(200);

                textureStream = new MemoryStream(File.ReadAllBytes(fileName));

                texture.BeginInit();
                texture.CacheOption = BitmapCacheOption.OnLoad;
                texture.StreamSource = textureStream;
                texture.EndInit();

                if ((Math.Round(texture.DpiX) + Math.Round(texture.DpiY) > 97 * 2 || Math.Round(texture.DpiX) + Math.Round(texture.DpiY) < 95 * 2))
                {
                    MessageBox.Show("This image doesn't use 96dpi, you need to use your imaging software to save it again.\r\nIf you don't, the image wont map correctly in Techne.");
                }

                texture.Freeze();

                if (settingsManager.TechneSettings.AskTextureResolution &&
                    (texture.PixelWidth != activeModel.TextureSize.X || texture.PixelHeight != activeModel.TextureSize.Y))
                {
                    if (messageBoxService.ShowYesNo("The resolution of the image you selected differs from your project settings.\r\nDo you want to update the settings to match the new resolution?", CustomDialogIcons.Question) == CustomDialogResults.Yes)
                    {
                        activeModel.TextureSize = new Vector(texture.PixelWidth, texture.PixelHeight);
                        TextureViewModel.TextureSize = activeModel.TextureSize;
                    }
                }

                Texture = texture;
                activeModel.Texture = texture;
                activeModel.TextureStream = textureStream;
            }
            catch
            {
                //screw you
            }
        }

        public void SaveModel(string savedFileName, bool isBackup = false)
        {
            try
            {
                if (String.IsNullOrEmpty(activeModel.Name))
                {
                    activeModel.Name = "Model";
                }

                byte[] backup = null;

                if (File.Exists(savedFileName))
                {
                    backup = File.ReadAllBytes(savedFileName);

                    if (File.Exists(savedFileName + ".bak"))
                        File.Delete(savedFileName + ".bak");

                    File.Move(savedFileName, savedFileName + ".bak");
                }

                try
                {
                    FileManager.Save(savedFileName, techneModel);
                    if (!isBackup)
                    {
                        techneModel.HasChanged = false;
                        techneModel.Location = savedFileName;
                    }
                    File.Delete(savedFileName + ".bak");
                }
                catch (Exception x)
                {
                    MessageBox.Show("something went wrong, didn't save model\r\n\r\n" + x);

                    if (backup != null)
                        File.WriteAllBytes(savedFileName, backup);
                }

                Message = "Model successfully saved to " + savedFileName;
            }
            catch (Exception x)
            {
                MessageBox.Show("something went wrong, didn't save model\r\n\r\n" + x);
            }
        }
        #endregion

        public void ReloadConfiguration()
        {
            LoadHelixConfiguration();
        }
        #endregion

        #region public
        public IEnumerable<ITechneVisual> CloneVisual(IEnumerable<ITechneVisual> visuals)
        {
            foreach (var iTechneVisual in visuals)
            {
                var guid = iTechneVisual.Guid.ToString();
                ITechneVisual clonedVisual = ExtensionManager.GetShape(guid);

                clonedVisual.RotationX = iTechneVisual.RotationX;
                clonedVisual.RotationY = iTechneVisual.RotationY;
                clonedVisual.RotationZ = iTechneVisual.RotationZ;
                clonedVisual.Guid = iTechneVisual.Guid;
                clonedVisual.Texture = iTechneVisual.Texture;
                clonedVisual.Name = iTechneVisual.Name;
                clonedVisual.Position = iTechneVisual.Position;
                clonedVisual.Offset = iTechneVisual.Offset;
                clonedVisual.TextureSize = iTechneVisual.TextureSize;

                if (iTechneVisual is ITechneVisualCollection)
                {
                    foreach (var child in CloneVisual(iTechneVisual.Children).ToList())
                    {
                        ((ITechneVisualCollection)clonedVisual).AddChild(child);
                    }
                }
                else
                {
                    clonedVisual.Height = iTechneVisual.Height;
                    clonedVisual.Length = iTechneVisual.Length;
                    clonedVisual.Size = iTechneVisual.Size;
                    clonedVisual.TextureOffset = iTechneVisual.TextureOffset;
                    clonedVisual.Width = iTechneVisual.Width;
                }

                yield return clonedVisual;
            }

            //return clones;
        }
        #endregion

        #region protected
        protected override void NotifyPropertyChanged(PropertyChangedEventArgs args)
        {
            //this is ugly
            //there is a RaiseCloseRequest - I may use that - somehow
            //todo: make that work
            if (args.Equals(null))
            {
                techneModel.HasChanged = true;
                NotifyPropertyChanged(hasChangedChangeArgs);
            }

            base.NotifyPropertyChanged(args);
        }
        #endregion

        #region History
        private new void PropertyChanged(ITechneVisual visualHit, HistoryAction historyAction)
        {
        }

        private new void PropertyChanged(object oldValue, object newValue, PropertyInfo property)
        {   
        }
        #endregion

        #region Model
        public void SelectAll()
        {
            DeselectVisual();
            foreach (var item in TechneModel.ToList())
            {
                SelectedVisual.AddVisual(item);
            }
        }

        internal void ChangeOpacity(double opacity, IEnumerable<ITechneVisual> visuals)
        {
            foreach (var item in visuals)
            {
                if (item == null)
                    continue;

                if (SelectedVisual.IsSelected(item))
                {
                    item.Opacity = 1;
                    continue;
                }

                if (item is ITechneVisualCollection)
                {
                    ChangeOpacity(opacity, (item as ITechneVisualCollection).Children);
                }
                else
                {
                    if (item.Opacity != opacity)
                        item.Opacity = opacity;
                }
            }
        }

        internal void DeselectVisual()
        {
            SelectedVisual.ClearSelection();

            ChangeOpacity(1, TechneModel);

            selectionCues.Children.Clear();
        }

        internal void DeselectVisual(ITechneVisual techneVisual)
        {
            ChangeOpacity(1, TechneModel);

            selectionCues.Children.Clear();
        }

        public void AddVisual(ITechneVisual visual)
        {
            AddVisual(visual as Visual3D);
        }

        public void AddVisual(Visual3D visual)
        {
            if (visual == null)
                return;

            var parent = VisualTreeHelper.GetParent(visual);
            if (parent != null)
            {
                if (parent is ModelVisual3D)
                {
                    (parent as ModelVisual3D).Children.Remove(visual);
                }
                else
                {
                }
            }

            var techneVisual = visual as ITechneVisual;

            if (techneVisual != null)
            {
                TechneModel.Add(techneVisual);

                ITechneVisual first;
                if ((first = SelectedVisual.SelectedVisuals.FirstOrDefault()) != null)
                {
                    if (first is ITechneVisualCollection)
                        ((ITechneVisualCollection)first).AddChild(techneVisual);
                    else if (first.Parent != null)
                        first.Parent.AddChild(techneVisual);
                    else
                    {
                        if (!activeModel.Geometry.Contains(techneVisual))
                            activeModel.Geometry.Add(techneVisual);
                    }

                    model.Children.Add((Visual3D)techneVisual);
                }
                else
                {
                    if (!activeModel.Geometry.Contains(techneVisual))
                        activeModel.Geometry.Add(techneVisual);

                    model.Children.Add((Visual3D)techneVisual);
                }

                ShowWireframeAndAnchor();
                UpdateTextureOverlay();
            }
            else
            {
                Model.Add(visual);
            }
        }

        internal void UpdateTextureOverlay()
        {
            Canvas canvas = new Canvas();

            if (SelectedVisual.HasSelected() && settingsManager.TechneSettings.DisplaySingleOverlay)
            {
                GetTextureOverlay(selectedModel.SelectedVisual, ref canvas, true);
            }
            else
            {
                GetTextureOverlay(activeModel.Geometry, ref canvas);
            }

            TextureViewerOverlay = canvas;
        }

        private void GetTextureOverlay(IEnumerable<ITechneVisual> visuals, ref Canvas canvas, bool isSelected = false)
        {
            foreach (var item in visuals)
            {
                if (item is ITechneVisualCollection)
                {
                    GetTextureOverlay(((ITechneVisualCollection)item).Children, ref canvas, isSelected || SelectedVisual.IsSelected(item));
                }
                else
                {
                    GetTextureOverlay(canvas, isSelected, item);
                }
            }
        }

        private void GetTextureOverlay(Canvas canvas, bool isSelected, ITechneVisual item)
        {
            string guid = item.Guid.ToString();
            if (!ExtensionManager.ShapePlugins.ContainsKey(guid))
            {
                guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();
            }

            var overlay = ExtensionManager.ShapePlugins[guid].GetTextureViewerOverlay(item, item == SelectedVisual);

            if (!isSelected && !SelectedVisual.IsSelected(item))
            {
                overlay.Opacity = 0.25;
            }
            else
            {
                overlay.Opacity = 0.75;
            }

            canvas.Children.Add(overlay);
        }

        private void AddVisual(IEnumerable<ITechneVisual> visuals)
        {
            foreach (var item in visuals)
            {
                if (item.IsDecorative)
                {
                    item.Opacity = settingsManager.TechneSettings.DecorationOpacity;
                }
                AddVisual(item);
            }
        }

        public void RemoveVisual(Visual3D visual3D)
        {
            var techneVisual = visual3D as ITechneVisual;

            if (techneVisual != null)
            {
                if (SelectedVisual.IsSelected(techneVisual))
                    SelectedVisual.RemoveVisual(techneVisual);

                if (techneVisual.Parent != null)
                    techneVisual.Parent.RemoveChild(techneVisual);

                model.Children.Remove(visual3D);

                foreach (var child in techneVisual.Children.ToList())
                {
                    RemoveVisual(child);
                }

                TechneModel.Remove(techneVisual);
                try
                {
                    activeModel.Geometry.Remove(techneVisual);
                }
                catch (Exception e)
                {
                    if (!e.TargetSite.Name.Equals("EvaluateOldNewStates", StringComparison.InvariantCultureIgnoreCase))
                        throw;
                }
                PropertyChanged(techneVisual, HistoryAction.RemoveVisual);
                UpdateTextureOverlay();
            }
            else
            {
                Model.Remove(visual3D);
            }
        }

        public void RemoveVisual(IList<ITechneVisual> visuals)
        {
            foreach (var techneVisual in visuals)
            {
                RemoveVisual(techneVisual);

                if (SelectedVisual.IsSelected(techneVisual))
                {
                    SelectedVisual.RemoveVisual(techneVisual);
                }
            }
        }

        public void RemoveVisual(ITechneVisual visual)
        {
            RemoveVisual(visual as Visual3D);
        }

        public void ClearModel()
        {
            try
            {
                DeselectVisual();
                TechneModel.Clear();
                foreach (var item in techneModel.Models)
                {
                    item.Geometry.Clear();
                }
                model.Children.Clear();
                activeModel.Geometry.Clear();
                UpdateTextureOverlay();
            }
            catch
            {
            }
        }
        #endregion

        #endregion
    }
}

