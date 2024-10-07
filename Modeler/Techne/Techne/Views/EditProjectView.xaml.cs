/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using Cinch;

namespace Techne.Views
{
    /// <summary>
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("EditProjectView", typeof(EditProjectView))]
    public partial class EditProjectView : Window
    {
        public EditProjectView()
        {
            InitializeComponent();
        }
    }
}

