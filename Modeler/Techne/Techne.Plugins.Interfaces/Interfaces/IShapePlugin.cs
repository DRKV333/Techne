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
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows;

namespace Techne.Plugins.Interfaces
{
    public interface IShapePlugin : ITechnePlugin
    {
        BitmapSource Icon { get; }
        String AltText { get; }

        ITechneVisual CreateVisual(String name = "New Shape", Vector3D size = new Vector3D(), float scale = 0, Vector3D position = new Vector3D(), Vector3D offset = new Vector3D(), Vector textureOffset = new Vector(), Vector3D rotation = new Vector3D(), bool isMirrored = false, bool isDecorative = false, Dictionary<string, MetadataBase> metadata = null);
        ITechneVisual CreateVisual(Rect3D size);
        ITechneVisual CreateVisual(double x, double y, double z, double height, double widht, double length);

        Panel GetTextureViewerOverlay(ITechneVisual visual, bool isSelectedVisual);
    }
}

