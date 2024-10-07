/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit;
using Techne.Model;
using Techne.Models.Enums;

namespace Techne.Visuals
{
    internal class AxisControlVisual3D : MeshElement3D
    {
        //private Vector3D direction;
        private readonly ThreeAxisControlVisual3D threeAxisControlVisual3D;

        #region DependencyProperties
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register("Diameter",
                                        typeof(double),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(1.0, GeometryChanged));

        //public static readonly DependencyProperty HeadLengthProperty =
        //    DependencyProperty.Register("HeadLength", typeof (double), typeof (ArrowVisual3D),
        //                                new UIPropertyMetadata(3.0, GeometryChanged));

        public static readonly DependencyProperty Point1Property =
            DependencyProperty.Register("Point1",
                                        typeof(Point3D),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(new Point3D(0, 0, 0), GeometryChanged));

        public static readonly DependencyProperty Point2Property =
            DependencyProperty.Register("Point2",
                                        typeof(Point3D),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(new Point3D(0, 0, 10), GeometryChanged));

        public static readonly DependencyProperty ThetaDivProperty =
            DependencyProperty.Register("ThetaDiv",
                                        typeof(int),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(36, GeometryChanged));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type",
                                        typeof(EnumSelectionType),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(EnumSelectionType.Nothing, GeometryChanged));

        public static readonly DependencyProperty AxisProperty =
            DependencyProperty.Register("Axis",
                                        typeof(EnumAxis),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(EnumAxis.X, AxisPropertyChanged));

        public static readonly DependencyProperty AxisDirectionProperty =
            DependencyProperty.Register("AxisDirection",
                                        typeof(byte),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata((byte)0, AxisPropertyChanged));

        public static readonly DependencyProperty ArrowLengthProperty =
            DependencyProperty.Register("ArrowLength",
                                        typeof(double),
                                        typeof(AxisControlVisual3D),
                                        new UIPropertyMetadata(8.0, GeometryChanged));
        #endregion

        #region public Properties
        public byte AxisDirection
        {
            get { return (byte)GetValue(AxisDirectionProperty); }
            set { SetValue(AxisDirectionProperty, value); }
        }

        public EnumAxis Axis
        {
            get { return (EnumAxis)GetValue(AxisProperty); }
            set { SetValue(AxisProperty, value); }
        }

        public double ArrowLength
        {
            get { return (double)GetValue(ArrowLengthProperty); }
            set { SetValue(ArrowLengthProperty, value); }
        }

        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        //public double HeadLength
        //{
        //    get { return (double) GetValue(HeadLengthProperty); }
        //    set { SetValue(HeadLengthProperty, value); }
        //}

        public int ThetaDiv
        {
            get { return (int)GetValue(ThetaDivProperty); }
            set { SetValue(ThetaDivProperty, value); }
        }

        public Point3D Point1
        {
            get { return (Point3D)GetValue(Point1Property); }
            set
            {
                SetValue(Point1Property, value);
                Direction = Axis.GetAxisVector();
            }
        }

        public Point3D Point2
        {
            get { return (Point3D)GetValue(Point2Property); }
            set { SetValue(Point2Property, value); }
        }

        public Point3D Origin
        {
            get { return Point1; }
            set { Point1 = value; }
        }

        public Vector3D Direction
        {
            get { return Point2 - Point1; }
            set { Point2 = Point1 + value * ArrowLength; }
        }

        public EnumSelectionType Type
        {
            get { return (EnumSelectionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }
        #endregion

        public AxisControlVisual3D(ThreeAxisControlVisual3D threeAxisControlVisual3D)
        {
            // TODO: Complete member initialization
            this.threeAxisControlVisual3D = threeAxisControlVisual3D;
            //direction = new Vector3D(0, 0, 0);
        }

        protected static void AxisPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = ((AxisControlVisual3D)d);
            control.Direction = GetAxisVector(control.Axis, control.AxisDirection);
            GeometryChanged(d, e);
        }

        private static Vector3D GetAxisVector(EnumAxis enumAxis, byte direction)
        {
            var vector = enumAxis.GetAxisVector();

            if (direction == 1)
                vector.Negate();

            return vector;
        }

        public void Clicked()
        {
            threeAxisControlVisual3D.Select(this);
        }


        protected override MeshGeometry3D Tessellate()
        {
            var builder = new MeshBuilder(true, true);
            //builder.AddArrow(Point1, Point2, Diameter, 3, ThetaDiv);

            switch (Type)
            {
                case EnumSelectionType.Nothing:
                    break;
                case EnumSelectionType.Position:
                    builder.AddArrow(Point1, Point2, Diameter, 3, ThetaDiv);
                    break;
                case EnumSelectionType.Offset:
                    break;
                case EnumSelectionType.Size:
                    double length = Direction.Length;
                    double r = Diameter / 2;
                    double headLength = 3;

                    //builder.AddRevolvedGeometry(
                    //    new System.Windows.Media.PointCollection()
                    //    {
                    //        new Point(0, 0),
                    //         new Point(0, r),
                    //         new Point(length - Diameter * headLength, r),
                    //         new Point(length - Diameter * headLength, Diameter),
                    //         new Point(length, Diameter),
                    //         new Point(length, 0)
                    //    },
                    //    Point1,
                    //    Direction,
                    //    ThetaDiv);
                    var point = Point1 + GetAxisVector(Axis, AxisDirection) * ArrowLength;
                    builder.AddBox(point, ArrowLength / 20, ArrowLength / 20, ArrowLength / 20);
                    break;
                case EnumSelectionType.TextureOffset:
                    break;
                case EnumSelectionType.Rotation:
                    break;
                default:
                    break;
            }

            return builder.ToMesh();
        }
    }
}

