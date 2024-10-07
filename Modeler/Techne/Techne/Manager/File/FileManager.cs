/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Ionic.Zip;
using Techne.Manager;
using Techne.Models;
using Techne.Plugins.Interfaces;

namespace Techne
{
    internal class FileManager
    {
        internal static void Save(string filename, TechneModel techneModels)
        {
            BitmapImage image = new BitmapImage();
            StringBuilder sb = new StringBuilder();

            tcnReader reader = new tcnReader();
            var element = reader.Serialize(techneModels);

            using (ZipFile zip = new ZipFile(filename))
            {
                zip.AddEntry("model.xml", element.ToString());

                foreach (var item in techneModels.Models)
                {
                    if (item.TextureStream != null)
                    {
                        item.TextureStream.Seek(0, SeekOrigin.Begin);
                        zip.AddEntry(item.TexturePath, item.TextureStream);
                    }
                }

                zip.Save();
            }
        }

        internal TechneModel Load(string filename, Dictionary<string, IShapePlugin> shapes)
        {
            using (ZipFile zip = ZipFile.Read(filename))
            {
                var modelFiles = zip.Entries.Where(z => z.FileName.EndsWith(".xml"));

                if (!modelFiles.Any())
                {
                    return null;
                }

                TechneXmlImporter deserializer = new TechneXmlImporter();

                foreach (var item in modelFiles)
                {
                    using (MemoryStream definitionStream = new MemoryStream())
                    {
                        item.Extract(definitionStream);

                        var techneModels = deserializer.Deserialize(shapes, definitionStream);

                        foreach (var model in techneModels.Models)
                        {
                            var texture = zip.Entries.Where(e => e.FileName.Equals(model.TexturePath ?? "texture.png", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                            if (texture != null)
                            {
                                MemoryStream textureStream = new MemoryStream();
                                texture.Extract(textureStream);

                                var textureImage = new BitmapImage();
                                textureImage.BeginInit();
                                textureImage.CacheOption = BitmapCacheOption.OnLoad;
                                textureImage.StreamSource = textureStream;
                                textureImage.EndInit();

                                model.Texture = textureImage;

                                model.TextureStream = textureStream;
                            }
                        }

                        return techneModels;
                    }
                }
            }

            return null;
        }
    }
}

