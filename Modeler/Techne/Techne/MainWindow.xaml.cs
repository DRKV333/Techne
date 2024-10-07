/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.ComponentModel;
using System.Windows;
using Techne.Properties;

namespace Techne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ConfigureWindow();
        }

        private void ConfigureWindow()
        {
            Closing += MainWindow_Closing;
            WindowStartupLocation = WindowStartupLocation.Manual;

            WindowState = Settings.Default.WindowState;

            if (WindowState == WindowState.Normal)
            {
                Width = Settings.Default.WindowWith;
                Height = Settings.Default.WindowHeight;
                Top = Settings.Default.WindowTop;
                Left = Settings.Default.WindowLeft;
            }

            MoveIntoView();
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        public void MoveIntoView()
        {
            if (Top + Width / 2 > SystemParameters.VirtualScreenHeight)
            {
                Top = SystemParameters.VirtualScreenHeight - Height;
            }

            if (Left + Width / 2 > SystemParameters.VirtualScreenWidth)
            {
                Left = SystemParameters.VirtualScreenWidth - Width;
            }

            if (Top < 0)
            {
                Top = 0;
            }

            if (Left < 0)
            {
                Left = 0;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            var vm = DataContext as MainWindowViewModel;

            if (vm != null && vm.HasChanged)
            {
                string messageBoxText = "You have unsaved changes, do you really want to quit?";
                string caption = "Techne";
                MessageBoxButton button = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Warning;

                MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon);

                switch (result)
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.No:
                        e.Cancel = true;
                        return;
                        break;
                    case MessageBoxResult.None:
                        break;
                    case MessageBoxResult.OK:
                        break;
                    case MessageBoxResult.Yes:
                        break;
                    default:
                        break;
                }
            }

            Settings.Default.WindowState = WindowState;

            Settings.Default.WindowWith = Width;
            Settings.Default.WindowHeight = Height;
            Settings.Default.WindowTop = Top;
            Settings.Default.WindowLeft = Left;

            Settings.Default.Save();

            base.OnClosing(e);
        }

        private void Focus_Helix(object sender, RoutedEventArgs e)
        {
            HelixView.Focus();
        }
    }
}

