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
//using System.Windows.Media.Imaging;
//using System.Windows.Controls;
//using Techne.Models;
//using System.Windows;
//using System.Collections.ObjectModel;
//using System.Windows.Media.Media3D;
//using System.Text.RegularExpressions;

//namespace Techne.Plugins.FileHandler.Texturemap
//{
//    [PluginExportAttribute("Texture-Offset autoaligner", "0.1", "ZeuX", "", "DB9C4862-6E63-4642-8D88-3024256F026E")]
//    [Export(typeof(IToolPlugin))]
//    public class TextureOffsetAutoAlign : IToolPlugin
//    {
//        //maybe just the vector is enough - but who knows what I want to do in future
//        Dictionary<String, ITechneVisual> visualNames;
//        Regex nameRegex;
//        bool isRunning = false;
//        private System.Guid guid = new Guid("DB9C4862-6E63-4642-8D88-3024256F026E");

//        public System.Windows.Media.Imaging.BitmapSource Icon
//        {
//            get
//            {
//                return null;
//            }
//            set
//            {
                
//            }
//        }

//        public string MenuHeader
//        {
//            get
//            {
//                return "Align Texture offset";
//            }
//            set
//            {
//            }
//        }

//        public void OnClick(SaveModel saveModel, Dictionary<string, IShapePlugin> shapes)
//        {
//            if (isRunning)
//                return;

//            isRunning = true;

//            visualNames = new Dictionary<String, ITechneVisual>();
//            nameRegex = new Regex(@"([A-Za-z0-9]+[A-Za-z]+)([0-9]*)");

//            var dynamicCanvas = new Mapper.Canvas();
//            dynamicCanvas.SetCanvasDimensions(64, 32);

//            OffsetVisuals(dynamicCanvas, saveModel.Geometry, shapes);

//            nameRegex = null;
//            visualNames = null;
//            isRunning = false;
//        }

//        //need to think about bigger textures
//        private void OffsetVisuals(Mapper.Canvas dynamicCanvas, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes)
//        {
//            foreach (var item in visuals)
//            {
//                string guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();

//                if (item.Guid.ToString().Equals("9EC93754-B48D-4C70-AE4E-84D81AE55396", StringComparison.InvariantCultureIgnoreCase))
//                {
//                    OffsetVisuals(dynamicCanvas, ((ModelVisual3D)item).Children.Cast<ITechneVisual>(), shapes);
//                }
//                else
//                {
//                    guid = item.Guid.ToString().ToLower();
//                    if (!shapes.ContainsKey(guid))
//                        continue;

//                    var result = nameRegex.Match(item.Name);
//                    var pureName = result.Groups[1].Value;

//                    if (visualNames.ContainsKey(pureName))
//                    {
//                        item.TextureOffset = visualNames[pureName].TextureOffset;
//                    }
//                    else
//                    {
//                        visualNames.Add(pureName, item);

//                        var overlay = shapes[guid].GetTextureViewerOverlay(item);
//                        int offsetX = 0;
//                        int offsetY = 0;
//                        int heightDeficit = 0;

//                        dynamicCanvas.AddRectangle((int)overlay.Width, (int)overlay.Height, out offsetX, out offsetY, out heightDeficit);

//                        item.TextureOffset = new Vector((double)offsetX, (double)offsetY);
//                    }
//                }
//            }
//        }

//        public Guid Guid
//        {
//            get { return guid; }
//        }

//        public void OnLoad()
//        {
            
//        }
//    }
//}

