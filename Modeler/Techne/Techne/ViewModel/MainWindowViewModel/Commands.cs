/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using Cinch;
using Techne.Manager;
using Techne.Plugins.Interfaces;
using Techne.ViewModel;

namespace Techne
{
    public partial class MainWindowViewModel
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Local
        #region Commands
        public SimpleCommand<Object, Object> MouseDownCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> SaveCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> ClearCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> SaveAsCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> LoadTextureCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> KeyUpCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> ChangeSelectedVisualCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> ResetValueCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> QuitCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenAboutWindowCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> AnimateCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> SelectedElementChangedCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> NewModelCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenSettingsCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> ToggleShowcaseCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> SendFeedbackCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenBackupFolderCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> MouseMoveCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> MouseButtonUpCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenAboutViewCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> DeleteCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> PasteCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> SelectAllCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> UndoCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> RedoCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> PasteCoordinatesCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> EditProjectCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenDonateCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenChangeLogViewCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenIrcCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> OpenHelpCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> CutCommand
        {
            get;
            private set;
        }

        public SimpleCommand<Object, Object> CopyCommand
        {
            get;
            private set;
        }

        #endregion
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

        private void InitializeCommands()
        {
            MouseDownCommand = new SimpleCommand<Object, Object>(ExecuteMouseDownCommand);
            LoadTextureCommand = new SimpleCommand<Object, Object>(ExecuteLoadTextureCommand);
            OpenCommand = new SimpleCommand<Object, Object>(ExecuteOpenCommand);
            SaveCommand = new SimpleCommand<Object, Object>(ExecuteSaveCommand);
            SaveAsCommand = new SimpleCommand<Object, Object>(ExecuteSaveAsCommand);
            ClearCommand = new SimpleCommand<Object, Object>(ExecuteClearCommand);
            QuitCommand = new SimpleCommand<Object, Object>(ExecuteQuitCommand);
            KeyUpCommand = new SimpleCommand<Object, Object>(ExecuteKeyUpCommand);
            ChangeSelectedVisualCommand = new SimpleCommand<Object, Object>(ExecuteChangeSelectedVisualCommand);
            SelectedElementChangedCommand = new SimpleCommand<object, object>(ExecuteSelectedElementChangedCommand);
            NewModelCommand = new SimpleCommand<object, object>(ExecuteNewModelCommand);
            OpenSettingsCommand = new SimpleCommand<object, object>(ExecuteOpenSettingsCommand);
            ToggleShowcaseCommand = new SimpleCommand<object, object>(ExecuteToggleShowcaseCommand);
            SendFeedbackCommand = new SimpleCommand<object, object>(ExecuteSendFeedbackCommand);
            OpenBackupFolderCommand = new SimpleCommand<object, object>(ExecuteOpenBackupFolderCommand);
            MouseMoveCommand = new SimpleCommand<object, object>(ExecuteMouseMoveCommand);
            MouseButtonUpCommand = new SimpleCommand<object, object>(ExecuteMouseButtonUpCommand);
            OpenAboutViewCommand = new SimpleCommand<object, object>(ExecuteOpenAboutViewCommand);
            DeleteCommand = new SimpleCommand<object, object>(ExecuteDeleteCommand);
            PasteCommand = new SimpleCommand<object, object>(ExecutePasteCommand);
            OpenDonateCommand = new SimpleCommand<object, object>(ExecuteOpenDonateCommand);
            OpenChangeLogViewCommand = new SimpleCommand<object, object>(ExecuteOpenChangelogCommand);
            SelectAllCommand = new SimpleCommand<object, object>(ExecuteSelectAllCommand);
            PasteCoordinatesCommand = new SimpleCommand<object, object>(ExecutePasteCoordinatesCommand);
            EditProjectCommand = new SimpleCommand<object, object>(ExecuteEditProjectCommand);
            OpenIrcCommand = new SimpleCommand<object, object>(ExecuteOpenIrcCommand);
            OpenHelpCommand = new SimpleCommand<object, object>(ExecuteOpenHelpCommand);
            
            CopyCommand = new SimpleCommand<object, object>(ExecuteCopyCommand);


            AnimateCommand = new SimpleCommand<object, object>(ExecuteAnimateCommand);
            //Model.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Model_CollectionChanged);
        }

        #region Command Handler

        #region Tools
        internal void AddNewShapeCommand(Object obj)
        {
            var guid = obj as Guid?;
            if (!guid.HasValue)
                return;

            AddShape(guid.Value.ToString());
        }

        private void ExecuteLoadTextureCommand(Object obj)
        {
            //openFileService.InitialDirectory = @"C:\";
            openFileService.Filter = "*.png | *.png";

            var result = openFileService.ShowDialog(null);
            if (result.HasValue && result.Value)
            {
                string fileName = openFileService.FileName;

                //watcher = new FileSystemWatcher();
                watcher.EnableRaisingEvents = false;
                LoadTexture(fileName);
                watcher.Path = Path.GetDirectoryName(fileName);
                watcher.Filter = Path.GetFileName(fileName);
                watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.LastWrite;
                watcher.EnableRaisingEvents = true;
            }
        }

        private void ExecuteChangeSelectedVisualCommand(Object obj)
        {
            //var eventArgs = obj as EventToCommandArgs;

            //if (eventArgs == null)
            //    return;

            //var selectionArgs = eventArgs.EventArgs as SelectionChangedEventArgs;

            //if (selectionArgs == null)
            //    return;

            //SelectedVisual = (ITechneVisual)selectionArgs.AddedItems[0];
        }

        private void ExecuteClearCommand(Object obj)
        {
            ClearModel();
        }
        #endregion

        #region Menu

        #region File

        private void ExecuteQuitCommand(Object obj)
        {
            Application.Current.Shutdown();
        }

        internal void ExecuteNewModelCommand(Object obj)
        {
            //ClearModel();
            var viewModel = new NewProjectViewModel(this);

            OpenDialog(viewModel, "NewProjectView");
        }

        internal void ExecuteOpenCommand(Object obj)
        {
            try
            {
                openFileService.FileName = "";
                openFileService.Filter = "Techne Model|*tcn; *.zip";

                var result = openFileService.ShowDialog(null);

                if (result.HasValue && result.Value)
                {
                    string filename = openFileService.FileName;

                    OpenModel(filename);

                    //Model = tmp;
                }
            }
            catch (Exception x)
            {
                throw;
            }

        }

        internal void ExecuteSaveCommand(Object obj)
        {
            if (String.IsNullOrEmpty(techneModel.Location))
                ExecuteSaveAsCommand(obj);
            else
                SaveModel(techneModel.Location);
        }

        internal void ExecuteSaveAsCommand(Object obj)
        {
            saveFileService.Filter = "Techne Model|*.tcn";
            saveFileService.OverwritePrompt = true;

            var result = saveFileService.ShowDialog(null);
            if (result.HasValue && result.Value)
            {
                string savedFileName = saveFileService.FileName;

                SaveModel(savedFileName);
            }
        }

        internal void ExecuteExportCommand(Object obj)
        {
            var plugin = obj as IExportPlugin;

            if (plugin == null)
                return;

            saveFileService.FileName = techneModel.Name;
            saveFileService.Filter = plugin.Filter;
            saveFileService.OverwritePrompt = true;

            var result = saveFileService.ShowDialog(null);
            if (result.HasValue && result.Value)
            {
                string savedFileName = saveFileService.FileName;

                if (File.Exists(savedFileName))
                    File.Delete(savedFileName);

                TechneStateManager.IsExporting = true;
                plugin.Export(savedFileName, activeModel.Geometry, ExtensionManager.ShapePlugins, techneModel);
                TechneStateManager.IsExporting = false;
            }
        }

        internal void ExecuteImportCommand(Object obj)
        {
            var plugin = obj as IImportPlugin;

            if (plugin == null)
                return;

            openFileService.FileName = "";
            openFileService.Filter = plugin.Filter;

            var result = openFileService.ShowDialog(null);
            if (result.HasValue && result.Value)
            {
                string savedFileName = openFileService.FileName;

                var visuals = plugin.Import(ExtensionManager.ShapePlugins, savedFileName);

                if (visuals == null)
                    return;

                AddVisual(visuals);
            }
        }
        #endregion

        #region Edit
        internal void ExecuteEditProjectCommand(Object obj)
        {
            var viewModel = new EditProjectViewModel(this, techneModel);

            visualizerService.ShowDialog("EditProjectView", viewModel);
        }

        internal void ExecutePasteCoordinatesCommand(Object o)
        {
            PasteCopiedBox(copiedModel, true);
        }

        internal void ExecuteDeleteCommand(Object obj)
        {
            RemoveVisual(selectedModel.SelectedVisual);
        }

        internal void ExecuteClearAllCommand(Object obj)
        {
            ClearModel();
        }

        internal void ExecuteSelectAllCommand(Object obj)
        {
            SelectAll();
        }

        internal void ExecuteFocusSelectedVisualCommand(Object obj)
        {
            FocusSelectedVisual();
        }

        internal void ExecuteCopyCommand(Object obj)
        {
            CopyBox();
        }

        internal void ExecutePasteCommand(Object obj)
        {
            PasteCopiedBox(copiedModel, Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift));
        }

        private void CopyBox()
        {
            copiedModel = CloneVisual(selectedVisual.SelectedVisuals).ToList();
        }

        private void PasteCopiedBox(System.Collections.Generic.IEnumerable<ITechneVisual> visuals, bool copyPosition)
        {
            if (visuals == null)
                return;

            var copies = CloneVisual(visuals).ToList();

            foreach (var copy in copies)
            {
                if (!copyPosition)
                {
                    copy.Position = new Vector3D();
                    copy.Offset = new Vector3D();
                }

                if (copy is ITechneVisualCollection)
                {
                    AddItemsToModel(copy.Children);
                }

                AddVisual(copy);       
            }
        }
        #endregion

        #region Tools
        internal void ExecuteOpenSettingsCommand(Object obj)
        {
            var viewModel = new SettingsViewModel(settingsManager);

            visualizerService.ShowDialog("SettingsView", viewModel);
        }

        internal void ExecuteToggleShowcaseCommand(Object obj)
        {
            DeselectVisual();

            if (!showcaseActive)
            {
                Model.Remove(gridLines);
                Model.Remove(ground);
            }
            else
            {
                Model.Add(gridLines);
                Model.Add(ground);
            }

            showcaseActive = !showcaseActive;

            //var a1 = new Point3DAnimation(helixView.Camera.Position, newPosition,
            //        new Duration(TimeSpan.FromMilliseconds(animationTime))) { AccelerationRatio = 0.3, DecelerationRatio = 0.5, FillBehavior = FillBehavior.Stop };
            //camera.BeginAnimation(ProjectionCamera.PositionProperty, a1);

            //var a2 = new Vector3DAnimation(fromDirection, newDirection,
            //                               new Duration(TimeSpan.FromMilliseconds(animationTime))) { AccelerationRatio = 0.3, DecelerationRatio = 0.5, FillBehavior = FillBehavior.Stop };
            //camera.BeginAnimation(ProjectionCamera.LookDirectionProperty, a2);

            //var a3 = new Vector3DAnimation(fromUpDirection, newUpDirection,
            //                               new Duration(TimeSpan.FromMilliseconds(animationTime))) { AccelerationRatio = 0.3, DecelerationRatio = 0.5, FillBehavior = FillBehavior.Stop };
            //camera.BeginAnimation(ProjectionCamera.UpDirectionProperty, a3);

            helixView.InfiniteSpin = showcaseActive;
            helixView.CameraController.SpinCamera(new Vector(75, 10));
        }

        internal void ExecuteOpenBackupFolderCommand(Object obj)
        {
            // suppose that we have a test.txt at E:\
            string filePath;

            if (backupManager == null || String.IsNullOrEmpty(backupManager.BackupPath))
                filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Techne");
            else
                filePath = backupManager.BackupPath;

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            // combine the arguments together
            // it doesn't matter if there is a space after ','
            string argument = @"/root, " + filePath;

            Process.Start("explorer.exe", argument);
        }
        #endregion

        #region Help
        internal void ExecuteOpenIrcCommand(Object obj)
        {
            const string target = "http://webchat.esper.net/?nick=&channels=techne";

            try
            {
                Process.Start(target);
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        internal void ExecuteOpenHelpCommand(Object obj)
        {
            const string target = "http://techne.wikkii.com/wiki/Main_Page";

            try
            {
                Process.Start(target);
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        internal void ExecuteOpenAboutViewCommand(Object obj)
        {
            OpenDialog(new AboutViewModel(), "AboutView");
        }

        internal void ExecuteOpenChangelogCommand(Object obj)
        {
            ShowUpdateDialog(updateManager.GetChangelog(false));
        }
        #endregion

        private void ExecuteOpenDonateCommand(Object obj)
        {
            const string target = "https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=JL5NPD987VZFJ&lc=US&item_name=Techne%20donation&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted";

            try
            {
                Process.Start(target);
            }
            catch (Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        internal void ExecuteSendFeedbackCommand(Object obj)
        {
            OpenDialog(new FeedbackViewModel(), "FeedbackView");
        }

        internal void ExecuteSelectedElementChangedCommand(Object obj)
        {
            var eventArgs = obj as EventToCommandArgs;

            if (eventArgs == null)
                return;

            var propertyChangedArgs = eventArgs.EventArgs as RoutedPropertyChangedEventArgs<object>;

            if (propertyChangedArgs == null)
                return;

            if (propertyChangedArgs.NewValue != propertyChangedArgs.OldValue)
                SelectedVisual.SelectVisual(propertyChangedArgs.NewValue as ITechneVisual);
        }
        #endregion

        #region Global
        private void ExecuteMouseButtonUpCommand(Object obj)
        {
            var args = obj as EventToCommandArgs;
            //HelixToolkit.HelixView3D helixView = args.Sender as HelixToolkit.HelixView3D;

            if (args == null || helixView == null || hitTestManager == null)
                return;

            var mouseArgs = args.EventArgs as MouseButtonEventArgs;

            if (mouseArgs == null)
                return;

            hitTestManager.MouseButtonUp(mouseArgs);
        }

        private void ExecuteMouseMoveCommand(Object obj)
        {
            //hitTestManager.MouseMoved();
        }

        private void ExecuteMouseDownCommand(Object obj)
        {
            var args = obj as EventToCommandArgs;
            //HelixToolkit.HelixView3D helixView = args.Sender as HelixToolkit.HelixView3D;

            if (args == null || helixView == null)
                return;

            helixView.Focus();

            var mouseArgs = args.EventArgs as MouseButtonEventArgs;

            if (mouseArgs == null)
                return;

            if (hitTestManager == null)
                throw new NullReferenceException("Hittest-manager was null. WHY?!");

            hitTestManager.HitTest(mouseArgs, helixView);
        }

        private void ExecuteKeyUpCommand(Object obj)
        {
            var commandArgs = obj as EventToCommandArgs;

            if (commandArgs == null || (commandArgs.CommandParameter as bool?).HasValue && ((bool?)commandArgs.CommandParameter).Value)
                return;

            var keyEventArgs = commandArgs.EventArgs as KeyEventArgs;

            if (keyEventArgs == null)
                return;

            if (keyEventArgs.Key == Key.Delete)
            {
                if (Keyboard.Modifiers == ModifierKeys.Shift)
                {
                    ExecuteClearAllCommand(null);
                }
                else
                {
                    ExecuteDeleteCommand(null);
                }
            }
            else if (keyEventArgs.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ExecuteOpenCommand(null);
            }
            else if (keyEventArgs.Key == Key.S && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ExecuteSaveCommand(null);
            }
            else if (keyEventArgs.Key == Key.E && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ExecuteExportCommand(null);
            }
            else if (keyEventArgs.Key == Key.C && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ExecuteCopyCommand(null);
            }
            else if (keyEventArgs.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
            }
            else if (keyEventArgs.Key == Key.Y && Keyboard.Modifiers == ModifierKeys.Control)
            {
            }
            else if (keyEventArgs.Key == Key.N && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AddShape("D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower());
            }
            else if (keyEventArgs.Key == Key.A && Keyboard.Modifiers == ModifierKeys.Control)
            {
                ExecuteSelectAllCommand(null);
            }
            else if (keyEventArgs.Key == Key.Z)
            {
                FocusSelectedVisual();
            }
            else if (keyEventArgs.Key == Key.PageDown)
            {
            }
            else if (keyEventArgs.Key == Key.PageUp)
            {
            }
            else if (keyEventArgs.Key == Key.X)
            {
            }
            else if (keyEventArgs.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control || Keyboard.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift))
            {
                ExecutePasteCommand(null);
            }
        }

        private void ExecuteAnimateCommand(Object obj)
        {
            foreach (var item in TechneModel.ToList())
            {
                if (item.AnimationDuration == null || item.AnimationDuration.LengthSquared == 0 || Double.IsNaN(item.AnimationDuration.LengthSquared))
                    continue;

                if (item.AnimationAngles == null || item.AnimationAngles.LengthSquared == 0 || Double.IsNaN(item.AnimationAngles.LengthSquared))
                    continue;

                TechneRotation3DAnimation animation = new TechneRotation3DAnimation();

                animation.To = item.AnimationAngles;
                animation.From = -item.AnimationAngles;
                animation.Default = new Vector3D(item.RotationX, item.RotationY, item.RotationZ);
                animation.AxisDuration = item.AnimationDuration;
                animation.AutoReverse = false;

                var oldTransform = item.Transform;

                ITechneVisual copiedItem = item;
                animation.Completed += (s, e) => { copiedItem.Transform = oldTransform; };

                RotateTransform3D rt = new RotateTransform3D
                                       {
                                           CenterX = (item).Position.X,
                                           CenterY = (item).Position.Y,
                                           CenterZ = (item).Position.Z
                                       };

                item.Transform = rt;

                rt.BeginAnimation(RotateTransform3D.RotationProperty, animation);
            }


            //DeobfuscatedImporter import = new DeobfuscatedImporter("");
            //var result = import.Parse(extensionManager.ShapePlugins);

            //foreach (var item in result.Values)
            //{
            //    AddVisual(item);
            //}
        }

        private static RotateTransform3D CreateRotateTransform(Vector3D anchor)
        {
            RotateTransform3D rt = new RotateTransform3D
                                   {
                                       CenterX = anchor.X,
                                       CenterY = anchor.Y,
                                       CenterZ = anchor.Z
                                   };
            return rt;
        }

        private static Rotation3DAnimation CreateAnimation(Vector3D axis, double angle, double duration)
        {
            Rotation3DAnimation myDoubleAnimation = new Rotation3DAnimation
                                                    {
                                                        From = new AxisAngleRotation3D(axis, angle)
                                                    };

            myDoubleAnimation.From = new AxisAngleRotation3D(axis, -angle);

            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(duration));
            myDoubleAnimation.AutoReverse = true;
            myDoubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

            return myDoubleAnimation;
        }

        public double[] EulerToAxisAngle(double heading, double attitude, double bank)
        {
            heading *= Math.PI / 181;
            attitude *= Math.PI / 181;
            bank *= Math.PI / 181;

            double c1 = Math.Cos(heading / 2);
            double s1 = Math.Sin(heading / 2);
            double c2 = Math.Cos(attitude / 2);
            double s2 = Math.Sin(attitude / 2);
            double c3 = Math.Cos(bank / 2);
            double s3 = Math.Sin(bank / 2);
            double c1c2 = c1 * c2;
            double s1s2 = s1 * s2;
            var w = c1c2 * c3 - s1s2 * s3;
            var x = c1c2 * s3 + s1s2 * c3;
            var y = s1 * c2 * c3 + c1 * s2 * s3;
            var z = c1 * s2 * c3 - s1 * c2 * s3;
            var angle = 2 * Math.Acos(w);
            double norm = x * x + y * y + z * z;
            if (norm < 0.001)
            {
                // when all euler angles are zero angle =0 so
                // we can set axis to anything to avoid divide by zero
                x = 1;
                y = z = 0;
            }
            else
            {
                norm = Math.Sqrt(norm);
                x /= norm;
                y /= norm;
                z /= norm;
            }

            return new[] {x, y, z, angle * 181 / Math.PI};
        }

        //public double[] Quater(Quaternion q1)
        //{
        //    double[] result = new double[4];

        //    if (q1.W > 1) q1.Normalize(); // if w>1 acos and sqrt will produce errors, this cant happen if quaternion is normalised
        //    result[3] = 2 * Math.Acos(q1.w);
        //    double s = Math.Sqrt(1 - q1.W * q1.W); // assuming quaternion normalised then w is less than 1, so term always positive.
        //    if (s < 0.001)
        //    { // test to avoid divide by zero, s is always positive due to sqrt
        //        // if s close to zero then direction of axis not important
        //        result[0] = q1.X; // if it is important that axis is normalised then replace with x=1; y=z=0;
        //        result[1] = q1.Y;
        //        result[2] = q1.Z;
        //    }
        //    else
        //    {
        //        result[0] = q1.X / s; // normalise axis
        //        result[1] = q1.Y / s;
        //        result[2] = q1.Z / s;
        //    }

        //    return result;
        //}
        #endregion

        #endregion
    }
}

