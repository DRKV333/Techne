/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Techne.Models;
using Techne.Plugins.Attributes;
using Techne.Plugins.Interfaces;

namespace Techne.Plugins.FileHandler.Texturemap
{
    [PluginExport("Texturemap exporter", "0.1", "ZeuX", "", "31697C9C-09B5-4854-AB63-3BA34A97FAF3")]
    [Export(typeof(IExportPlugin))]
    public class TexturemapExproterPlugin : IExportPlugin
    {
        private readonly Guid guid = new Guid("31697C9C-09B5-4854-AB63-3BA34A97FAF3");

        #region IExportPlugin Members
        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel saveModel)
        {
            var model = saveModel.Models.FirstOrDefault();

            int width = 64;
            int height = 64;

            if (model != null)
            {
                width = (int)model.TextureSize.X;
                height = (int)model.TextureSize.Y;
            }

            Grid canvas = new Grid()
                {
                    Width = width,
                    Height = height
                };

            LoopVisuals(visuals, shapes, canvas);

            canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            canvas.Arrange(new Rect(0, 0, width, height));
            canvas.UpdateLayout();

            var rtb = new RenderTargetBitmap(
                width,
                //width 
                height,
                //height 
                96,
                //dpi x 
                96,
                //dpi y 
                PixelFormats.Pbgra32 // pixelformat 
                );

            rtb.Render(canvas);

            SaveRTBAsPNG(rtb, filename);
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
        #endregion

        private static void LoopVisuals(IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, Grid canvas)
        {
            foreach (var item in visuals)
            {
                if (item is ITechneVisualCollection)
                {
                    LoopVisuals(item.Children, shapes, canvas);
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

                    Panel overlay = shapes[guid].GetTextureViewerOverlay(item, false);

                    overlay.Background = new SolidColorBrush(Colors.White);
                    overlay.Opacity = 0.5;

                    var children = overlay.Children.Cast<FrameworkElement>().ToList();

                    foreach (FrameworkElement child2 in children)
                    {
                        if (!(child2 is TextBlock))
                        {
                            child2.Margin = new Thickness(
                                child2.Margin.Left + overlay.Margin.Left,
                                child2.Margin.Top + overlay.Margin.Top,
                                child2.Margin.Right + overlay.Margin.Right,
                                child2.Margin.Bottom + overlay.Margin.Bottom
                                );
                        }
                        overlay.Children.Remove(child2);
                        canvas.Children.Add(child2);
                    }
                }
            }
        }

        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename)
        {
            var enc = new PngBitmapEncoder();
            enc.Frames.Add(BitmapFrame.Create(bmp));

            using (var stm = File.Create(filename))
            {
                enc.Save(stm);
            }
        }
    }
}

