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
using System.ComponentModel.Composition;
using Techne.Plugins.Attributes;
using Techne.Plugins.Interfaces;
using System.IO;
using Techne.Models;

namespace Techne.Plugins.FileHandler.Java
{
    [PluginExportAttribute("Adventure-craft Exporter", "0.2", "ZeuX", "Allows models to be exported as ac-script", "9675DC01-3EF1-4023-AC80-2C65B1A6517A")]
    [Export(typeof(IExportPlugin))]
    public class AdventureCraftExporter : IExportPlugin
    {
        public PluginType PluginType { get { return PluginType.Exporter; } }
        public Guid Guid { get { return new Guid("9675DC01-3EF1-4023-AC80-2C65B1A6517A"); } }
        public string DefaultExtension
        {
            get { return "java"; }
        }

        public string Filter
        {
            get
            {
                return "ac-script file|*.txt";
            }
        }

        public String MenuHeader
        {
            get
            {
                return "Adventure-craft";
            }
        }

        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel saveModel)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("//Model for Adventurecraft exported by Techne");
            sb.AppendLine("//I'm pretty sure I missed or misunderstood something. If I did, please let me know");
            sb.AppendLine("//you can find me in #techne or #adventurecraft :)");
            sb.AppendLine("// - ZeuX");
            sb.AppendLine();

            foreach (var item in visuals)
            {
                if (item == null)
                    continue;

                var itemName = item.Name;
                
                item.Name = itemName.Replace(' ', '_');

                WriteConstructor(item, sb);
                WriteAddBox(item, sb);
                WriteSetPosition(item, sb);
                WriteSetRotation(item, sb);

                sb.AppendLine();

                item.Name = itemName;
            }

            File.WriteAllText(filename, sb.ToString());
        }

        private void WriteConstructor(ITechneVisual item, StringBuilder sb)
        {
            sb.Append("var ");
            sb.Append(item.Name);
            sb.Append(" = new Model();");
            sb.AppendLine();
        }

        private void WriteSetRotation(ITechneVisual item, StringBuilder sb)
        {
            sb.Append(item.Name);
            sb.Append(".setRotation(");
            sb.Append(Math.Round(item.RotationY, 5).ToString().Replace(',', '.'));
            sb.Append(", ");
            sb.Append(Math.Round(item.RotationX * -1, 5).ToString().Replace(',', '.'));
            sb.Append(", ");
            sb.Append(Math.Round(item.RotationZ, 5).ToString().Replace(',', '.'));
            sb.Append(");");
            sb.AppendLine();
        }

        public void OnLoad()
        {

        }

        private void WriteAddBox(ITechneVisual item, StringBuilder sb)
        {
            sb.Append(item.Name);
            sb.Append(".addBox(\"");
            sb.Append(item.Name);
            sb.Append("\", ");
            sb.Append(item.Offset.X);
            sb.Append(", ");
            sb.Append(item.Offset.Y);
            sb.Append(", ");
            sb.Append(item.Offset.Z);
            sb.Append(", ");
            sb.Append(item.Size.X);
            sb.Append(", ");
            sb.Append(item.Size.Y);
            sb.Append(", ");
            sb.Append(item.Size.Z);
            sb.Append(", ");
            sb.Append(item.TextureOffset.X);
            sb.Append(", ");
            sb.Append(item.TextureOffset.Y);
            sb.Append(");");
            sb.AppendLine();
        }

        private void WriteSetPosition(ITechneVisual item, StringBuilder sb)
        {
            sb.Append(item.Name);
            sb.Append(".setPosition(");
            sb.Append(item.Position.X / 16);
            sb.Append(", ");
            sb.Append(item.Position.Y / 16 * -1);
            sb.Append(", ");
            sb.Append(item.Position.Z / 16);
            sb.Append(");");
            sb.AppendLine();
        }
    }

}

