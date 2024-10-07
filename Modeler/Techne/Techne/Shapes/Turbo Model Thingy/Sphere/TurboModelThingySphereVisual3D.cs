/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit;
using Techne.Plugins.Attributes;
using Techne.Plugins.BaseClasses;
using Techne.Plugins.ValueConverter;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    public class TurboModelThingySphereVisual3D : ShapeBase
    {
        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.Register("Segments", typeof(Int32), typeof(TurboModelThingySphereVisual3D));

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        public TurboModelThingySphereVisual3D()
            : this(1, 1, 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneCubeVisual3D"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="length">The length.</param>
        /// <param name="height">The height.</param>
        public TurboModelThingySphereVisual3D(double width, double length, double height)
        {
            Content = new GeometryModel3D();
            CompositionTarget.Rendering += CompositionTarget_Rendering;

            InvalidateModel();

            Width = width;
            Length = length;
            Height = height;

            Size = new Vector3D(width, length, height);

            Fill = new SolidColorBrush(Color.FromRgb(127, 127, 127));

            Transform = new RotateTransform3D();

            CalculateCenter();
        }

        [JavaConstructor("ModelRendererTurbo", 2)]
        public override Vector TextureSize
        {
            get { return base.TextureSize; }
        }

        [JavaMethod("setRotationPoint", 1, typeof(Vector3DToFloatVector3DConverter))]
        public override Vector3D Position
        {
            get { return base.Position; }
            set { base.Position = value; }
        }

        [JavaMethod("addSphere", 2)]
        public override double Height
        {
            get { return base.Height; }
            set
            {
                base.Width = value;
                base.Length = value;
                base.Height = value;
            }
        }

        [JavaMethod("addSphere", 3)]
        public int Segments
        {
            get { return 25; }
        }

        [JavaMethod("addSphere", 4)]
        public int Rings
        {
            get { return 25; }
        }

        [JavaMethod("addSphere", 1)]
        public override Vector TextureOffset
        {
            get { return base.TextureOffset; }
            set { base.TextureOffset = value; }
        }

        [JavaMethod("addSphere", 1, typeof(Vector3DToFloatVector3DConverter))]
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


        public override double Length
        {
            get { return base.Length; }
            set
            {
                base.Width = value;
                base.Length = value;
                base.Height = value;
            }
        }

        public override double Width
        {
            get { return base.Width; }
            set
            {
                base.Width = value;
                base.Length = value;
                base.Height = value;
            }
        }

        public override Guid Guid
        {
            get { return new Guid("E1957603-6C07-4A1E-9047-BB1F45E57CEA"); }
            set { }
        }

        public override void ReapplyTexture()
        {
        }

        protected override MeshGeometry3D Tessellate()
        {
            MeshBuilder meshBuilder = new MeshBuilder();

            meshBuilder.AddSphere(Center, Width, Segments);

            return meshBuilder.ToMesh();
        }
    }
}

