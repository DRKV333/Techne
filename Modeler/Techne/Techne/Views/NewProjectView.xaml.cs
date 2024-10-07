/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.ComponentModel;
using System.Windows;
using Cinch;
using Techne.ViewModel;

namespace Techne.Views
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("NewProjectView", typeof(NewProjectView))]
    public partial class NewProjectView : Window
    {
        public NewProjectView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            ((NewProjectViewModel)DataContext).ExecuteCreateModelCommand(null);
        }
    }
}

