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
using Techne.Plugins.BaseClasses;
using System.Windows;
using Techne.Plugins.Attributes;
using System.Windows.Media.Media3D;
using Techne.Plugins.ValueConverter;
using System.Windows.Media;
using HelixToolkit;
using System.Windows.Controls;

namespace Techne.Plugins.Shapes
{
    public class TurboModelThingyCubeVisual3D : ShapeBase
    {
        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register("Segments", typeof(Int32), typeof(TurboModelThingyCubeVisual3D));

        [JavaMethod("addBox", 3)]
        public Double Scale
        {
            get
            {
                return 0.0F;
            }
            set
            {
            }
        }
        [JavaConstructor("ModelRendererTurbo", 2)]
        public override Vector TextureSize
        {
            get
            {
                return base.TextureSize;
            }
        }
        [JavaMethod("addBox", 2)]
        public override Vector3D Size
        {
            get
            {
                return base.Size;
            }
            set
            {
                base.Size = value;
            }
        }
        [JavaConstructor("ModelRendererTurbo", 1)]
        public override Vector TextureOffset
        {
            get
            {
                return base.TextureOffset;
            }
            set
            {
                base.TextureOffset = value;
            }
        }
        [JavaMethod("setRotationPoint", 1, typeof(Vector3DToFloatVector3DConverter))]
        public override Vector3D Position
        {
            get
            {
                return base.Position;
            }
            set
            {
                base.Position = value;
            }
        }

        [JavaMethod("addBox", 1, typeof(Vector3DToFloatVector3DConverter))]
        public override Vector3D Offset
        {
            get
            {
                return base.Offset;
            }
            set
            {
                base.Offset = value;
            }
        }
        [JavaField("rotateAngleX", typeof(DegreeToRadiansConverter))]
        public float MinecraftRotationX
        {
            get
            {
                return (float)base.RotationX;
            }
        }

        [JavaField("rotateAngleY", typeof(DegreeToRadiansConverter))]
        public float MinecraftRotationY
        {
            get
            {
                return (float)base.RotationY;
            }
        }

        [JavaField("rotateAngleZ", typeof(DegreeToRadiansConverter))]
        public float MinecraftRotationZ
        {
            get
            {
                return (float)base.RotationZ;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        public TurboModelThingyCubeVisual3D()
            : this(1, 1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="length">The length.</param>
        /// <param name="height">The height.</param>
        public TurboModelThingyCubeVisual3D(double width, double length, double height)
        {
            Content = new GeometryModel3D();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            InvalidateModel();

            this.Width = width;
            this.Length = length;
            this.Height = height;

            Size = new Vector3D(width, length, height);

            //Fill = new SolidColorBrush(Color.FromRgb(127, 127, 127));

            this.Transform = new RotateTransform3D();

            CalculateCenter();
        }

        public override void ReapplyTexture()
        {
            if (Texture == null)
            {
                var brush = new SolidColorBrush(Color.FromRgb(127, 127, 127));
                brush.Opacity = Opacity;

                this.Material = new DiffuseMaterial(brush);
                //this.Fill = brush;
            }
            else if (Texture != null)
            {
                this.Fill = null;

                Image image = new Image()
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

                UpdateModel();
            }
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
            var left = TextureHelper.GetViewboxRect(this, CubeSide.Left).ToRelative(this.TextureSize).ToPoints();
            var right = TextureHelper.GetViewboxRect(this, CubeSide.Right).ToRelative(this.TextureSize).ToPoints();
            var top = TextureHelper.GetViewboxRect(this, CubeSide.Top).ToRelative(this.TextureSize).ToPoints();
            var bottom = TextureHelper.GetViewboxRect(this, CubeSide.Bottom).ToRelative(this.TextureSize).ToPoints();
            var front = TextureHelper.GetViewboxRect(this, CubeSide.Front).ToRelative(this.TextureSize).ToPoints();
            var back = TextureHelper.GetViewboxRect(this, CubeSide.Back).ToRelative(this.TextureSize).ToPoints();

            if (IsMirrored)
            {
                left = left.Mirror();
                right = right.Mirror();
                top = top.Mirror();
                bottom = bottom.Mirror();
                front = front.Mirror();
                back = back.Mirror();
            }

            TextureCoordinates = new List<Point[]>()
            {
                left.Rotate(1),
                right.Rotate(1),
                top.Rotate(2),
                bottom,
                back.Rotate(2),
                front,
            };
        }

        public override Guid Guid
        {
            get
            {
                return new Guid("de81aa14-bd60-4228-8d8d-5238bcd3caaa");
            }
            set
            {

            }
        }
    }
}

