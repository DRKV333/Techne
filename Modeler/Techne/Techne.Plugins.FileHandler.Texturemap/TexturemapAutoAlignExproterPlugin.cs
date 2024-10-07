/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Techne.Plugins.Interfaces;
//using Techne.Plugins.Attributes;
//using System.ComponentModel.Composition;
//using System.Windows.Controls;
//using System.Windows.Media.Imaging;
//using System.Windows.Media;
//using System.Windows;
//using Techne.Models;
//using Techne.Model;

//namespace Techne.Plugins.FileHandler.Texturemap
//{
//    [PluginExportAttribute("Texturemap exporter", "0.1", "ZeuX", "", "DB9C4862-6E63-4642-8D88-3024256F026E")]
//    [Export(typeof(IExportPlugin))]
//    public class TexturemapAutoAlignExproterPlugin : IExportPlugin
//    {
//        private Guid guid = new Guid("DB9C4862-6E63-4642-8D88-3024256F026E");

//        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel saveModel)
//        {
//            int power = 6;
//            //List<Panel> overlays = new List<Panel>();

//            Grid canvas = new Grid()
//            {
//                Width = 64,
//                Height = 32
//            };

//            var dynamicCanvas = new Mapper.Canvas();
//            dynamicCanvas.SetCanvasDimensions(64, 32);

//            foreach (var item in visuals)
//            {
//                    string guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();
//                    if (shapes.ContainsKey(item.Guid.ToString().ToLower()))
//                    {
//                        guid = item.Guid.ToString().ToLower();
//                    }

//                    if (!shapes.ContainsKey(guid))
//                        continue;

//                    var overlay = shapes[guid].GetTextureViewerOverlay(item);
//                    int offsetX = 0;
//                    int offsetY = 0;
//                    int heightDeficit = 0;
//                    dynamicCanvas.AddRectangle((int)overlay.Width, (int)overlay.Height, out offsetX, out offsetY, out heightDeficit);

//                    if (heightDeficit > 0)
//                        power++;

//                    item.TextureOffset = new Vector((double)offsetX, (double)offsetY);

//                    overlay.Margin = new Thickness(
//                         overlay.Margin.Left + (double)offsetX,
//                         overlay.Margin.Top + (double)offsetY,
//                         overlay.Margin.Right - (double)offsetX,
//                         overlay.Margin.Bottom - (double)offsetY
//                        );
//            }

//            //AlignPanelsInCanvas(canvas, overlays);

//            int width = (int)Math.Pow(2, power);
//            int height = (int)Math.Pow(2, power - 1);

//            canvas.Width = width;
//            canvas.Height = height;

//            if (Double.IsNaN(height))
//                height = 0;
//            if (Double.IsNaN(width))
//                width = 0;

//            canvas.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
//            canvas.Arrange(new Rect(0, 0, width, height));
//            canvas.UpdateLayout();

//            var rtb = new RenderTargetBitmap(
//                (int)width, //width 
//                (int)height, //height 
//                96, //dpi x 
//                96, //dpi y 
//                PixelFormats.Pbgra32 // pixelformat 
//                ); 

//            rtb.Render(canvas);

//            SaveRTBAsPNG(rtb, filename); 
//        }

//        private void AlignPanelsInCanvas(Grid canvas, List<Panel> overlays)
//        {
            
//        }

//        private static void SaveRTBAsPNG(RenderTargetBitmap bmp, string filename) 
//        { 
//            var enc = new PngBitmapEncoder(); 
//            enc.Frames.Add(BitmapFrame.Create(bmp));

//            using (var stm = System.IO.File.Create(filename)) 
//            { 
//                enc.Save(stm); 
//            } 
//        } 

//        public void OnLoad()
//        {
//        }

//        public string DefaultExtension
//        {
//            get { return "png"; }
//        }

//        public string Filter
//        {
//            get { return "png image|*.png"; }
//        }

//        public string MenuHeader
//        {
//            get { return "as auto-aligned texturemap"; }
//        }

//        public Guid Guid
//        {
//            get { return guid; }
//        }
//    }
//}

