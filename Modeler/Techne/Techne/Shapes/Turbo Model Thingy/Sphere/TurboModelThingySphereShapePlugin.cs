/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using Techne.Plugins.Attributes;
using Techne.Plugins.Interfaces;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    [PluginExport("Turbo Model Thingy Sphere Shape", "0.1", "ZeuX", "Adds Support for Turbo Model Thingy's Sphere", "E1957603-6C07-4A1E-9047-BB1F45E57CEA")]
    public class TurboModelThingySphereShapePlugin : IShapePlugin
    {
        private readonly Guid guid = new Guid("E1957603-6C07-4A1E-9047-BB1F45E57CEA");

        #region IShapePlugin Members
        public BitmapSource Icon
        {
            get
            {
                var myAssembly = Assembly.GetExecutingAssembly();

                using (Stream stream = myAssembly.GetManifestResourceStream("Techne.Plugins.FileHandler.TurboModelThingy.Resources.AddSphere.png"))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    return bitmap;
                }

                return new BitmapImage();
            }
        }

        public string AltText
        {
            get { return "TMT-Sphere"; }
        }

        public ITechneVisual CreateVisual(string name = "New Shape",
                                          Vector3D size = new Vector3D(),
                                          float scale = 0,
                                          Vector3D position = new Vector3D(),
                                          Vector3D offset = new Vector3D(),
                                          Vector textureOffset = new Vector(),
                                          Vector3D rotation = new Vector3D(),
                                          bool isMirrored = false,
                                          bool isDecorative = false,
                                          Dictionary<string, MetadataBase> metadata = null)
        {
            if (size.X <= 0)
                size.X = 1;
            if (size.Y <= 0)
                size.Y = 1;
            if (size.Z <= 0)
                size.Z = 1;

            if (metadata == null)
                metadata = new Dictionary<string, MetadataBase>();

            return new TurboModelThingySphereVisual3D
                   {
                       Rotation = rotation,
                       IsDecorative = isDecorative,
                       IsMirrored = isMirrored,
                       TextureOffset = textureOffset,
                       Size = size,
                       Scale = scale,
                       Position = position,
                       Offset = offset,
                       Name = name,
                       Metadata = metadata
                   };
        }

        public ITechneVisual CreateVisual(Rect3D size)
        {
            throw new NotImplementedException();
        }

        public ITechneVisual CreateVisual(double x, double y, double z, double height, double widht, double length)
        {
            throw new NotImplementedException();
        }

        public Panel GetTextureViewerOverlay(ITechneVisual visual, bool isSelectedVisual)
        {
            return new Canvas();
        }

        public Guid Guid
        {
            get { return guid; }
        }

        public void OnLoad()
        {
        }
        #endregion
    }
}

