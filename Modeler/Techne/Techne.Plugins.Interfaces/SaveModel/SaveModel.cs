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
using System.Windows.Media.Imaging;
using Techne.Plugins.Interfaces;
using System.Xml.Serialization;
using System.Windows.Media.Media3D;
using System.Globalization;
using System.Collections.ObjectModel;
using Techne.Plugins.Attributes.XML;

namespace Techne.Models
{
    [TechneXmlRoot("Model")]
    public class SaveModel : IXmlSerializable
    {
        private BitmapImage texture;
        private System.Windows.Vector textureSize;

        [TechneXmlElement("2.0")]
        public String Name { get; set; }
        [TechneXmlElement("2.0")]
        public String BaseClass { get; set; }
        [TechneXmlAttribute("texture", typeof(string), "2.0", "texture.png")]
        public string TexturePath
        {
            get
            {
                return this.Name + "texture.png";
            }
            set
            {
            }
        }
        [TechneXmlElement("2.2")]
        public System.Windows.Vector TextureSize
        {
            get
            {
                return textureSize;
            }
            set
            {
                textureSize = value;
                foreach (var item in Geometry)
                {
                    item.TextureSize = value;
                }
            }
        }

        public BitmapImage Texture
        {
            get
            {
                return texture;
            }
            set
            {
                texture = value;

                foreach (var item in Geometry)
                {
                    item.Texture = value;
                }
            }
        }
        public System.IO.Stream TextureStream { get; set; }
        [TechneXmlElement("2.0")]
        public Vector3D GlScale { get; set; }

        [TechneXmlEnumerableAttribute("2.0", "Geometry")]
        public ObservableCollection<ITechneVisual> Geometry { get; set; }
        //public IEnumerable<String> 

        public SaveModel()
        {
            Geometry = new ObservableCollection<ITechneVisual>();
            GlScale = new Vector3D(1, 1, 1);
        }

        public SaveModel(SaveModel save)
        {
            this.BaseClass = save.BaseClass;
            Geometry = new ObservableCollection<ITechneVisual>();
            GlScale = new Vector3D(1, 1, 1);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement("BaseClass");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                BaseClass = "ModelBase";
            else
                BaseClass = reader.ReadContentAsString();
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();

            reader.ReadStartElement("Name");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                Name = "Model";
            else
                Name = reader.ReadContentAsString();
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var textureName =  this.Name + "texture.png";
            TexturePath = textureName;
            writer.WriteAttributeString("texture", textureName);
            
            writer.WriteStartElement("BaseClass");
            writer.WriteString(BaseClass);
            writer.WriteEndElement();

            writer.WriteStartElement("Name");
            writer.WriteString(Name);
            writer.WriteEndElement();

            writer.WriteStartElement("Geometry");

            writer.WriteStartElement("GlScale");
            writer.WriteString(GlScale.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            foreach (var item in Geometry)
            {
                writer.WriteStartElement("Shape");
                item.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}

