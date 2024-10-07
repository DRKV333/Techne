/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Diagnostics;
using System.Windows;
using Cinch;
using System.ComponentModel;

namespace Techne.Views
{
    /// <summary>
    /// Interaction logic for ChangelogView.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("ChangelogView", typeof(ChangelogView))]
    public partial class ChangelogView : Window
    {
        public ChangelogView()
        {
            InitializeComponent();

            try
            {
                this.browser.Navigate(@"http://dl.dropbox.com/u/15368593/Minecraft/Techne/Version/changelog.html");
            }
            catch
            {
                browser.NavigateToString("<html><head></head><body><center>I am sorry.<br />So sorry.<br />Something went wrong fetching the changelog.</center></body></html>");
            }

            browser.Navigating += browser_Navigating;
        }

        void browser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            e.Cancel = true;

            string target = e.Uri.ToString();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

