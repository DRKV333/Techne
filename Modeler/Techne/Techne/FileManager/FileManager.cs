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
using Ionic.Zip;
using System.IO;
using System.Windows.Media;
using Techne.Plugins.Interfaces;

namespace Techne
{
    class FileManager
    {
        private SaveModels groups;

        internal static void Save(string filename, Cinch.DispatcherNotifiedObservableCollection<System.Windows.Media.Media3D.Visual3D> models)
        {
            Save(filename, models, null);
        }

        internal static void Save(string filename, Cinch.DispatcherNotifiedObservableCollection<System.Windows.Media.Media3D.Visual3D> models, Stream textureStream)
        {
            BitmapImage image = new BitmapImage();
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.Append("<Models><Model texture=\"texture.png\"><Author>ZeuX</Author><Description>Standard Model</Description><DateCreated>11.03.2010</DateCreated><Definition>");

            foreach (var item in models)
            {
                var visual = item as ITechneVisual;

                if (visual == null)
                    continue;

                sb.Append("<Group name=\"");
                sb.Append(visual.Name);
                sb.Append("\">");

                sb.AppendLine(CreateConstructor(visual));
                sb.AppendLine(AddBox(visual));

                //if (image == null)
                    //image = visual.Texture;

                sb.Append("</Object></Group>");
            }

            sb.Append("</Definition></Model></Models>");

            using (ZipFile zip = new ZipFile(filename))
            {
                zip.AddEntry("model.xml", ASCIIEncoding.ASCII.GetBytes(sb.ToString()));

                if (textureStream != null)
                {
                    //todo: don't create the image...
                    //zip.AddEntry("texture.png", BufferFromImage(image));

                    //edit: I don't it now :)
                    textureStream.Seek(0, SeekOrigin.Begin);
                    zip.AddEntry("texture.png", textureStream);
                }

                zip.Save();
            }
        }

        public static Byte[] BufferFromImage(BitmapImage bitmapImage)
        {
            //int height = bitmapImage.PixelHeight;
            //int width = bitmapImage.PixelWidth;
            //int stride = width * ((bitmapImage.Format.BitsPerPixel + 7) / 8);

            //byte[] bits = new byte[height * stride];
            //bitmapImage.CopyPixels(bits, stride, 0);

            //return bits;
            MemoryStream memStream = new MemoryStream();
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage.StreamSource));
            encoder.Save(memStream);
            return memStream.GetBuffer();
        }

        private static string CreateConstructor(ITechneVisual item)
        {
            StringBuilder sb = new StringBuilder();
            //<Object type="box" name="head" scale="0">
            sb.Append("<Object type=\"box\" name=\"");
            sb.Append(item.Name);
            sb.Append("\" scale=\"0\">");

            return sb.ToString();
        }

        private static string AddBox(ITechneVisual item)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<Offset x=\"");
            sb.Append(item.Offset.X);
            sb.Append("\" y=\"");
            sb.Append(item.Offset.Y);
            sb.Append("\" z=\"");
            sb.Append(item.Offset.Z);
            sb.Append("\" />");

            sb.Append("<TextureOffset x=\"");
            sb.Append(item.TextureOffset.X);
            sb.Append("\" y=\"");
            sb.Append(item.TextureOffset.Y);
            sb.Append("\" />");

            sb.Append("<Rotation x=\"");
            sb.Append(item.RotationX);
            sb.Append("\" y=\"");
            sb.Append(item.RotationY);
            sb.Append("\" z=\"");
            sb.Append(item.RotationZ);
            sb.Append("\" />");

            sb.Append("<Size x=\"");
            sb.Append(item.Size.X);
            sb.Append("\" y=\"");
            sb.Append(item.Size.Y);
            sb.Append("\" z=\"");
            sb.Append(item.Size.Z);
            sb.Append("\" />");

            sb.Append("<Position x=\"");
            sb.Append(item.Position.X);
            sb.Append("\" y=\"");
            sb.Append(item.Position.Y);
            sb.Append("\" z=\"");
            sb.Append(item.Position.Z);
            sb.Append("\" />");

            return sb.ToString();
        }

        internal IEnumerable<ITechneVisual> CreateModel(Dictionary<string, Techne.Plugins.Interfaces.IShapePlugin> shapes, SaveModels model)
        {
            var result = new List<ITechneVisual>();

            //Image image = new Image()
            //{
            //    Source = model.Texture
            //};

            //RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.NearestNeighbor);

            var brush = new VisualBrush();

            foreach (var geometry in model.Geometry)
            {
                foreach (var box in geometry.Objects)
                {
                    string guid;

                    if (box.Guid == null)
                        guid = "D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower();
                    else
                        guid = box.Guid;

                    //todo: should add some form of feedback
                    if (!shapes.ContainsKey(guid))
                        continue;

                    var group = shapes[guid].CreateVisual();

                    group.Name = box.Name;
                    group.Offset = box.Offset;
                    group.Width = box.Size.X;
                    group.Length = box.Size.Y;
                    group.Height = box.Size.Z;
                    group.Position = box.Position;
                    group.RotationX = box.Rotation.X;
                    group.RotationY = box.Rotation.Y;
                    group.RotationZ = box.Rotation.Z;

                    if (model.Texture != null)
                    {
                        group.TextureOffset = box.TextureOffset;
                        //group.TextureSize = new System.Windows.Vector(model.Texture.Width, model.Texture.Height);
                        group.Texture = model.Texture;
                    }

                    result.Add(group);
                    //groups.Add(box.Name, group);
                }
            }

            return result;

        }

        internal SaveModels Load(string filename)
        {
            groups = new SaveModels();
            groups.Geometry = new List<SaveGroup>();
            groups.Location = filename;

            using (ZipFile zip = ZipFile.Read(filename))
            {
                var modelFiles = zip.Entries.Where(z => z.FileName.EndsWith(".xml"));

                if (modelFiles.Count() <= 0)
                {
                    return null;
                }

                TechneXMLImporter importer = new TechneXMLImporter();

                foreach (var item in modelFiles)
                {
                    using (MemoryStream definitionStream = new MemoryStream())
                    {
                        item.Extract(definitionStream);

                        //importer.Read(definitionStream);

                        var models = importer.Read(definitionStream);
                        
                        foreach (var model in models)
                        {
                            var texture = zip.Entries.Where(e => e.FileName.Equals(model.TexturePath, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                            if (texture != null)
                            {
                                    MemoryStream textureStream = new MemoryStream();
                                //using (MemoryStream textureStream = new MemoryStream())
                                //{
                                    texture.Extract(textureStream);

                                    model.Texture = new BitmapImage();
                                    model.Texture.BeginInit();
                                    model.Texture.CacheOption = BitmapCacheOption.OnLoad;
                                    model.Texture.StreamSource = textureStream;
                                    model.Texture.EndInit();

                                    if (groups.Texture == null)
                                    {
                                        groups.Texture = model.Texture;
                                        groups.TextureStream = textureStream;
                                    }
                                //}
                            }

                            foreach (var geometry in model.Geometry)
                            {

                                ((List<SaveGroup>)groups.Geometry).Add(geometry);
                                
                            }
                            groups.Author = model.Author;
                            groups.Description = model.Description;
                            groups.Name = model.Name;
                            //CreateModel(shapes, model);
                        }
                    }
                }
                return this.groups;
            }
        }
    }
}

