/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using Techne.Models;
using Techne.Plugins.Interfaces;

namespace Techne.Magager
{
    public class LegacyTechneImporter
    {
        private Dictionary<string, ITechneVisual> Groups = new Dictionary<string, ITechneVisual>();

        public LegacyTechneImporter()
        {
            Points = new Point3DCollection();
            TexCoords = new PointCollection();
            Normals = new Vector3DCollection();

            Materials = new Dictionary<string, MaterialDefinition>();
        }

        private StreamReader reader
        {
            get;
            set;
        }

        private Point3DCollection Points
        {
            get;
            set;
        }

        private PointCollection TexCoords
        {
            get;
            set;
        }

        private Vector3DCollection Normals
        {
            get;
            set;
        }

        public string TexturePath
        {
            get;
            set;
        }

        private IList<ITechneVisual> CurrentGroup
        {
            get;
            set;
        }

        public Dictionary<string, MaterialDefinition> Materials
        {
            get;
            private set;
        }

        public TechneModel Read(string path, Dictionary<string, IShapePlugin> shapes)
        {
            TexturePath = Path.GetDirectoryName(path);
            var s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = Read(s, shapes);
            s.Close();
            return result;
        }

        public TechneModel Read(Stream s, Dictionary<string, IShapePlugin> shapes)
        {
            //string input = System.IO.File.ReadAllText(@"D:\System\Users\Alex\Documents\Visual Studio 2010\Projects\Pokecraft\Modeler\McModeler\McModeler\Models\sample.xml");
            //string input = System.IO.File.ReadAllText(@"D:\System\Users\Alex\Documents\Visual Studio 2010\Projects\Pokecraft\Modeler\McModeler\McModeler\Models\Biped.xml");

            StreamReader reader = new StreamReader(s);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            var input = reader.ReadToEnd();

            XDocument XmlDoc = XDocument.Parse(input);
            var version = XmlDoc.Root.Attribute("Version");

            if (version == null || version.Value == "")
                return OpenOldFormat(XmlDoc, shapes);

            return new TechneModel();
        }

        private TechneModel OpenOldFormat(XDocument XmlDoc, Dictionary<string, IShapePlugin> shapes)
        {
            return (from c in XmlDoc.Descendants("Model")
                    select new TechneModel
                           {
                               Author = c.Descendants("Author").FirstOrDefault() != null ? c.Descendants("Author").FirstOrDefault().Value : "",
                               Description = c.Descendants("Description").FirstOrDefault() != null ? c.Descendants("Description").FirstOrDefault().Value : "",
                               PreviewImage = c.Descendants("PreviewImage").FirstOrDefault() != null ? c.Descendants("PreviewImage").FirstOrDefault().Value : "",
                               ProjectName = c.Descendants("ProjectName").FirstOrDefault() != null ? c.Descendants("ProjectName").FirstOrDefault().Value : "",
                               Name = c.Descendants("ModelName").FirstOrDefault() != null ? c.Descendants("ModelName").FirstOrDefault().Value : "",
                               Models = new List<SaveModel>
                                        {
                                            new SaveModel
                                            {
                                                BaseClass = c.Descendants("BaseClass").FirstOrDefault() != null ? c.Descendants("BaseClass").FirstOrDefault().Value : "",
                                                TexturePath = c.Attribute("texture").Value,
                                                GlScale = new Vector3D(1, 1, 1),
                                                Geometry = (from g in c.Descendants("Group")
                                                            select
                                                                (from e in g.Descendants("Object")
                                                                 select GetShapePlugin(shapes, e.Attribute("type").Value).CreateVisual(
                                                                     size: (from coords in
                                                                                (from offset in e.Descendants("Size")
                                                                                 select offset)
                                                                            select
                                                                                new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture),
                                                                                             Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture),
                                                                                             Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                                     position: (from coords in
                                                                                    (from offset in e.Descendants("Position")
                                                                                     select offset)
                                                                                select
                                                                                    new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture),
                                                                                                 Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture),
                                                                                                 Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                                     offset: (from coords in
                                                                                  (from offset in e.Descendants("Offset")
                                                                                   select offset)
                                                                              select
                                                                                  new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture),
                                                                                               Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture),
                                                                                               Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                                     textureOffset: (from coords in
                                                                                         (from offset in e.Descendants("TextureOffset")
                                                                                          select offset)
                                                                                     select new Vector(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture))).
                                                                         FirstOrDefault(),
                                                                     rotation: (from coords in
                                                                                    (from offset in e.Descendants("Rotation")
                                                                                     select offset)
                                                                                select
                                                                                    new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture),
                                                                                                 Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture),
                                                                                                 Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                                     name: e.Attribute("name").Value,
                                                                     scale: float.Parse(e.Attribute("scale").Value)
                                                                     ))
                                                           ).ConcatToObservableCollection()
                                            }
                                        }
                           }).FirstOrDefault();
        }

        private IShapePlugin GetShapePlugin(Dictionary<string, IShapePlugin> shapes, string guid)
        {
            if (shapes.ContainsKey(guid))
                return shapes[guid];

            guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();

            if (shapes.ContainsKey(guid))
                return shapes[guid];

            //muhahaha
            return null;
        }

        #region Nested type: MaterialDefinition
        public class MaterialDefinition
        {
            // http://en.wikipedia.org/wiki/Material_Template_Library

            public Color Ambient
            {
                get;
                set;
            }

            public Color Diffuse
            {
                get;
                set;
            }

            public Color Specular
            {
                get;
                set;
            }

            public double SpecularCoefficient
            {
                get;
                set;
            }

            public double Dissolved
            {
                get;
                set;
            }

            public int Illumination
            {
                get;
                set;
            }

            public string AmbientMap
            {
                get;
                set;
            }

            public string AlphaMap
            {
                get;
                set;
            }

            public string DiffuseMap
            {
                get;
                set;
            }

            public string SpecularMap
            {
                get;
                set;
            }

            public string BumpMap
            {
                get;
                set;
            }

            public Material GetMaterial()
            {
                var mg = new MaterialGroup();
                if (DiffuseMap == null)
                {
                    var diffuseBrush = new SolidColorBrush(Diffuse)
                                       {
                                           Opacity = Dissolved
                                       };
                    mg.Children.Add(new DiffuseMaterial(diffuseBrush));
                }
                else
                {
                    // var path = Path.Combine(TexturePath, DiffuseMap);
                    var path = DiffuseMap;
                    if (File.Exists(path))
                    {
                        var img = new BitmapImage(new Uri(path, UriKind.Relative));
                        var textureBrush = new ImageBrush(img)
                                           {
                                               Opacity = Dissolved
                                           };
                        mg.Children.Add(new DiffuseMaterial(textureBrush));
                    }
                }

                mg.Children.Add(new SpecularMaterial(new SolidColorBrush(Specular), SpecularCoefficient));

                return mg;
            }
        }
        #endregion
    }
}

