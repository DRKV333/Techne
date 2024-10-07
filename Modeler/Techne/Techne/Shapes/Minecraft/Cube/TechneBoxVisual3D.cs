/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit;
using Techne.Plugins.Attributes;
using Techne.Plugins.BaseClasses;
using Techne.Plugins.ValueConverter;

namespace Techne.Plugins.Shapes
{
    public class TechneBoxVisual3D : ShapeBase
    {
        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register("Segments", typeof(Int32), typeof(TechneBoxVisual3D));

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        public TechneBoxVisual3D()
            : this(1, 1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="length">The length.</param>
        /// <param name="height">The height.</param>
        public TechneBoxVisual3D(double width, double length, double height)
        {
            Content = new GeometryModel3D();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            InvalidateModel();

            Width = width;
            Length = length;
            Height = height;

            Size = new Vector3D(width, length, height);

            //Fill = new SolidColorBrush(Color.FromRgb(127, 127, 127));

            Transform = new RotateTransform3D();

            CalculateCenter();
        }

        [JavaMethod("addBox", 3)]
        public new float Scale
        {
            get { return 0.0F; }
            set { }
        }

        [JavaMethod("addBox", 2)]
        public override Vector3D Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        [JavaConstructor("ModelRenderer", 1)]
        public override Vector TextureOffset
        {
            get { return base.TextureOffset; }
            set { base.TextureOffset = value; }
        }

        [JavaMethod("setRotationPoint", 1, typeof(Vector3DToFloatVector3DConverter))]
        public override Vector3D Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        [JavaMethod("addBox", 1, typeof(Vector3DToFloatVector3DConverter))]
        public override Vector3D Offset
        {
            get { return base.Offset; }
            set { base.Offset = value; }
        }

        [JavaField("rotateAngleX", typeof(DegreeToRadiansConverter))]
        public float MinecraftRotationX
        {
            get { return (float)base.RotationX; }
        }

        [JavaField("rotateAngleY", typeof(DegreeToRadiansConverter))]
        public float MinecraftRotationY
        {
            get { return (float)base.RotationY; }
        }

        [JavaField("rotateAngleZ", typeof(DegreeToRadiansConverter))]
        public float MinecraftRotationZ
        {
            get { return (float)base.RotationZ; }
        }

        [JavaField("mirror")]
        public override bool IsMirrored
        {
            get { return base.IsMirrored; }
            set { base.IsMirrored = value; }
        }

        public override Guid Guid
        {
            get { return new Guid("D9E621F7-957F-4B77-B1AE-20DCD0DA7751"); }
            set { }
        }

        public override void ReapplyTexture()
        {
            if (Texture == null)
            {
                var brush = new SolidColorBrush(Color.FromRgb(127, 127, 127));
                brush.Opacity = Opacity;

                Material = new DiffuseMaterial(brush);
                //this.Fill = brush;
            }
            else if (Texture != null)
            {
                Fill = null;

                Image image = new Image
                              {
                                  Source = Texture
                              };
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);
                var brush = new VisualBrush();

                brush.Visual = image;
                brush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
                brush.Viewbox = TextureHelper.GetViewboxRect(this, false).ToRelative(TextureSize);
                brush.Stretch = Stretch.Fill;

                brush.Opacity = Opacity;

                Material = new DiffuseMaterial(brush);
                BackMaterial = new DiffuseMaterial(brush);
            }

            UpdateModel();
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == BoxVisual3D.WidthProperty || e.Property == BoxVisual3D.HeightProperty || e.Property == BoxVisual3D.LengthProperty)
            {
                Size = new Vector3D(Width, Length, Height);
                //CalculateCenter();
                //ReapplyTexture();
                //SetValue(sizeProperty, new Vector3D(Width, Length, Height));
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Tessellates this visual.
        /// </summary>
        /// <returns></returns>
        protected override MeshGeometry3D Tessellate()
        {
            CalculateCoordinates();

            var b = new MeshBuilder();
            //left
            b.AddCubeFace(Center, new Vector3D(-1, 0, 0), new Vector3D(0, 0, 1), Width, Length, Height, TextureCoordinates[IsMirrored ? 1 : 0]);
            //right
            b.AddCubeFace(Center, new Vector3D(1, 0, 0), new Vector3D(0, 0, -1), Width, Length, Height, TextureCoordinates[IsMirrored ? 0 : 1]);
            //front (real front)
            b.AddCubeFace(Center, new Vector3D(0, -1, 0), new Vector3D(0, 0, 1), Length, Width, Height, TextureCoordinates[2]);
            //back (real back)
            b.AddCubeFace(Center, new Vector3D(0, 1, 0), new Vector3D(0, 0, -1), Length, Width, Height, TextureCoordinates[3]);
            //up - my front
            b.AddCubeFace(Center, new Vector3D(0, 0, 1), new Vector3D(0, -1, 0), Height, Width, Length, TextureCoordinates[4]);
            b.AddCubeFace(Center, new Vector3D(0, 0, -1), new Vector3D(0, 1, 0), Height, Width, Length, TextureCoordinates[5]);

            return b.ToMesh();
        }

        /// <summary>
        /// Calculates the texture coordinates.
        /// </summary>
        private void CalculateCoordinates()
        {
            var left = TextureHelper.GetViewboxRect(this, CubeSide.Left).ToRelative(TextureSize).ToPoints();
            var right = TextureHelper.GetViewboxRect(this, CubeSide.Right).ToRelative(TextureSize).ToPoints();
            var top = TextureHelper.GetViewboxRect(this, CubeSide.Top).ToRelative(TextureSize).ToPoints();
            var bottom = TextureHelper.GetViewboxRect(this, CubeSide.Bottom).ToRelative(TextureSize).ToPoints();
            var front = TextureHelper.GetViewboxRect(this, CubeSide.Front).ToRelative(TextureSize).ToPoints();
            var back = TextureHelper.GetViewboxRect(this, CubeSide.Back).ToRelative(TextureSize).ToPoints();

            if (IsMirrored)
            {
                left = left.Mirror();
                right = right.Mirror();
                top = top.Mirror();
                bottom = bottom.Mirror();
                front = front.Mirror();
                back = back.Mirror();
            }

            TextureCoordinates = new List<Point[]>
                                 {
                                     left.Rotate(1),
                                     right.Rotate(1),
                                     top.Rotate(2),
                                     bottom.Rotate(2),
                                     back.Rotate(2),
                                     front,
                                 };
        }
    }
}

