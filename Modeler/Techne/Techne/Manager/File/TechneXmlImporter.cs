/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Techne.Magager;
using Techne.Model;
using Techne.Models;
using Techne.Plugins.Interfaces;

namespace Techne.Manager
{
    internal class TechneXmlImporter
    {
        internal TechneModel Deserialize(Dictionary<string, IShapePlugin> shapes, Stream definitionStream)
        {
            StreamReader streamReader = new StreamReader(definitionStream, leaveOpen: true);
            var input = streamReader.ReadToEnd();

            //streamReader.BaseStream.Seek(0, SeekOrigin.Begin);

            byte[] encodedString;
            if (!input.StartsWith(@"<?xml version=""1.0"" encoding=""utf-8"" ?>"))
                encodedString = Encoding.Unicode.GetBytes(input);
            else
                encodedString = Encoding.UTF8.GetBytes(input);

            // Put the byte array into a stream and rewind it to the beginning
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;

            //XmlTextReader x = new XmlTextReader(ms);

            //streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            var tcnFile = XDocument.Load(ms);

            var tcnXml = tcnFile.Element("Techne");

            if (tcnXml == null)
            {
                tcnXml = tcnFile.Element("Models");

                if (tcnXml == null)
                {
                    //todo: inform the user
                    return new TechneModel
                           {
                               Models = new List<SaveModel>
                                        {
                                            new SaveModel()
                                        }
                           };
                }
            }
            var versionAttribute = tcnXml.Attribute(XName.Get("Version"));

            if (versionAttribute == null)
            {
                return new LegacyTechneImporter().Read(definitionStream, shapes);
            }

            switch (versionAttribute.Value)
            {
                case "2.0":
                    return ReadLegacyXML(ms, shapes);
                    break;
                case "2.1":
                    return ReadLegacyXML(ms, shapes);
                    break;
                case "2.1.1":
                    return ReadLegacyXML(ms, shapes);
                    break;
                case "2.2":
                    return new tcnReader(shapes).Deserialize(tcnXml);
                    break;
                default:
                    return new LegacyTechneImporter().Read(definitionStream, shapes);
                    break;
            }

            return null;
        }

        private TechneModel ReadLegacyXML(MemoryStream ms, Dictionary<string, IShapePlugin> shapes)
        {
            ms.Seek(0, SeekOrigin.Begin);

            XmlReader reader = XmlReader.Create(ms);
            reader.MoveToContent();
            reader.ReadStartElement(reader.Name);

            TechneModel techneModel = new TechneModel();

            techneModel.ReadXml(reader);

            reader.ReadStartElement("Models");
            while (reader.NodeType != XmlNodeType.EndElement && reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement)
                    break;

                SaveModel model = new SaveModel();

                model.TexturePath = reader.GetAttribute("texture");

                reader.ReadStartElement("Model");

                model.ReadXml(reader);

                reader.ReadStartElement("Geometry");

                reader.ReadStartElement("GlScale");
                model.GlScale = reader.ReadContentAsVector3D();
                reader.ReadEndElement();

                while (reader.NodeType != XmlNodeType.EndElement && reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.EndElement)
                        break;

                    var guid = reader.GetAttribute("type");
                    var name = reader.GetAttribute("name");

                    ITechneVisual visual;

                    if (guid.Equals("9EC93754-B48D-4C70-AE4E-84D81AE55396", StringComparison.InvariantCultureIgnoreCase))
                    {
                        visual = new TechneVisualCollection();
                    }
                    else
                    {
                        if (!shapes.ContainsKey(guid))
                            guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();

                        var shapePlugin = shapes[guid];

                        visual = shapePlugin.CreateVisual();
                    }

                    visual.Name = name;

                    reader.ReadStartElement("Shape");
                    visual.ReadXml(reader);
                    reader.ReadEndElement();

                    model.Geometry.Add(visual);
                }

                reader.ReadEndElement();

                techneModel.Models.Add(model);
            }

            reader.ReadEndElement();
            reader.ReadEndElement();

            reader.Close();

            return techneModel;
        }
    }
}

