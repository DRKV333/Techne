/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Techne.Plugins.Attributes;
//using System.ComponentModel.Composition;
//using Techne.Plugins.Interfaces;

//namespace Techne.Importer
//{
//    [PluginExportAttribute("Fixed Importer", "0.1", "ZeuX", "Allows models to be imported as reference", "713C289D-A987-415B-B1C4-973EB643C0A2")]
//    [Export(typeof(IImportPlugin))]
//    public class FixedImporter : IImportPlugin
//    {
//        public IEnumerable<ITechneVisual> Import(Dictionary<string, Techne.Plugins.Interfaces.IShapePlugin> shapes, string filename)
//        {
//            FileManager manager = new FileManager();
//            var save = manager.Load(filename);
//            var model = manager.CreateModel(shapes, save);

//            foreach (var item in model)
//            {
//                item.IsFixed = true;
//            }

//            return model;
//        }

//        public Guid Guid
//        {
//            get { return new Guid("713C289D-A987-415B-B1C4-973EB643C0A2"); }
//        }

//        public void OnLoad()
//        {

//        }

//        public string DefaultExtension
//        {
//            get { return ""; }
//        }

//        public string Filter
//        {
//            get { return "Techne Model (*.tcn, *.zip) | *tcn; *.zip"; }
//        }

//        public string MenuHeader
//        {
//            get { return "fixpoint"; }
//        }
//    }
//}


