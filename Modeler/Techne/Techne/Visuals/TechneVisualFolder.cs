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
    [TechneXmlRoot("Folder")]
    public class TechneVisualFolder : ModelVisual3D, ITechneVisualCollection
    {
        public static DependencyProperty nameProperty = DependencyProperty.Register("Name", typeof(String), typeof(TechneVisualFolder));

        public Guid guid = new Guid("F8BF7D5B-37BF-455B-93F9-B6F9E81620E1");

        /// <summary>
        /// Initializes a new instance of the <see cref="TechneVisualFolder"/> class.
        /// </summary>
        public TechneVisualFolder()
        {
            AnimationType = new Vector3D();
            Children = new ObservableCollection<ITechneVisual>();
        }

        public Boolean IsSelected { get; set; }

        public bool ChildrenInheritProperties
        {
            get
            {
                return false;
            }
        }

        public ITechneVisualCollection Parent
        {
            get;
            set;
        }

        [TechneXmlEnumerableAttribute("Folder", typeof(ObservableCollection<ITechneVisual>), "2.2", null)]
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
            get { return true; }
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
                
            }
            get
            {
                if (Parent != null)
                {
                    return Parent.CombinedPosition;
                }
                return new Vector3D();
            }
        }

        public void CalculateCombinedPosition()
        {
            foreach (var techneVisual in Children)
            {
                techneVisual.CalculateCombinedPosition();
            }
        }

        /// <summary>
        /// Gets or sets the offset of this visual
        /// </summary>
        public Vector3D Position
        {
            get
            {
                if (Parent != null)
                    return Parent.Position;

                return new Vector3D();
            }
            set
            {
                
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
            set {  }
        }

        public Double RotationX
        {
            get { return 0; }
            set
            {
            }
        }

        public Double RotationY
        {
            get { return 0; }
            set
            {
            }
        }

        public Double RotationZ
        {
            get { return 0; }
            set
            {
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
        }

        public void WriteXml(XmlWriter writer)
        {
        }
        #endregion

        public void AddChild(ITechneVisual item)
        {
            item.Parent = this;
            Children.Add(item);
        }

        public void InsertChild(int index, ITechneVisual iTechneVisual)
        {
            if (index > Children.Count)
                index = Children.Count;

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

