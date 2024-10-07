/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Reflection;
using System.Windows.Controls;

namespace Techne
{
    public static class TreeViewExtensions
    {
        public static void SetSelectedItem(this TreeView control, object item)
        {
            try
            {
                //do soemthing else with that
                if (item == null)
                {
                    for (int i = 0; i < control.Items.Count; i ++)
                    {
                        ((TreeViewItem)control.ItemContainerGenerator.ContainerFromIndex(i)).IsSelected = false;
                        ;
                    }
                }
                else
                {
                    var dObject = control.ItemContainerGenerator.ContainerFromItem(item);

                    //uncomment the following line if UI updates are unnecessary
                    ((TreeViewItem)dObject).IsSelected = true;

                    MethodInfo selectMethod = typeof(TreeViewItem).GetMethod("Select",
                                                                             BindingFlags.NonPublic | BindingFlags.Instance);

                    selectMethod.Invoke(dObject, new object[] {true});
                }
            }
            catch
            {
            }
        }
    }
}

