/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Techne.Manager;
using Techne.Models;
using Techne.Plugins.Interfaces;

namespace Techne
{
    internal static class FileManager
    {
        internal static void Save(string filename, TechneModel techneModels)
        {
            tcnReader reader = new();
            var element = reader.Serialize(techneModels);

            using ZipArchive zip = new(File.Create(filename), ZipArchiveMode.Create);

            ZipArchiveEntry modelEntry = zip.CreateEntry("model.xml");
            using (StreamWriter writer = new(modelEntry.Open()))
            {
                writer.Write(element.ToString());
            }

            foreach (var item in techneModels.Models)
            {
                if (item.TextureStream != null)
                {
                    ZipArchiveEntry textureEntry = zip.CreateEntry(item.TexturePath);
                    using Stream textureEntrySteam = textureEntry.Open();

                    item.TextureStream.Seek(0, SeekOrigin.Begin);
                    item.TextureStream.CopyTo(textureEntrySteam);
                }
            }
        }

        internal static TechneModel Load(string filename, Dictionary<string, IShapePlugin> shapes)
        {
            using ZipArchive zip = new(File.OpenRead(filename), ZipArchiveMode.Read);
            
            var modelFiles = zip.Entries.Where(z => z.Name.EndsWith(".xml")).ToList();

            if (modelFiles.Count == 0)
            {
                return null;
            }

            TechneXmlImporter deserializer = new();

            foreach (var item in modelFiles)
            {
                using Stream definitionStream = item.Open();
                var techneModels = deserializer.Deserialize(shapes, definitionStream);

                foreach (var model in techneModels.Models)
                {
                    var texture = zip.Entries.Where(e => e.Name.Equals(model.TexturePath ?? "texture.png", StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

                    if (texture != null)
                    {
                        MemoryStream textureStream = new();
                        using (Stream entryStream = texture.Open())
                        {
                            entryStream.CopyTo(textureStream);
                        }

                        var textureImage = new BitmapImage();
                        textureImage.BeginInit();
                        textureImage.CacheOption = BitmapCacheOption.OnDemand;
                        textureImage.StreamSource = textureStream;
                        textureImage.EndInit();

                        model.Texture = textureImage;

                        model.TextureStream = textureStream;
                    }
                }

                return techneModels;
            }

            return null;
        }
    }
}

