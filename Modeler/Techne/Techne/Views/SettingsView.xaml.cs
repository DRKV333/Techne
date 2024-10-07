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
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("SettingsView", typeof(SettingsView))]
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
        }
    }
}

