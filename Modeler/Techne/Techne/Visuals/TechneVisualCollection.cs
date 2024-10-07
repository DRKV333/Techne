/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml;
using System.Xml.Schema;
using Techne.Plugins;
using Techne.Plugins.Attributes.XML;
using Techne.Plugins.Interfaces;
using System.Collections.ObjectModel;

namespace Techne.Model
{
    [TechneXmlRoot("Piece")]
    public class TechneVisualCollection : ModelVisual3D, ITechneVisualCollection
    {
        public Guid guid = new Guid("9EC93754-B48D-4C70-AE4E-84D81AE55396");

        public static DependencyProperty nameProperty = DependencyProperty.Register("Name", typeof(String), typeof(TechneVisualCollection));
        public static DependencyProperty positionProperty = DependencyProperty.Register("Position", typeof(Vector3D), typeof(TechneVisualCollection));
        public static DependencyProperty combinedPositionProperty = DependencyProperty.Register("CombinedPosition", typeof(Vector3D), typeof(TechneVisualCollection));
        public static DependencyProperty rotationXProperty = DependencyProperty.Register("RotationX", typeof(Double), typeof(TechneVisualCollection));
        public static DependencyProperty rotationYProperty = DependencyProperty.Register("RotationY", typeof(Double), typeof(TechneVisualCollection));
        public static DependencyProperty rotationZProperty = DependencyProperty.Register("RotationZ", typeof(Double), typeof(TechneVisualCollection));

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneVisualCollection"/> class.
        /// </summary>
        public TechneVisualCollection()
        {
            AnimationType = new Vector3D();
            Children = new ObservableCollection<ITechneVisual>();
        }

        public Boolean IsSelected { get; set; }

        public bool ChildrenInheritProperties
        {
            get
            {
                return true;
            }
        }

        public ITechneVisualCollection Parent
        {
            get;
            set;
        }

        [TechneXmlEnumerableAttribute("Piece", typeof(ObservableCollection<ITechneVisual>), "2.2", null)]
        public new ObservableCollection<ITechneVisual> Children
        {
            get;
            private set;
        }

        #region ITechneVisual Members
        /// <summary>
        /// Visual is rotatable but opacity stays the same
        /// </summary>
        public Boolean IsDecorative
        {
            get { return false; }
            set
            {
            }
        }

        /// <summary>
        /// slightly opaque with fixed texture and fixed position
        /// </summary>
        public Boolean IsFixed
        {
            get { return false; }
            set
            {
            }
        }

        /// <summary>
        /// Gets or sets the texture coordinates of this visual
        /// </summary>
        public List<Point[]> TextureCoordinates
        {
            get { return new List<Point[]>(); }
            set { }
        }

        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        public Vector3D Offset
        {
            get
            {
                return new Vector3D();
                //return offset;
            }
            set
            {
            }
        }

        public virtual Vector3D CombinedPosition
        {
            set
            {
                //this is a dummy and may need to be extended
                SetValue(combinedPositionProperty, value);
            }
            get
            {
                return (Vector3D)GetValue(combinedPositionProperty);
            }
        }

        public void CalculateCombinedPosition()
        {
            var position = (Vector3D)GetValue(positionProperty);
            if (Parent != null)
            {
                position += Parent.CombinedPosition;
            }
            CombinedPosition = position;

            foreach (var techneVisual in Children)
            {
                techneVisual.CalculateCombinedPosition();
            }
        }

        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        [TechneXmlElement("2.2")]
        public Vector3D Position
        {
            get
            {
                return (Vector3D)GetValue(positionProperty);
            }
            set
            {
                SetValue(positionProperty, value);
                UpdateModel();

                CalculateCombinedPosition();
            }
        }

        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        public Vector TextureOffset
        {
            get
            {
                return new Vector();
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the size of this visual
        /// </summary>
        public Vector3D Size
        {
            get
            {
                return new Vector3D();
            }
            set
            {

            }
        }

        public Point3D Center
        {
            get { return new Point3D(); }
            set { }
        }

        [TechneXmlElement("2.2")]
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
                return (Double)GetValue(rotationXProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(rotationXProperty, value);
                RotateModel();
            }
        }
        public virtual Double RotationY
        {
            get
            {
                return (Double)GetValue(rotationYProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(rotationYProperty, value);
                RotateModel();
            }
        }
        public virtual Double RotationZ
        {
            get
            {
                return (Double)GetValue(rotationZProperty);
            }
            set
            {
                if (IsFixed)
                    return;

                SetValue(rotationZProperty, value);
                RotateModel();
            }
        }

        public Vector3D AnimationAngles
        {
            get { return new Vector3D(); }
            set { }
        }

        public Vector3D AnimationDuration
        {
            get { return new Vector3D(); }
            set { }
        }

        public Vector3D AnimationType
        {
            get { return new Vector3D(); }
            set { }
        }

        public Boolean IsMirrored
        {
            get { return false; }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the texture of this visual
        /// </summary>
        public BitmapSource Texture
        {
            get { return null; }
            set
            {
                foreach (var item in Children)
                {
                    item.Texture = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the name of this visual used for saving and especially exporting
        /// </summary>
        [TechneXmlAttribute("2.0")]
        public virtual String Name
        {
            get { return (String)GetValue(nameProperty); }
            set
            {
                SetValue(nameProperty, value.Replace("_", ""));
            }
        }

        public Double Opacity
        {
            get { return 1; }
            set
            {

            }
        }

        /// <summary>
        /// Gets or sets the texture size of this visual
        /// </summary>
        public Vector TextureSize
        {
            get { return new Vector(); }
            set
            {
                foreach (var item in Children)
                {
                    item.TextureSize = value;
                }
            }
        }

        public Dictionary<string, MetadataBase> Metadata
        {
            get;
            set;
        }

        public Double Width
        {
            get { return 0; }
            set
            {

            }
        }

        public Double Height
        {
            get { return 0; }
            set
            {

            }
        }

        public Double Length
        {
            get { return 0; }
            set
            {

            }
        }

        [TechneXmlAttribute("type", typeof(Guid), "2.0", "")]
        public Guid Guid
        {
            get { return guid; }
            set { }
        }

        protected virtual void RotateModel()
        {
            var rotation = new RotateTransform3D
            {
                CenterX = Position.X,
                CenterY = Position.Y,
                CenterZ = Position.Z,
                Rotation =
                    new QuaternionRotation3D(CalculateQuaternion(RotationY, RotationZ, RotationX))
            };

            this.Transform = rotation;

            foreach (var child in Children)
            {
                child.UpdateModel();
            }
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            //Name = reader.GetAttribute("name");
            //read name as well

            while (reader.NodeType != XmlNodeType.EndElement && reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                    break;

                var guid = reader.GetAttribute("type");
                var name = reader.GetAttribute("name");

                ITechneVisual visual;

                if (guid.Equals("9ec93754-b48d-4c70-ae4e-84d81ae55396"))
                {
                    visual = new TechneVisualCollection();
                }
                else
                {
                    if (!ExtensionManager.ShapePlugins.ContainsKey(guid))
                        guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();

                    var shapePlugin = ExtensionManager.ShapePlugins[guid];

                    visual = shapePlugin.CreateVisual();
                }

                reader.ReadStartElement("Shape");
                visual.ReadXml(reader);
                reader.ReadEndElement();

                visual.Name = name;

                AddChild(visual);
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteAttributeString("type", Guid.ToString());
            writer.WriteAttributeString("name", Name);

            foreach (var item in Children)
            {
                writer.WriteStartElement("Shape");
                item.WriteXml(writer);
                writer.WriteEndElement();
            }
        }
        #endregion

        public void AddChild(ITechneVisual item)
        {
            var rotation = item.Transform as RotateTransform3D;
            if (rotation != null)
                rotation.Rotation = new QuaternionRotation3D(new Quaternion(0, 0, 0, 1));

            item.Parent = this;
            Children.Add(item);
        }

        public void InsertChild(int index, ITechneVisual iTechneVisual)
        {
            if (index > Children.Count)
                index = Children.Count;

            var rotation = iTechneVisual.Transform as RotateTransform3D;
            if (rotation != null)
                rotation.Rotation = new QuaternionRotation3D(new Quaternion(0, 0, 0, 1));

            iTechneVisual.Parent = this;
            Children.Insert(index, iTechneVisual);
        }

        public void RemoveChild(ITechneVisual item)
        {
            try
            {
                item.Parent = null;
                Children.Remove(item);
            }
            catch (NullReferenceException ex)
            {
                if (!ex.TargetSite.Name.Equals("EvaluateOldNewStates", StringComparison.InvariantCultureIgnoreCase))
                    throw ex;
            }
        }

        public void UpdateModel()
        {
            foreach (var child in Children)
            {
                child.UpdateModel();
            }
        }
    }
}

