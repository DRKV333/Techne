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
using System.Windows;
using System.Windows.Media.Media3D;
using System.Windows.Media.Imaging;
using Techne.Plugins.Interfaces;
using System.Windows.Media;
using System.Xml.Serialization;
using System.Globalization;
using System.Xml;
using Techne.Plugins.Attributes.XML;
using System.Collections.ObjectModel;

namespace Techne.Plugins.BaseClasses
{
    [TechneXmlRoot("Shape")]
    public abstract class ShapeBase : ModelVisual3D, ITechneVisual
    {
        #region Fields
        private readonly object invalidateLock = "";
        private bool update = true;
        protected Matrix3D rotationMatrix;
        private bool isInvalidated = true;
        /// <summary>
        /// 
        /// </summary>
        private Vector textureOffset;
        /// <summary>
        /// backing field for Texture
        /// </summary>
        private BitmapSource texture;
        private Double opacity = 1;
        #endregion

        #region Properties
        public static DependencyProperty textureOffsetProperty = DependencyProperty.Register("TextureOffset", typeof(Vector), typeof(ShapeBase));
        public static DependencyProperty positionProperty = DependencyProperty.Register("Position", typeof(Vector3D), typeof(ShapeBase));
        public static DependencyProperty combinedPositionProperty = DependencyProperty.Register("CombinedPosition", typeof(Vector3D), typeof(ShapeBase));
        public static DependencyProperty offsetProperty = DependencyProperty.Register("Offset", typeof(Vector3D), typeof(ShapeBase));
        public static DependencyProperty sizeProperty = DependencyProperty.Register("Size", typeof(Vector3D), typeof(ShapeBase));

        public static DependencyProperty rotationXProperty = DependencyProperty.Register("RotationX", typeof(Double), typeof(ShapeBase));
        public static DependencyProperty rotationYProperty = DependencyProperty.Register("RotationY", typeof(Double), typeof(ShapeBase));
        public static DependencyProperty rotationZProperty = DependencyProperty.Register("RotationZ", typeof(Double), typeof(ShapeBase));

        public static DependencyProperty isFixedProperty = DependencyProperty.Register("IsFixed", typeof(Boolean), typeof(ShapeBase));
        public static DependencyProperty isDecorativeProperty = DependencyProperty.Register("IsDecorative", typeof(Boolean), typeof(ShapeBase));
        public static DependencyProperty isMirroredProperty = DependencyProperty.Register("IsMirrored", typeof(Boolean), typeof(ShapeBase));

        public static DependencyProperty nameProperty = DependencyProperty.Register("Name", typeof(String), typeof(ShapeBase));

        //public static DependencyProperty textureSizeProperty = DependencyProperty.Register("AnimationTransform", typeof(), typeof(TechneCubeVisual3D));

        public static DependencyProperty textureSizeProperty = DependencyProperty.Register("TextureSize", typeof(Vector), typeof(ShapeBase));
        public static DependencyProperty textureCoordinatesProperty = DependencyProperty.Register("TextureCoordinates", typeof(List<Point[]>), typeof(ShapeBase));
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty FillProperty =
            DependencyProperty.Register("Fill", typeof(Brush), typeof(ShapeBase),
                                        new UIPropertyMetadata(null, FillChanged));

        public static readonly DependencyProperty MaterialProperty =
            DependencyProperty.Register("Material", typeof(Material), typeof(ShapeBase),
                                        new UIPropertyMetadata(null, MaterialChanged));

        public static readonly DependencyProperty BackMaterialProperty =
            DependencyProperty.Register("BackMaterial", typeof(Material), typeof(ShapeBase),
                                        new UIPropertyMetadata(null, MaterialChanged));

        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point3D), typeof(ShapeBase),
                                        new UIPropertyMetadata(new Point3D(), GeometryChanged));

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double), typeof(ShapeBase),
                                        new UIPropertyMetadata(1.0, GeometryChanged));

        public static readonly DependencyProperty LengthProperty =
            DependencyProperty.Register("Length", typeof(double), typeof(ShapeBase),
                                        new UIPropertyMetadata(1.0, GeometryChanged));

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double), typeof(ShapeBase),
                                        new UIPropertyMetadata(1.0, GeometryChanged));
        #endregion

        #region Public Properties
        public ITechneVisualCollection Parent
        {
            get;
            set; 
        }
        public ObservableCollection<ITechneVisual> Children
        {
            get;
            private set;
        }

        public virtual GeometryModel3D Model
        {
            get { return Content as GeometryModel3D; }
        }

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public Material Material
        {
            get { return (Material)GetValue(MaterialProperty); }
            set { SetValue(MaterialProperty, value); }
        }

        public Material BackMaterial
        {
            get { return (Material)GetValue(BackMaterialProperty); }
            set { SetValue(BackMaterialProperty, value); }
        }

        /// <summary>
        /// Visual is rotatable but opacity stays the same
        /// </summary>
        [TechneXmlElement("2.0")]
        public virtual Boolean IsDecorative
        {
            get
            {
                return (bool)GetValue(isDecorativeProperty);
            }
            set
            {
                if (value)
                {
                    Opacity = .5;
                }
                else
                {
                    Opacity = 1;
                }

                SetValue(isDecorativeProperty, value);
            }
        }

        /// <summary>
        /// slightly opaque with fixed texture and fixed position
        /// </summary>
        [TechneXmlElement("2.0")]
        public virtual Boolean IsFixed
        {
            get
            {
                return (bool)GetValue(isFixedProperty);
            }
            set
            {
                if (value)
                {
                    Opacity = .5;
                }
                else
                {
                    Opacity = 1;
                }

                SetValue(isFixedProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the texture coordinates of this visual
        /// </summary>
        public virtual List<Point[]> TextureCoordinates
        {
            get
            {
                return (List<Point[]>)GetValue(textureCoordinatesProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(textureCoordinatesProperty, value);
            }
        }
        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        [TechneXmlElement("2.0")]
        public virtual Vector3D Offset
        {
            get
            {
                return (Vector3D)GetValue(offsetProperty);
                //return offset;
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(offsetProperty, value);
                CalculateCenter();
                RotateModel();
            }
        }

        public virtual Vector3D CombinedPosition 
        {
            set
            {
                SetValue(combinedPositionProperty, value);
            }
            get
            {
                return (Vector3D)GetValue(combinedPositionProperty);
            }
        }

        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        [TechneXmlElement("2.0")]
        public virtual Vector3D Position
        {
            get
            {
                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    return Parent.Position;
                }
                return (Vector3D)GetValue(positionProperty);
                //return position;
            }
            set
            {
                if (IsFixed)
                    return;

                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    Parent.Position = value;

                    //I think we don't need this here since the parent calls it anyway
                    //CalculateCombinedPosition();
                }
                else
                {
                    SetValue(positionProperty, value);

                    CalculateCombinedPosition();
                    RotateModel();
                }

                //dummy
            }
        }

        public void CalculateCombinedPosition()
        {
            Vector3D position;
            if (Parent == null)
            {
                position = (Vector3D)GetValue(positionProperty);
            }
            else if (!Parent.ChildrenInheritProperties)
            {
                position = (Vector3D)GetValue(positionProperty);
                position += Parent.CombinedPosition;
            }
            else
            {
                position = Parent.CombinedPosition;
            }
            CombinedPosition = position;

            CalculateCenter();
        }
        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        [TechneXmlElement("2.0")]
        public virtual Vector TextureOffset
        {
            get
            {
                return (Vector)GetValue(textureOffsetProperty);
                //return textureOffset;
            }
            set
            {
                if (IsFixed)
                    return;

                this.textureOffset = value;
                SetValue(textureOffsetProperty, value);
                //todo: commented to remove texture mapping

                if (Texture == null)
                    FillShape();
                else
                    ReapplyTexture();
            }
        }
        /// <summary>
        /// Gets or sets the size of this visual
        /// </summary>
        [TechneXmlElement("2.0")]
        public virtual Vector3D Size
        {
            get
            {
                return (Vector3D)GetValue(sizeProperty);
                //return new Vector3D(Width, Length, Height);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(sizeProperty, value);

                if (Width != value.X)
                    Width = value.X;
                if (Length != value.Y)
                    Length = value.Y;
                if (Height != value.Z)
                    Height = value.Z;

                CalculateCenter();

                if (Texture == null)
                    FillShape();
                else
                    ReapplyTexture();
            }
        }

        [TechneXmlElement("2.0")]
        public Vector3D Rotation
        {
            get
            {
                return new Vector3D(RotationX, RotationY, RotationZ);
            }
            set
            {
                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    Parent.RotationX = value.X;
                    Parent.RotationY = value.Y;
                    Parent.RotationZ = value.Z;
                }
                else
                {
                    RotationX = value.X;
                    RotationY = value.Y;
                    RotationZ = value.Z;
                }
            }
        }
        
        public virtual Double RotationX
        {
            get
            {
                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    return Parent.RotationX;
                }
                return (Double)GetValue(rotationXProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    Parent.RotationX = value;
                }
                else
                {
                    SetValue(rotationXProperty, value);
                    RotateModel();
                }
            }
        }
        public virtual Double RotationY
        {
            get
            {
                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    return Parent.RotationY;
                }
                return (Double)GetValue(rotationYProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    Parent.RotationY = value;
                }
                else
                {
                    SetValue(rotationYProperty, value);
                    RotateModel();
                }
            }
        }
        public virtual Double RotationZ
        {
            get
            {
                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    return Parent.RotationZ;
                }
                return (Double)GetValue(rotationZProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                if (Parent != null && Parent.ChildrenInheritProperties)
                {
                    Parent.RotationZ = value;
                }
                else
                {
                    SetValue(rotationZProperty, value);
                    RotateModel();
                }
            }
        }

        /// <summary>
        /// Gets or sets the texture of this visual
        /// </summary>
        public virtual BitmapSource Texture
        {
            get
            {
                return texture;
            }
            set
            {
                if (IsFixed)
                    return;

                texture = value;

                if (texture != null)
                {
                    //TextureSize = new System.Windows.Vector(texture.PixelWidth, texture.PixelHeight);
                    //TextureSize = new System.Windows.Vector(64, 32);
                    ReapplyTexture();
                    //CalculateCenter();
                }
                else
                {
                    TextureSize = new Vector(64, 32);
                }
            }
        }
        /// <summary>
        /// Gets or sets the name of this visual used for saving and especially exporting
        /// </summary>
        [TechneXmlAttribute("2.0", ElementName = "name")]
        public virtual String Name
        {
            get
            {
                return (String)GetValue(nameProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(nameProperty, value.Replace("_", ""));
            }
        }

        public Vector3D AnimationAngles
        {
            get
            {
                return Animation.AnimationAngles;
            }
            set
            {
                Animation.AnimationAngles = value;
            }
        }
        
        public Vector3D AnimationDuration
        {
            get
            {
                return Animation.AnimationDuration;
            }
            set
            {
                Animation.AnimationDuration = value;
            }
        }
        
        public Vector3D AnimationType
        {
            get
            {
                return Animation.AnimationType;
            }
            set
            {
                Animation.AnimationType = value;
            }
        }

        [TechneXmlElement("2.1", ElementName = "Animation")]
        public Techne.Plugins.Models.AnimationDataModel Animation
        {
            get;
            set;
        }

        public virtual Double Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                if (IsFixed || IsDecorative)
                    return;

                opacity = value;
                ReapplyTexture();
            }
        }

        /// <summary>
        /// Gets or sets the texture size of this visual
        /// </summary>
        public virtual Vector TextureSize
        {
            get
            {
                return (Vector)GetValue(textureSizeProperty);
            }
            set
            {
                SetValue(textureSizeProperty, value);
                ReapplyTexture();
            }
        }

        //[XmlElement("MetaData")]
        [XmlIgnore]
        public virtual Dictionary<string, MetadataBase> Metadata { get; set; }

        //[XmlAttribute("type", Type = typeof(Guid))]
        [TechneXmlAttribute("type", typeof(Guid), "2.0", "")]
        public abstract Guid Guid
        {
            get;
            set;
        }

        public virtual double Length
        {
            get { return (double)GetValue(LengthProperty); }
            set
            {
                if (value < 0)
                    value = 0;

                SetValue(LengthProperty, value);
                Size = new Vector3D(Width, Length, Height);
            }
        }

        public virtual double Width
        {
            get { return (double)GetValue(WidthProperty); }
            set
            {
                if (value < 0)
                    value = 0;

                SetValue(WidthProperty, value);
                Size = new Vector3D(Width, Length, Height);
            }
        }

        public virtual double Height
        {
            get { return (double)GetValue(HeightProperty); }
            set
            {
                if (value < 0)
                    value = 0;

                SetValue(HeightProperty, value);
                Size = new Vector3D(Width, Length, Height);
            }
        }

        public virtual Point3D Center
        {
            get { return (Point3D)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        [TechneXmlElement("2.0")]
        public virtual Boolean IsMirrored
        {
            get
            {
                return (Boolean)GetValue(isMirroredProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(isMirroredProperty, value);
                ReapplyTexture();
            }
        }

        public Double Scale
        {
            get
            {
                return 0;
            }
            set
            {
            }
        }

        public Boolean IsSelected { get; set; }
        #endregion

        public ShapeBase()
        {
            Animation = new Models.AnimationDataModel();
            Children = new ObservableCollection<ITechneVisual>();
        }

        #region Methods
        #region protected
        //protected Matrix3D CalculateRotationMatrix(double x, double y, double z)
        //{
        //    //x *= Math.PI / 180;
        //    //y *= Math.PI / 180;
        //    //z *= Math.PI / 180;

        //    rotationMatrix = new Matrix3D();

        //    rotationMatrix.Rotate(new Quaternion(new Vector3D(0, 0, 1), z));
        //    rotationMatrix.Rotate(new Quaternion(new Vector3D(0, 1, 0) * rotationMatrix, y));
        //    rotationMatrix.Rotate(new Quaternion(new Vector3D(1, 0, 0) * rotationMatrix, x));

        //    return rotationMatrix;

        //    //rotationMatrix.Translate(Position);
        //}
        protected void CalculateRotationMatrix(double x, double y, double z)
        {
//            void Main()
//{
//    var rotation = CalculateQuaternion(0, 0, 90);
	
//    var vector = new Vector3D(0, 1, 0);
	
//    var result = RotateVector(rotation, vector);
	
//    //var v3rot = q * v3;
	
//    var a1 = Math.Atan2(result.X, result.Y);
//    var a2 = Math.Asin(result.Z);
	
//    (a1  * 180 / Math.PI).Dump();
//    (a2  * 180 / Math.PI).Dump();
	
//    var q1 = new Quaternion(0, 0, -Math.Sin(a1 / 2), Math.Cos(a1 / 2));
//    var q2 = new Quaternion(Math.Sin(a2 / 2), 0, 0, Math.Cos(a2 / 2));
	
////	q1.Dump();
////	q2.Dump();
	
//    var v3 = new Vector3D(0, 0, 0);
	
//    var q12 = q1 * q2;
////	q12.Dump();
	
//    var v312 = RotateVector(q12, v3);
//    var v3g = RotateVector(rotation, v3);
	
//    (Math.Acos(Vector3D.DotProduct(v312,v3g)) * 180 / Math.PI).Dump();
	
//    var sign = Math.Sign(Vector3D.DotProduct(Vector3D.CrossProduct(v312, v3g), result));
//}

//Quaternion CalculateQuaternion(double heading, double attitude, double bank)
//        {
//            heading *= Math.PI / 181;
//            attitude *= Math.PI / 181;
//            bank *= Math.PI / 181;
//            // Assuming the angles are in radians.
//            double c1 = Math.Cos(heading);
//            double s1 = Math.Sin(heading);
//            double c2 = Math.Cos(attitude);
//            double s2 = Math.Sin(attitude);
//            double c3 = Math.Cos(bank);
//            double s3 = Math.Sin(bank);
//            var w = Math.Sqrt(1.0 + c1 * c2 + c1 * c3 - s1 * s2 * s3 + c2 * c3) / 2.0;
//            double w4 = (4.0 * w);
//            var x = (c2 * s3 + c1 * s3 + s1 * s2 * c3) / w4;
//            var y = (s1 * c2 + s1 * c3 + c1 * s2 * s3) / w4;
//            var z = (-s1 * s3 + c1 * s2 * c3 + s2) / w4;

//            return new Quaternion(x, y, z, w);
//        }

//Vector3D RotateVector(Quaternion rotation, Vector3D vector)
//{
//float x = (float)(rotation.X + rotation.X);
//    float y = (float)(rotation.Y + rotation.Y);
//    float z = (float)(rotation.Z + rotation.Z);
//    float wx = (float)(rotation.W * x);
//    float wy = (float)(rotation.W * y);
//    float wz = (float)(rotation.W * z);
//    float xx = (float)(rotation.X * x);
//    float xy = (float)(rotation.X * y);
//    float xz = (float)(rotation.X * z);
//    float yy = (float)(rotation.Y * y);
//    float yz = (float)(rotation.Y * z);
//    float zz = (float)(rotation.Z * z);
	
//    float num1 = ((1.0f - yy) - zz);
//    float num2 = (xy - wz);
//    float num3 = (xz + wy);
//    float num4 = (xy + wz);
//    float num5 = ((1.0f - xx) - zz);
//    float num6 = (yz - wx);
//    float num7 = (xz - wy);
//    float num8 = (yz + wx);
//    float num9 = ((1.0f - xx) - yy);
	
//    return new Vector3D(
//        ((vector.X * num1) + (vector.Y * num2)) + (vector.Z * num3),
//        ((vector.X * num4) + (vector.Y * num5)) + (vector.Z * num6),
//        ((vector.X * num7) + (vector.Y * num8)) + (vector.Z * num9));
//}
        }
        protected static void GeometryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShapeBase)d).GeometryChanged();
        }

        protected static void MaterialChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ShapeBase)d).MaterialChanged();
        }

        protected void MaterialChanged()
        {
            if (!update)
                return;

            if (Model == null)
                return;

            if (Material == null)
            {
                // use a default blue material
                Model.Material = CreateMaterial(Brushes.Blue);
            }
            else
                Model.Material = Material;

            //model.Material = Material;

            // the back material may be null (invisible)
            Model.BackMaterial = BackMaterial;
        }

        protected void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            lock (invalidateLock)
            {
                if (isInvalidated)
                {
                    isInvalidated = false;
                    GeometryChanged();
                    MaterialChanged();
                }
            }
        }

        protected void InvalidateModel()
        {
            lock (invalidateLock)
            {
                isInvalidated = true;
            }
        }
        #endregion
        #region private
        private static void FillChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var el = (ShapeBase)d;
            el.Material = CreateMaterial(el.Fill);
            el.BackMaterial = el.Material;
        }

        private static Material CreateMaterial(Brush brush)
        {
            return CreateMaterial(brush, 100);
        }

        private static Material CreateMaterial(Brush brush, double specularPower)
        {
            var materialGroup = new MaterialGroup();
            materialGroup.Children.Add(new DiffuseMaterial(brush));
            if (specularPower > 0)
                materialGroup.Children.Add(new SpecularMaterial(Brushes.White, specularPower));
            return materialGroup;
        }
        #endregion
        #region public
        public void UpdateModel()
        {
            CalculateCombinedPosition(); //contains call to CalculateCenter();
            RotateModel();
            GeometryChanged();
            MaterialChanged();
        }
        #endregion
        #endregion

        protected void GeometryChanged()
        {
            if (!update)
                return;

            Model.Geometry = Tessellate();
        }

        #region Virtual Methods
        /// <summary>
        /// Calculates the center.
        /// </summary>
        protected virtual void CalculateCenter()
        {
            var sum = Offset + CombinedPosition;
            var posX = sum.X;
            var posY = sum.Y;
            var posZ = sum.Z;

            this.Center = new Point3D(posX + this.Width / 2, posY + this.Length / 2, posZ + this.Height / 2);
            
        }

        protected virtual void RotateModel()
        {
            Transform3DGroup transform = new Transform3DGroup();
            var rotation = new RotateTransform3D
                               {
                                   CenterX = Position.X,
                                   CenterY = Position.Y,
                                   CenterZ = Position.Z,
                                   Rotation =
                                       new QuaternionRotation3D(CalculateQuaternion(RotationY, RotationZ, RotationX))
                               };
            
            if (this.Parent != null)
                ParentTransforms(this.Parent, transform);
            else
                transform.Children.Add(rotation);

            this.Transform = transform;
        }

        private void ParentTransforms(ITechneVisualCollection visual, Transform3DGroup transform)
        {
            if (visual.Parent != null)
            {
                ParentTransforms(visual.Parent, transform);
            }

            if (visual.ChildrenInheritProperties)
                transform.Children.Add(visual.Transform);
        }

        protected virtual Quaternion CalculateQuaternion(double heading, double attitude, double bank)
        {
            heading *= Math.PI / 181;
            attitude *= Math.PI / 181;
            bank *= Math.PI / 181;
            // Assuming the angles are in radians.
            double c1 = Math.Cos(heading);
            double s1 = Math.Sin(heading);
            double c2 = Math.Cos(attitude);
            double s2 = Math.Sin(attitude);
            double c3 = Math.Cos(bank);
            double s3 = Math.Sin(bank);
            var w = Math.Sqrt(1.0 + c1 * c2 + c1 * c3 - s1 * s2 * s3 + c2 * c3) / 2.0;
            double w4 = (4.0 * w);
            var x = (c2 * s3 + c1 * s3 + s1 * s2 * c3) / w4;
            var y = (s1 * c2 + s1 * c3 + c1 * s2 * s3) / w4;
            var z = (-s1 * s3 + c1 * s2 * c3 + s2) / w4;

            return new Quaternion(x, y, z, w);
        }

        protected virtual void FillShape()
        {
            var brush = new SolidColorBrush(Color.FromRgb(127, 127, 127));
            brush.Opacity = opacity;

            this.Material = new DiffuseMaterial(brush);
        }
        #endregion

        #region Abstract Methods
        /// <summary>
        /// Applies the current texture to the visual
        /// </summary>
        public abstract void ReapplyTexture();
        /// <summary>
        /// Tessellates this visual.
        /// </summary>
        /// <returns></returns>
        protected abstract MeshGeometry3D Tessellate();
        #endregion

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            //read name here as well
        
            reader.ReadStartElement("Scale");
            Scale = reader.ReadContentAsDouble();
            reader.ReadEndElement();

            reader.ReadStartElement("Offset");
            Offset = reader.ReadContentAsVector3D();
            reader.ReadEndElement();

            reader.ReadStartElement("TextureOffset");
            TextureOffset = reader.ReadContentAsVector();
            reader.ReadEndElement();

            reader.ReadStartElement("Rotation");
            Rotation = reader.ReadContentAsVector3D();
            reader.ReadEndElement();

            reader.ReadStartElement("Size");
            Size = reader.ReadContentAsVector3D();
            reader.ReadEndElement();

            reader.ReadStartElement("Position");
            Position = reader.ReadContentAsVector3D();
            reader.ReadEndElement();

            reader.ReadStartElement("IsMirrored");
            IsMirrored = Boolean.Parse(reader.ReadContentAsString());
            reader.ReadEndElement();
            
            reader.ReadStartElement("IsDecorative");
            IsDecorative = Boolean.Parse(reader.ReadContentAsString());
            reader.ReadEndElement();

            reader.Read();

            if (reader.NodeType == XmlNodeType.EndElement)
                return;

            reader.ReadStartElement("Animation");
            reader.ReadStartElement("Angles");
            AnimationAngles = reader.ReadContentAsVector3D();
            reader.ReadEndElement();
            reader.ReadStartElement("Durations");
            AnimationDuration = reader.ReadContentAsVector3D();
            reader.ReadEndElement();
            reader.ReadStartElement("Types");
            AnimationType = reader.ReadContentAsVector3D();
            reader.ReadEndElement();
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("type", Guid.ToString());
            writer.WriteAttributeString("name", Name);

            writer.WriteStartElement("Scale");
            writer.WriteString(Scale.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("Offset");
            writer.WriteString(Offset.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("TextureOffset");
            writer.WriteString(TextureOffset.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("Rotation");
            writer.WriteString(Rotation.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("Size");
            writer.WriteString(Size.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("Position");
            writer.WriteString(Position.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("IsMirrored");
            writer.WriteString(IsMirrored.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("IsDecorative");
            writer.WriteString(IsDecorative.ToString());
            writer.WriteEndElement();

            writer.WriteStartElement("Animation");
            writer.WriteStartElement("Angles");
            writer.WriteString(AnimationAngles.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
            writer.WriteStartElement("Durations");
            writer.WriteString(AnimationDuration.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
            writer.WriteStartElement("Types");
            writer.WriteString(AnimationType.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}

