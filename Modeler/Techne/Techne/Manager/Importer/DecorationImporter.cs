/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Techne.Model;
using Techne.Plugins.Attributes;
using Techne.Plugins.Interfaces;

namespace Techne.Importer
{
    [PluginExport("Decoration Importer", "0.1", "ZeuX", "Allows models to be imported as reference", "2E022EA5-4EA0-4197-8391-E4D266813384")]
    [Export(typeof(IImportPlugin))]
    public class DecorationImporter : IImportPlugin
    {
        #region IImportPlugin Members
        public IEnumerable<ITechneVisual> Import(Dictionary<string, IShapePlugin> shapes, string filename)
        {
            var save = FileManager.Load(filename, shapes);

            if (save == null || save.Models == null || save.Models.Count == 0)
                return new List<ITechneVisual>();

            var collection = new TechneVisualFolder()
            {
                Name = save.Name
            };

            foreach (var model in save.Models)
            {
                foreach (var item in model.Geometry)
                {
                    collection.AddChild(item);
                }
            }

            collection.Texture = null;

            return new List<ITechneVisual>
                   {
                       collection
                   };
        }

        public Guid Guid
        {
            get { return new Guid("2E022EA5-4EA0-4197-8391-E4D266813384"); }
        }

        public void OnLoad()
        {
        }

        public string DefaultExtension
        {
            get { return ""; }
        }

        public string Filter
        {
            get { return "Techne Model (*.tcn, *.zip) | *tcn; *.zip"; }
        }

        public string MenuHeader
        {
            get { return "decoration"; }
        }
        #endregion
    }
}

