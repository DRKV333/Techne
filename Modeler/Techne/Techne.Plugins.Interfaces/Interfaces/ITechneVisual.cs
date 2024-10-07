/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace Techne.Plugins.Interfaces
{
    /// <summary>
    /// Basic interface for every Visual
    /// </summary>
    public interface ITechneVisual : IXmlSerializable
    {
        void CalculateCombinedPosition();
        Boolean IsSelected { get; set; }
        ITechneVisualCollection Parent { get; set; }
        ObservableCollection<ITechneVisual> Children { get; }
        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        Vector3D Offset { get; set; }
        Vector3D CombinedPosition { get; set;  }
        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        Vector3D Position { get; set; }
        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        /// <remarks>
        /// should be get only!
        /// </remarks>
        Point3D Center { get; set; }
        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        Vector TextureOffset { get; set; }
        /// <summary>
        /// Gets or sets the texture size of this visual
        /// </summary>
        Vector TextureSize { get; set; }
        /// <summary>
        /// Gets or sets the size of this visual
        /// </summary>
        Vector3D Size { get; set; }
        /// <summary>
        /// Gets or sets the texture of this visual
        /// </summary>
        BitmapSource Texture { get; set; }
        /// <summary>
        /// Gets or sets the name of this visual used for saving and especially exporting
        /// </summary>
        String Name { get; set; }
        /// <summary>
        /// Gets or sets the texture coordinates of this visual
        /// </summary>
        /// <remarks>
        /// use relative instead of absolute positioning!
        /// </remarks>
        List<Point[]> TextureCoordinates { get; set; }
        /// <summary>
        /// Gets or sets the width of this visual
        /// </summary>
        Double Width { get; set; }
        /// <summary>
        /// Gets or sets the height of this visual
        /// </summary>
        Double Height { get; set; }
        /// <summary>
        /// Gets or sets the length of this visual
        /// </summary>
        /// /// <summary>
        /// Gets or sets the Guid for this visual
        /// </summary>
        Guid Guid { get; set; }
        Double Length { get; set; }
        Double Opacity { get; set; }
        Double RotationX { get; set; }
        Double RotationY { get; set; }
        Double RotationZ { get; set; }
        Boolean IsMirrored { get; set; }
        /// <summary>
        /// Visual is rotatable but opacity stays the same
        /// </summary>
        Boolean IsDecorative { get; set; }
        /// <summary>
        /// slightly opaque with fixed texture and fixed position
        /// </summary>
        Boolean IsFixed { get; set; }
        Transform3D Transform { get; set; }
        Dictionary<string, MetadataBase> Metadata { get; set; }

        Vector3D AnimationAngles { get; set; }
        Vector3D AnimationDuration { get; set; }
        Vector3D AnimationType { get; set; }

        void UpdateModel();
    }
}

