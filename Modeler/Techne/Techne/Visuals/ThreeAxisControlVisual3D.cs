/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit;
using Techne.Model;
using Techne.Models.Enums;

namespace Techne.Visuals
{
    internal class ThreeAxisControlVisual3D : ModelVisual3D
    {
        private readonly CubeVisual3D centerCube;
        private AxisControlVisual3D[] controls = new AxisControlVisual3D[6];

        #region Dependency Properties
        public static readonly DependencyProperty SelectedAxisDirectionProperty =
            DependencyProperty.Register("SelectedAxisDirection",
                                        typeof(byte),
                                        typeof(ThreeAxisControlVisual3D),
                                        new UIPropertyMetadata((byte)1));

        public static readonly DependencyProperty SelectedAxisProperty =
            DependencyProperty.Register("SelectedAxis",
                                        typeof(EnumAxis),
                                        typeof(ThreeAxisControlVisual3D),
                                        new UIPropertyMetadata(EnumAxis.X));

        public static readonly DependencyProperty AcitvatedProperty =
            DependencyProperty.Register("Activated",
                                        typeof(bool),
                                        typeof(ThreeAxisControlVisual3D),
                                        new UIPropertyMetadata(false));

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type",
                                        typeof(EnumSelectionType),
                                        typeof(ThreeAxisControlVisual3D),
                                        new UIPropertyMetadata(EnumSelectionType.Position, TypePropertyChanged));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position",
                                        typeof(Vector3D),
                                        typeof(ThreeAxisControlVisual3D),
                                        new UIPropertyMetadata(PositionChanged));

        public static readonly DependencyProperty ArrowLengthsProperty =
            DependencyProperty.Register("ArrowLengths",
                                        typeof(double),
                                        typeof(ThreeAxisControlVisual3D),
                                        new UIPropertyMetadata(5.0, GeometryChanged));
        #endregion

        #region Properties
        public byte SelectedAxisDirection
        {
            get { return (byte)GetValue(SelectedAxisDirectionProperty); }
            set { SetValue(SelectedAxisDirectionProperty, value); }
        }

        public double ArrowLengths
        {
            get { return (double)GetValue(ArrowLengthsProperty); }
            set { SetValue(ArrowLengthsProperty, value); }
        }

        public EnumSelectionType Type
        {
            get { return (EnumSelectionType)GetValue(TypeProperty); }
            set
            {
                SetValue(TypeProperty, value);
                TypePropertyChanged(this, new DependencyPropertyChangedEventArgs());
            }
        }

        public bool Activated
        {
            get { return (bool)GetValue(AcitvatedProperty); }
            set { SetValue(AcitvatedProperty, value); }
        }

        public EnumAxis SelectedAxis
        {
            get { return (EnumAxis)GetValue(SelectedAxisProperty); }
            set { SetValue(SelectedAxisProperty, value); }
        }

        public Vector3D Position
        {
            get { return (Vector3D)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        #endregion

        #region Constructor
        public ThreeAxisControlVisual3D()
            : this(1, 1, 1)
        {
        }

        public ThreeAxisControlVisual3D(int x, int y, int z) : this(new Vector3D(x, y, z))
        {
        }

        public ThreeAxisControlVisual3D(Vector3D position)
        {
            Position = position;

            double l = ArrowLengths;
            double d = l * 0.05;

            centerCube = new CubeVisual3D
                         {
                             Center = Position.ToPoint3D(),
                             SideLength = d,
                             Fill = Brushes.Black
                         };
            Children.Add(centerCube);

            GeometryChanged(this, new DependencyPropertyChangedEventArgs());
        }
        #endregion

        protected static void TypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (ThreeAxisControlVisual3D)d;

            control.controls = new AxisControlVisual3D[6];

            switch (control.Type)
            {
                case EnumSelectionType.Nothing:
                    break;
                case EnumSelectionType.Position:
                    CreatePositionControls(control);
                    break;
                case EnumSelectionType.Offset:
                    CreatePositionControls(control);
                    break;
                case EnumSelectionType.Size:
                    CreateSizeControls(control);
                    break;
                case EnumSelectionType.TextureOffset:
                    break;
                case EnumSelectionType.Rotation:
                    break;
                default:
                    break;
            }

            GeometryChanged(d, e);
        }

        private static void CreatePositionControls(ThreeAxisControlVisual3D control)
        {
            double l = control.ArrowLengths;
            double d = l * 0.05;

            control.controls[0] = new AxisControlVisual3D(control)
                                  {
                                      Point1 = control.Position.ToPoint3D(),
                                      Axis = EnumAxis.X,
                                      Diameter = d,
                                      Fill = Brushes.Red,
                                      Type = control.Type
                                  };

            control.Children.Add(control.controls[0]);

            control.controls[1] = new AxisControlVisual3D(control)
                                  {
                                      AxisDirection = 1,
                                      Point1 = control.Position.ToPoint3D(),
                                      Axis = EnumAxis.Y,
                                      Diameter = d,
                                      Fill = Brushes.Green,
                                      Type = control.Type
                                  };

            control.Children.Add(control.controls[1]);

            control.controls[2] = new AxisControlVisual3D(control)
                                  {
                                      Point1 = control.Position.ToPoint3D(),
                                      Diameter = d,
                                      Axis = EnumAxis.Z,
                                      Type = control.Type
                                  };

            control.Children.Add(control.controls[2]);
        }

        private static void CreateSizeControls(ThreeAxisControlVisual3D control)
        {
            double l = control.ArrowLengths;
            double d = l * 0.05;

            control.controls[0] = new AxisControlVisual3D(control)
                                  {
                                      Point1 = control.Position.ToPoint3D(),
                                      Axis = EnumAxis.X,
                                      Diameter = d,
                                      Fill = Brushes.Red,
                                      Type = control.Type
                                  };

            control.controls[1] = new AxisControlVisual3D(control)
                                  {
                                      Point1 = control.Position.ToPoint3D(),
                                      Axis = EnumAxis.Y,
                                      Diameter = d,
                                      Fill = Brushes.Green,
                                      Type = control.Type
                                  };

            control.controls[2] = new AxisControlVisual3D(control)
                                  {
                                      Point1 = control.Position.ToPoint3D(),
                                      Diameter = d,
                                      Axis = EnumAxis.Z,
                                      Type = control.Type
                                  };

            control.controls[3] = new AxisControlVisual3D(control)
                                  {
                                      AxisDirection = 1,
                                      Point1 = control.Position.ToPoint3D(),
                                      Axis = EnumAxis.X,
                                      Diameter = d,
                                      Fill = Brushes.Red,
                                      Type = control.Type
                                  };

            control.controls[4] = new AxisControlVisual3D(control)
                                  {
                                      AxisDirection = 1,
                                      Point1 = control.Position.ToPoint3D(),
                                      Axis = EnumAxis.Y,
                                      Diameter = d,
                                      Fill = Brushes.Green,
                                      Type = control.Type
                                  };

            control.controls[5] = new AxisControlVisual3D(control)
                                  {
                                      AxisDirection = 1,
                                      Point1 = control.Position.ToPoint3D(),
                                      Diameter = d,
                                      Axis = EnumAxis.Z,
                                      Type = control.Type
                                  };

            control.Children.Add(control.controls[0]);
            control.Children.Add(control.controls[1]);
            control.Children.Add(control.controls[2]);
            control.Children.Add(control.controls[3]);
            control.Children.Add(control.controls[4]);
            control.Children.Add(control.controls[5]);
        }

        protected static void PositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ThreeAxisControlVisual3D)d).UpdateVisuals();
        }

        protected static void GeometryChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            ((ThreeAxisControlVisual3D)obj).UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            double l = ArrowLengths;
            double d = l * 0.05;

            foreach (var item in controls)
            {
                if (item != null)
                {
                    item.Point1 = Position.ToPoint3D();
                    item.Diameter = d;
                    item.Type = Type;
                }
            }

            if (centerCube != null)
            {
                centerCube.Center = Position.ToPoint3D();
                centerCube.SideLength = d;
            }
        }

        internal void Select(AxisControlVisual3D axisControlVisual3D)
        {
            SelectedAxis = axisControlVisual3D.Axis;
            Activated = true;
            SelectedAxisDirection = axisControlVisual3D.AxisDirection;
        }
    }
}

