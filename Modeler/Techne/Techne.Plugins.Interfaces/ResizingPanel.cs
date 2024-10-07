/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;

namespace Techne.Plugins
{
    public class ResizingPanel : Panel
    {
        //private double scale;

        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(Double), typeof(ResizingPanel));
        public static readonly DependencyProperty UnScaledHeightProperty = DependencyProperty.Register("UnScaledHeight", typeof(Double), typeof(ResizingPanel));
        public static readonly DependencyProperty UnScaledWidthProperty = DependencyProperty.Register("UnScaledWidth", typeof(Double), typeof(ResizingPanel));

        public Double Scale
        {
            get
            {
                return (Double)GetValue(ScaleProperty);
            }
            set
            {
                SetValue(ScaleProperty, value);
            }
        }
        public Double UnScaledHeight
        {
            get
            {
                return (Double)GetValue(UnScaledHeightProperty);
            }
            set
            {
                SetValue(UnScaledHeightProperty, value);
            }
        }
        public Double UnScaledWidth
        {
            get
            {
                return (Double)GetValue(UnScaledWidthProperty);
            }
            set
            {
                SetValue(UnScaledWidthProperty, value);
            }
        }

        public ResizingPanel()
        {
            Binding scaleBinding = new Binding("Scale");
            scaleBinding.Source = this;
            Binding heightBinding = new Binding("UnScaledHeight");
            heightBinding.Source = this;
            Binding widthBinding = new Binding("UnScaledWidth");
            widthBinding.Source = this;
            
            MultiBinding scaledHeight = new MultiBinding();
            scaledHeight.Bindings.Add(heightBinding);
            scaledHeight.Bindings.Add(scaleBinding);
            scaledHeight.Converter = new Techne.Plugins.ValueConverter.ScaleValueConverter();

            MultiBinding scaledWidth = new MultiBinding();
            scaledWidth.Bindings.Add(widthBinding);
            scaledWidth.Bindings.Add(scaleBinding);
            scaledWidth.Converter = new Techne.Plugins.ValueConverter.ScaleValueConverter();

            this.SetBinding(ResizingPanel.HeightProperty, scaledHeight);
            this.SetBinding(ResizingPanel.WidthProperty, scaledWidth);
        }
    }
}

