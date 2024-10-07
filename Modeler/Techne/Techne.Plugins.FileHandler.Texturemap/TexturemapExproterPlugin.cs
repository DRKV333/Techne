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
using Techne.Plugins.Interfaces;
using Techne.Plugins.Attributes;
using System.ComponentModel.Composition;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using Techne.Models;

namespace Techne.Plugins.FileHandler.Texturemap
{
    [PluginExportAttribute("Texturemap exporter", "0.1", "ZeuX", "", "31697C9C-09B5-4854-AB63-3BA34A97FAF3")]
    [Export(typeof(IExportPlugin))]
    public class TexturemapExproterPlugin : IExportPlugin
    {
        private Guid guid = new Guid("31697C9C-09B5-4854-AB63-3BA34A97FAF3");

        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel saveModel)
        {
            int power = 6;

            Grid canvas = new Grid()
            {
                Width = 64,
                Height = 32
            };

            power = LoopVisuals(visuals, shapes, power, canvas);

            int width = (int)Math.Pow(2, power);
            int height = (int)Math.Pow(2, power - 1);

            canvas.Width = width;
            canvas.Height = height;

            if (Double.IsNaN(height))
                height = 0;
            if (Double.IsNaN(width))
                width = 0;

            canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            canvas.Arrange(new Rect(0, 0, width, height));
            canvas.UpdateLayout();

            var rtb = new RenderTargetBitmap(
                (int)width, //width 
                (int)height, //height 
                96, //dpi x 
                96, //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                ); 

            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename); 
        }

        private static int LoopVisuals(IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, int power, Grid canvas)
        {
            foreach (var item in visuals)
            {
                if (item.Guid.ToString().Equals("9EC93754-B48D-4C70-AE4E-84D81AE55396", StringComparison.InvariantCultureIgnoreCase))
                {
                    power = LoopVisuals((item as System.Windows.Media.Media3D.ModelVisual3D).Children.Cast<ITechneVisual>(), shapes, power, canvas);
                }
                else
                {
                    string guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();
                    if (shapes.ContainsKey(item.Guid.ToString().ToLower()))
                    {
                        guid = item.Guid.ToString().ToLower();
                    }

                    if (!shapes.ContainsKey(guid))
                        continue;

                    Panel overlay = shapes[guid].GetTextureViewerOverlay(item) as Panel;

                    overlay.Background = new SolidColorBrush(Colors.White);
                    overlay.Opacity = 0.5;

                    var children = new List<FrameworkElement>();

                    foreach (FrameworkElement child in overlay.Children)
                    {
                        children.Add(child);
                    }

                    foreach (FrameworkElement child2 in children)
                    {
                        child2.Margin = new Thickness(
                            child2.Margin.Left + overlay.Margin.Left,
                            child2.Margin.Top + overlay.Margin.Top,
                            child2.Margin.Right + overlay.Margin.Right,
                            child2.Margin.Bottom + overlay.Margin.Bottom
                            );
                        overlay.Children.Remove(child2);
                        canvas.Children.Add(child2);

                        while (child2.ActualHeight + child2.Margin.Top > Math.Pow(2, power - 1) || child2.ActualWidth + child2.Margin.Left > Math.Pow(2, power))
                        {
                            power++;
                        }
                    }
                }
            }
            return power;
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename) 
        { 
            var enc = new PngBitmapEncoder(); 
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using (var stm = System.IO.File.Create(filename)) 
            { 
                enc.Save(stm); 
            } 
        } 

        public void OnLoad()
        {
        }

        public string DefaultExtension
        {
            get { return "png"; }
        }

        public string Filter
        {
            get { return "png image|*.png"; }
        }

        public string MenuHeader
        {
            get { return "as texturemap"; }
        }

        public Guid Guid
        {
            get { return guid; }
        }
    }
}

