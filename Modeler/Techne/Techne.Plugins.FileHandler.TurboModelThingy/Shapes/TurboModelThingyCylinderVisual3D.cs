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
using Techne.Plugins.Interfaces;
using System.Windows.Media.Media3D;
using Techne.Plugins.Extensions;
using System.Windows.Media;
using Techne.Plugins.Attributes;
using System.Linq.Expressions;
using System.Windows;
using Techne.Plugins.ValueConverter;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    public class TurboModelThingyCylinderVisual3D : ShapeBase
    {
        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register("Segments", typeof(Int32), typeof(TurboModelThingyCylinderVisual3D));

        [JavaConstructor("ModelRendererTurbo", 2)]
        public override Vector TextureSize
        {
            get
            {
                return base.TextureSize;
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

        [JavaMethod("addCylinder", 3)]
        public override double Length
        {
            get
            {
                return base.Length;
            }
            set
            {
                base.Length = value;
            }
        }

        [JavaMethod("addCylinder", 2)]
        public override double Height
        {
            get
            {
                return base.Height;
            }
            set
            {
                base.Height = value;
                base.Width = value;
            }
        }

        [JavaMethod("addCylinder", 4)]
        public int Segments
        {
            get
            {
                return 25;
            }
        }

        [JavaMethod("addCylinder", 1, typeof(Vector3DToFloatVector3DConverter))]
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
        
        public override double Width
        {
            get
            {
                return base.Width;
            }
            set
            {
                base.Width = value;
                base.Height = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        public TurboModelThingyCylinderVisual3D()
            : this(1, 1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="length">The length.</param>
        /// <param name="height">The height.</param>
        public TurboModelThingyCylinderVisual3D(double width, double length, double height)
        {
            Content = new GeometryModel3D();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            InvalidateModel();

            this.Width = width;
            this.Length = length;
            this.Height = height;

            Size = new Vector3D(width, length, height);

            Fill = new SolidColorBrush(Color.FromRgb(127, 127, 127));

            this.Transform = new RotateTransform3D();

            CalculateCenter();
        }

        public override void ReapplyTexture()
        {

        }

        protected override MeshGeometry3D Tessellate()
        {
            HelixToolkit.MeshBuilder meshBuilder = new HelixToolkit.MeshBuilder();

            var center = Center;
            center.Y -= Length / 2;
            meshBuilder.AddCone(center, new Vector3D(0, 1, 0), Width, Width, Length, true, true, 25);

            return meshBuilder.ToMesh();
        }

        public override Guid Guid
        {
            get
            {
                return new Guid("B94B0064-E61C-4517-8F99-ADB273F1B33E");
            }
            set
            {

            }
        }
    }
}

