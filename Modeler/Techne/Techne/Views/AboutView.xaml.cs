/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using Cinch;

namespace Techne
{
    /// <summary>
    /// Interaction logic for AboutView.xaml
    /// </summary>
    [PopupNameToViewLookupKeyMetadata("AboutView", typeof(AboutView))]
    public partial class AboutView : Window
    {
        public AboutView()
        {
            InitializeComponent();

            // Insert code required on object creation below this point.
        }
    }
}

