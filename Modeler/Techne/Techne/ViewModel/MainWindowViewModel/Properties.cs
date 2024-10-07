/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Cinch;
using HelixToolkit;
using Techne.Manager;
using Techne.Model;
using Techne.Models;
using Techne.Plugins.Interfaces;
using Techne.ViewModel;

namespace Techne
{
    public partial class MainWindowViewModel
    {
        #region Fields

        #region Manager
        public BackupManager backupManager;
        //public HistoryManager historyManager;
        private HitTestManager hitTestManager;
        public static SettingsManager settingsManager;
        private readonly UpdateManager updateManager;
        #endregion

        #region Services
        private IMessageBoxService messageBoxService;
        public IOpenFileService openFileService;
        public ISaveFileService saveFileService;
        private IViewAwareStatus viewAwareStatusService;
        private IUIVisualizerService visualizerService;
        #endregion

        #region Visuals
        private readonly ModelVisual3D model;
        private IEnumerable<ITechneVisual> copiedModel;
        private GridLinesVisual3D gridLines;
        private ModelVisual3D ground;
        private HelixView3D helixView;
        private ModelVisual3D models = new ModelVisual3D();
        private ModelVisual3D selectionCues;
        private readonly SelectedVisualViewModel selectedVisual;
        internal readonly SelectedVisualModel selectedModel;
        private TextureViewerViewModel textureViewModel = new TextureViewerViewModel();
        #endregion

        private Techne.ModelTreeViewModel modelTreeViewModel;
        public static bool IsDeployed;
        private readonly FileSystemWatcher watcher;
        private SaveModel activeModel;
        private AnimationDataModel animationX;
        private AnimationDataModel animationY;
        private AnimationDataModel animationZ;

        //stuff that stays
        //not decided
        private string message;
        private bool showcaseActive;
        private TechneModel techneModel;
        private MemoryStream textureStream;
        #endregion

        #region ProtperyChangedEventArgs
        //static PropertyChangedEventArgs modelChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.Model);

        private static readonly PropertyChangedEventArgs textureViewerChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.TextureViewModel);
        private static readonly PropertyChangedEventArgs textureChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.Texture);
        private static readonly PropertyChangedEventArgs textureOverlayChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.TextureViewerOverlay);

        private static readonly PropertyChangedEventArgs glScaleChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.GlScale);
        private static readonly PropertyChangedEventArgs glScaleXChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.GlScaleX);
        private static readonly PropertyChangedEventArgs glScaleYChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.GlScaleY);
        private static readonly PropertyChangedEventArgs glScaleZChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.GlScaleZ);

        private static readonly PropertyChangedEventArgs hasChangedChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.HasChanged);

        private static readonly PropertyChangedEventArgs techneModelChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.TechneModel);

        private static readonly PropertyChangedEventArgs exportPluginMenuItemsChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.ExportPluginMenuItems);
        private static readonly PropertyChangedEventArgs helixViewChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.HelixView);
        private static readonly PropertyChangedEventArgs modelTreeViewModelChangeArgs = ObservableHelper.CreateArgs<MainWindowViewModel>(x => x.ModelTreeViewModel);
        #endregion

        #region Public Properties
        public SaveModel ActiveModel
        {
            get { return activeModel; }
            set
            {
                activeModel = value;

                if (activeModel != null)
                {
                    ModelTreeViewModel = new ModelTreeViewModel(activeModel.Geometry, this);
                }
            }
        }

        public ModelTreeViewModel ModelTreeViewModel
        {
            get
            {
                return modelTreeViewModel;
            }
            set
            {
                modelTreeViewModel = value;
                NotifyPropertyChanged(modelTreeViewModelChangeArgs);
            }
        }

        public String Message
        {
            get { return message; }
            set
            {
                message = value;
                NotifyPropertyChanged("Message");
            }
        }

        public SelectedVisualViewModel SelectedVisual
        {
            get 
            {
                return selectedVisual;
            }
        }

        public Boolean HasChanged
        {
            get
            {
                if (techneModel == null)
                    return false;

                return techneModel.HasChanged;
            }
            set { }
        }

        public FrameworkElement TextureViewerOverlay
        {
            get { return TextureViewModel.Overlay; }
            set
            {
                TextureViewModel.Overlay = value;
                NotifyPropertyChanged(textureOverlayChangeArgs);
                NotifyPropertyChanged(textureViewerChangeArgs);
            }
        }

        public FrameworkElement HelixView
        {
            get { return helixView; }
            set
            {
                helixView = value as HelixView3D;
                NotifyPropertyChanged(helixViewChangeArgs);
            }
        }

        public TextureViewerViewModel TextureViewModel
        {
            get { return textureViewModel; }
            set
            {
                textureViewModel = value;
                NotifyPropertyChanged(textureViewerChangeArgs);
            }
        }

        public BitmapSource Texture
        {
            get
            {
                if (TextureViewModel.Texture == null)
                {
                    TextureViewModel.Texture = CreateEmptyTexture();
                }
                return TextureViewModel.Texture;
            }
            set
            {
                TextureViewModel.Texture = value;

                foreach (var item in model.Children)
                {
                    var techneVisual = item as ITechneVisual;

                    if (techneVisual == null)
                        continue;

                    ((Visual3D) techneVisual).Dispatcher.InvokeIfRequired(() => { techneVisual.Texture = value; });
                }

                NotifyPropertyChanged(textureChangeArgs);
                NotifyPropertyChanged(textureViewerChangeArgs);
            }
        }

        public Vector3D GlScale
        {
            get { return activeModel.GlScale; }
            set
            {
                model.Transform = new ScaleTransform3D(value);
                activeModel.GlScale = value;
                NotifyPropertyChanged(glScaleChangeArgs);
            }
        }

        public Double GlScaleX
        {
            get { return activeModel.GlScale.X; }
            set
            {
                GlScale = new Vector3D(value, activeModel.GlScale.Y, activeModel.GlScale.Z);
                NotifyPropertyChanged(glScaleXChangeArgs);
            }
        }

        public Double GlScaleY
        {
            get { return activeModel.GlScale.Y; }
            set
            {
                GlScale = new Vector3D(activeModel.GlScale.X, value, activeModel.GlScale.Z);
                NotifyPropertyChanged(glScaleYChangeArgs);
            }
        }

        public Double GlScaleZ
        {
            get { return activeModel.GlScale.Z; }
            set
            {
                GlScale = new Vector3D(activeModel.GlScale.X, activeModel.GlScale.Y, value);
                NotifyPropertyChanged(glScaleZChangeArgs);
            }
        }

        #region MenuItems
        public DispatcherNotifiedObservableCollection<ShapeToolItemViewModel> ShapeToolBarItems
        {
            get;
            set;
        }

        public DispatcherNotifiedObservableCollection<ToolToolItemViewModel> ToolToolBarItems
        {
            get;
            set;
        }

        public DispatcherNotifiedObservableCollection<ImportPluginMenuItemViewModel> ImportPluginMenuItems
        {
            get;
            set;
        }

        public DispatcherNotifiedObservableCollection<ExportPluginMenuItemViewModel> ExportPluginMenuItems
        {
            get;
            set;
        }
        #endregion

        #region Model
        public DispatcherNotifiedObservableCollection<Visual3D> Model
        {
            get;
            private set;
        }

        public DispatcherNotifiedObservableCollection<ITechneVisual> TechneModel
        {
            get;
            private set;
        }
        #endregion

        private WriteableBitmap CreateEmptyTexture()
        {
            int width = (int)activeModel.TextureSize.X;
            int height = (int)activeModel.TextureSize.Y;

            width = width != 0 ? width : 64;
            height = height != 0 ? height : 32;

            var emptyTexture = new WriteableBitmap(width,
                                                   height,
                                                   96,
                                                   96,
                                                   PixelFormats.Gray16,
                                                   new BitmapPalette(
                                                       new List<Color>
                                                       {
                                                           Colors.Gray
                                                       }));
            var pixelCount = emptyTexture.PixelHeight * emptyTexture.PixelWidth * emptyTexture.Format.BitsPerPixel / 8;
            var pixels = new byte[pixelCount];
            for (int i = 0; i < pixelCount; i++)
            {
                pixels[i] = 0xF0;
            }
            emptyTexture.WritePixels(new Int32Rect(0, 0, width, height),
                                     pixels,
                                     emptyTexture.PixelWidth * emptyTexture.Format.BitsPerPixel / 8,
                                     0);
            return emptyTexture;
        }
        #endregion
    }
}

