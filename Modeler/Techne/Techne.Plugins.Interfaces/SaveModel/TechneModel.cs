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
using System.Xml.Serialization;
using System.Globalization;
using Techne;
using Techne.Plugins;
using Techne.Plugins.Attributes.XML;
using System.Windows;

namespace Techne.Models
{
    [TechneXmlRoot("Techne")]
    public class TechneModel : IXmlSerializable
    {
        [TechneXmlEnumerableAttribute("2.0", "Models")]
        public List<SaveModel> Models { get; set; }
        [TechneXmlElement("2.0")]
        public String Name { get; set; }
        [TechneXmlElement("2.0")]
        public String Author { get; set; }
        [TechneXmlElement("2.0")]
        public String ProjectName { get; set; }
        [TechneXmlElement("2.0")]
        public String PreviewImage { get; set; }
        [TechneXmlElement("2.0")]
        public String Description { get; set; }
        public String Location { get; set; }
        public Boolean HasChanged { get; set; }
        [TechneXmlElement("2.0")]
        public DateTime DateCreated { get; set; }
        [TechneXmlAttribute("2.0")]
        public String Version { get { return "2.2"; } }
        [TechneXmlElement("2.1.1")]
        public ProjectType ProjectType { get; set; }

        public TechneModel()
        {
            HasChanged = false;
            Author = "ZeuX";
            Models = new List<SaveModel>();
        }

        public TechneModel(TechneModel model)
        {
            this.Models = model.Models;
            this.Name = model.Name;
            this.Author = model.Author;
            this.Description = model.Description;
            this.Location = model.Location;
            this.ProjectName = model.ProjectName;
            this.PreviewImage = model.PreviewImage;
            this.HasChanged = false;
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.ReadStartElement("Author");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace)
                Author = "ZeuX";
            else
                Author = reader.ReadContentAsString();
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();

            reader.ReadStartElement("Description");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace || !reader.HasValue)
            {
                Description = "";
                ProjectType = Plugins.ProjectType.Minecraft;
            }
            else
            {
                Description = "";
                var tmp = reader.ReadContentAsString();
                int projectId = 0;

                if (Int32.TryParse(tmp, out projectId))
                    ProjectType = (ProjectType)projectId;
                else
                    Description = tmp;
            }
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();

            reader.ReadStartElement("PreviewImage");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace || !reader.HasValue)
                PreviewImage = "";
            else
                PreviewImage = reader.ReadContentAsString();
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();

            reader.ReadStartElement("DateCreated");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace || !reader.HasValue)
                DateCreated = DateTime.Now;
            else
                DateCreated = DateTime.Parse(reader.ReadContentAsString(), CultureInfo.InvariantCulture);
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();

            reader.ReadStartElement("Name");
            if (reader.NodeType == System.Xml.XmlNodeType.Whitespace || !reader.HasValue)
                Name = "Model";
            else
                Name = reader.ReadContentAsString();
            if (reader.NodeType == System.Xml.XmlNodeType.EndElement)
                reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteAttributeString("Version", Version);

            writer.WriteStartElement("Author");
            writer.WriteString(Author);
            writer.WriteEndElement();

            writer.WriteStartElement("Description");
            //writer.WriteString(Description);
            writer.WriteValue((int)ProjectType);
            writer.WriteEndElement();

            writer.WriteStartElement("PreviewImage");
            writer.WriteString(PreviewImage);
            writer.WriteEndElement();

            writer.WriteStartElement("DateCreated");
            writer.WriteString(DateCreated.ToString(CultureInfo.InvariantCulture));
            writer.WriteEndElement();

            writer.WriteStartElement("Name");
            writer.WriteString(Name);
            writer.WriteEndElement();

            writer.WriteStartElement("Models");

            foreach (var item in Models)
            {
                writer.WriteStartElement("Model");
                item.WriteXml(writer);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
    }
}

