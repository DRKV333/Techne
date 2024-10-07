/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Techne.Plugins.Interfaces;

namespace Techne.Controls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Techne.Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Techne.Controls;assembly=Techne.Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:TechneTreeView/>
    ///
    /// </summary>
    public class TechneTreeView : TreeView
    {
        private readonly List<ITechneVisual> selectedItems;

        static TechneTreeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(TechneTreeView), new FrameworkPropertyMetadata(typeof(TechneTreeView)));
        }

        public TechneTreeView()
        {
            selectedItems = new List<ITechneVisual>();
        }

        public List<ITechneVisual> SelectedItems
        {
            get { return selectedItems; }
        }

        public bool CanSelectMultipleItems { get { return true; } }

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue == null)
            {
                ((ITechneVisual)e.OldValue).IsSelected = false;
                SelectedItems.Remove((ITechneVisual)e.OldValue);
            }
            else
            {
                if (e.OldValue != null && !(Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                {
                    if (e.OldValue is ITechneVisual)
                    {
                        ((ITechneVisual)e.OldValue).IsSelected = false;
                        SelectedItems.Remove((ITechneVisual)e.OldValue);
                    }
                    else if (e.OldValue is System.Collections.IList)
                    {
                        foreach (var child in e.OldValue as System.Collections.IList)
                        {
                            if (child is ITechneVisual)
                            {
                                ((ITechneVisual)child).IsSelected = false;
                                SelectedItems.Remove((ITechneVisual)child);
                            }
                        }
                    }
                }
                ((ITechneVisual)e.NewValue).IsSelected = true;
                SelectedItems.Add((ITechneVisual)e.NewValue);
            }

            var selectedItem = (SelectedItem as TreeViewItem);

            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
            }

            if (selectedItems.Count > 1)
            {
                typeof(TreeView).GetMethod("SetSelectedItem", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(this, new object[] { new List<ITechneVisual>(selectedItems) });
                RoutedPropertyChangedEventArgs<object> newE = new RoutedPropertyChangedEventArgs<object>(e.OldValue, selectedItems, SelectedItemChangedEvent);
                base.OnSelectedItemChanged(newE);
            }
            else
            {
                base.OnSelectedItemChanged(e);
            }
        }
    }
}

