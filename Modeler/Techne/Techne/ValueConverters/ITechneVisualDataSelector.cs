/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using System.Windows.Controls;
using Techne.Model;
using Techne.Plugins.Interfaces;

namespace Techne.ValueConverters
{
    public class ITechneVisualDataSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //DataTemplate dataTemplate = new DataTemplate();

            //Binding binding = new Binding();
            //binding.Path = new PropertyPath("Name");
            //binding.Mode = BindingMode.OneWay;
            //binding.Source = item;

            ////get textblock object from factory and set binding
            //FrameworkElementFactory textElement = new FrameworkElementFactory(typeof(TextBlock));
            //textElement.SetBinding(TextBlock.TextProperty, binding);

            //dataTemplate.VisualTree = textElement;

            //return dataTemplate;

            if (item is TechneVisualCollection)
                return ((FrameworkElement)container).TryFindResource("TechneVisualCollectionDataTemplate") as HierarchicalDataTemplate;

            if (item is TechneVisualFolder)
                return ((FrameworkElement)container).TryFindResource("TechneVisualFolderDataTemplate") as HierarchicalDataTemplate;

            if (item is ITechneVisual)
                return ((FrameworkElement)container).TryFindResource("ITechneVisualDataTemplate") as HierarchicalDataTemplate;

            return null;
        }
    }
}

