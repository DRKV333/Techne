/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Xml.Linq;
using System.Linq;
using System.Windows.Controls;
using Techne.Plugins.Interfaces;


namespace Techne
{
    public class TechneXMLImporter
    {
        private StreamReader reader { get; set; }
        private Point3DCollection Points { get; set; }
        private PointCollection TexCoords { get; set; }
        Dictionary<string, ITechneVisual> Groups = new Dictionary<string, ITechneVisual>();
        private Vector3DCollection Normals { get; set; }
        public string TexturePath { get; set; }
        IList<ITechneVisual> CurrentGroup { get; set; }

        public class MaterialDefinition
        {
            // http://en.wikipedia.org/wiki/Material_Template_Library

            public Color Ambient { get; set; }
            public Color Diffuse { get; set; }
            public Color Specular { get; set; }
            public double SpecularCoefficient { get; set; }
            public double Dissolved { get; set; }
            public int Illumination { get; set; }
            public string AmbientMap { get; set; }
            public string AlphaMap { get; set; }
            public string DiffuseMap { get; set; }
            public string SpecularMap { get; set; }
            public string BumpMap { get; set; }

            public Material GetMaterial()
            {
                var mg = new MaterialGroup();
                if (DiffuseMap == null)
                {
                    var diffuseBrush = new SolidColorBrush(Diffuse) { Opacity = Dissolved };
                    mg.Children.Add(new DiffuseMaterial(diffuseBrush));
                }
                else
                {
                    // var path = Path.Combine(TexturePath, DiffuseMap);
                    var path = DiffuseMap;
                    if (File.Exists(path))
                    {

                        var img = new BitmapImage(new Uri(path, UriKind.Relative));
                        var textureBrush = new ImageBrush(img) { Opacity = Dissolved };
                        mg.Children.Add(new DiffuseMaterial(textureBrush));
                    }
                }

                mg.Children.Add(new SpecularMaterial(new SolidColorBrush(Specular), SpecularCoefficient));

                return mg;
            }
        }

        public Dictionary<string, MaterialDefinition> Materials { get; private set; }

        public TechneXMLImporter()
        {
            Points = new Point3DCollection();
            TexCoords = new PointCollection();
            Normals = new Vector3DCollection();

            Materials = new Dictionary<string, MaterialDefinition>();
        }

        public IEnumerable<SaveModels> Read(string path)
        {
            TexturePath = Path.GetDirectoryName(path);
            var s = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            var result = Read(s);
            s.Close();
            return result;
        }

        public IEnumerable<SaveModels> Read(Stream s)
        {
            //string input = System.IO.File.ReadAllText(@"D:\System\Users\Alex\Documents\Visual Studio 2010\Projects\Pokecraft\Modeler\McModeler\McModeler\Models\sample.xml");
            //string input = System.IO.File.ReadAllText(@"D:\System\Users\Alex\Documents\Visual Studio 2010\Projects\Pokecraft\Modeler\McModeler\McModeler\Models\Biped.xml");

            StreamReader reader = new StreamReader(s);
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            var input = reader.ReadToEnd();

            XDocument XmlDoc = System.Xml.Linq.XDocument.Parse(input);

            return (from c in XmlDoc.Descendants("Model")
                    select new SaveModels()
                   {
                       //Author = c.Descendants("Author").FirstOrDefault().Value,
                       //Description = c.Descendants("Description").FirstOrDefault().Value,
                       //DateCreated = c.Descendants("Texture").FirstOrDefault().Value,
                       TexturePath = c.Attribute("texture").Value,
                       Geometry = from g in c.Descendants("Group")
                                  select new SaveGroup()
                                  {
                                      Name = g.Attribute("name").Value,
                                      Objects = from objects in g.Descendants("Object")
                                                select new SaveObject()
                                                {
                                                    Name = objects.Attribute("name").Value,
                                                    Scale = objects.Attribute("scale").Value,
                                                    Type = objects.Attribute("type").Value,
                                                    Offset = (from coords in
                                                                  (from offset in objects.Descendants("Offset")
                                                                   select offset)
                                                              select new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                    Position = (from coords in
                                                                    (from offset in objects.Descendants("Position")
                                                                     select offset)
                                                                select new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                    TextureOffset = (from coords in
                                                                         (from offset in objects.Descendants("TextureOffset")
                                                                          select offset)
                                                                     select new Vector(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                    Size = (from coords in
                                                                (from offset in objects.Descendants("Size")
                                                                 select offset)
                                                            select new Vector3D(Double.Parse(coords.Attribute("x").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("y").Value, CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("z").Value, CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                    Rotation = (from coords in
                                                                    (from offset in objects.Descendants("Rotation")
                                                                 select offset)
                                                                select new Vector3D(Double.Parse(coords.Attribute("x").Value.Replace(',', '.'), CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("y").Value.Replace(',', '.'), CultureInfo.InvariantCulture), Double.Parse(coords.Attribute("z").Value.Replace(',', '.'), CultureInfo.InvariantCulture))).FirstOrDefault(),
                                                }

                                  }
                   });
        }
    }
}


