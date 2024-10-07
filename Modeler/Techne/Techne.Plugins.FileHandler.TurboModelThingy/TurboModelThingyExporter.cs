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
using Techne.Plugins.Interfaces;
using Techne.Plugins.Attributes;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;
using System.Reflection;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    [PluginExportAttribute("Turbo Model Thingy Exporter", "0.1", "ZeuX", "Allows models to be exported as java for tmt", "1CD6C3E3-E9EA-419C-BC1B-3E12D48E554D")]
    [Export(typeof(IExportPlugin))]
    public class TurboModelThingyExporter : IExportPlugin
    {
        private Dictionary<string, MemberDescriptor> exportDefinitions = new Dictionary<string, MemberDescriptor>();

        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, SaveModel saveModel)
        {
            StringBuilder constructorContents = new StringBuilder();
            StringBuilder renderContents = new StringBuilder();
            StringBuilder fieldDefinitions = new StringBuilder();

            CreateContents(visuals, constructorContents, renderContents, fieldDefinitions);

            StringBuilder javaClass = new StringBuilder();

            javaClass.AppendLine("package net.minecraft.src;");
            javaClass.AppendLine("//Exported java file");
            javaClass.AppendLine("//Keep in mind that you still need to fill in some blanks");
            javaClass.AppendLine("// - ZeuX");
            javaClass.AppendLine();

            javaClass.Append("public class ");
            javaClass.Append(saveModel.Name);
            javaClass.Append(" extends ");
            javaClass.Append(saveModel.BaseClass);

            javaClass.AppendLine("{");
            javaClass.AppendLine();
            javaClass.AppendLine("public Model()");
            javaClass.AppendLine("{");

            javaClass.AppendLine(constructorContents.ToString());

            javaClass.AppendLine("}");

            javaClass.AppendLine("public void render(float f, float f1, float f2, float f3, float f4, float f5)");
            javaClass.AppendLine("{");
            javaClass.AppendLine("setRotationAngles(f, f1, f2, f3, f4, f5);");

            javaClass.AppendLine(renderContents.ToString());

            javaClass.AppendLine("}");

            javaClass.AppendLine();
            javaClass.AppendLine("//fields");
            javaClass.AppendLine();
            javaClass.AppendLine(fieldDefinitions.ToString());
            javaClass.AppendLine("}");

            File.WriteAllText(filename, javaClass.ToString());
        }

        private void CreateContents(IEnumerable<ITechneVisual> visuals, StringBuilder constructorContents, StringBuilder renderContents, StringBuilder fieldDefinitions)
        {
            foreach (var visual in visuals)
            {
                string originalName = visual.Name;

                visual.Name = originalName.Replace(' ', '_');

                CreateExportDefinition(visual);

                CreateVisualConstructorContents(constructorContents, visual, fieldDefinitions);
                CreateVisualRenderContents(renderContents, visual);

                visual.Name = originalName;
            }
        }

        private void CreateVisualRenderContents(StringBuilder renderContents, ITechneVisual visual)
        {
            renderContents.Append(visual.Name);
            renderContents.Append(".render(f5);");
        }

        private void CreateVisualConstructorContents(StringBuilder constructorContents, ITechneVisual visual, StringBuilder fieldDefinitions)
        {
            //appending constructor
            constructorContents.AppendLine(GenerateConstructor(visual, exportDefinitions[visual.GetType().ToString()]));
            constructorContents.AppendLine();

            //appending method calls

            foreach (var item in exportDefinitions[visual.GetType().ToString()].MethodEntries)
            {
                constructorContents.AppendLine(CallMethod(visual, item));

                fieldDefinitions.Append("public ");
                fieldDefinitions.Append(item[0].Attribute.Name);
                fieldDefinitions.Append(" ");
                fieldDefinitions.Append(visual.Name);
                fieldDefinitions.AppendLine(";");
            }

            constructorContents.AppendLine();


            foreach (var item in exportDefinitions[visual.GetType().ToString()].FiedEntries)
            {
                constructorContents.AppendLine(SetField(visual, item.FirstOrDefault()));
            }
        }

        private void CreateExportDefinition(ITechneVisual visual)
        {
            //var exportDefinition = new Dictionary<string, Dictionary<string, List<KeyValuePair<int, System.Reflection.PropertyInfo>>>>();

            string visualType = visual.GetType().ToString();

            if (exportDefinitions.ContainsKey(visualType))
                return;

            MemberDescriptor descriptor = new MemberDescriptor();

            foreach (var property in visual.GetType().GetProperties())
            {
                var attributes = property.GetCustomAttributes(typeof(JavaExporterDescriptionBase), true) as JavaExporterDescriptionBase[];

                if (attributes == null)
                    continue;

                var attribute = attributes.FirstOrDefault();

                if (attribute == null)
                    continue;

                descriptor.AddAttribute(attribute, property);
            }

            exportDefinitions.Add(visualType, descriptor);
        }

        private string SetField(ITechneVisual visual, Entry item)
        {
            if (item == null)
                return "";

            StringBuilder sb = new StringBuilder();

            sb.Append(visual.Name);
            sb.Append(".");
            sb.Append(item.Attribute.Name);
            sb.Append(" = ");

            var tmp = item.Property;

            if (tmp == null)
            {
                sb.Insert(0, "//");
            }
            else
            {
                sb.Append(TypeToJavaHelper.Convert(tmp.GetValue(visual, null), tmp.PropertyType, item.Attribute));
            }

            sb.Append(";");

            return sb.ToString();
        }

        private string CallMethod(ITechneVisual visual, List<Entry> item)
        {
            if (item == null || item.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();

            sb.Append(visual.Name);
            sb.Append(".");
            sb.Append(item[0].Attribute.Name);
            sb.Append("(");

            GenerateMethodArguments(visual, item, sb);

            sb.Append(");");

            return sb.ToString();
        }

        private string GenerateConstructor(ITechneVisual visual, MemberDescriptor descriptor)
        {
            List<Entry> entries = descriptor.GetDescription(typeof(JavaConstructorAttribute));

            if (entries == null || entries.Count == 0)
                return "";

            StringBuilder sb = new StringBuilder();

            sb.Append(visual.Name);
            sb.Append(" = new ");
            sb.Append(entries[0].Attribute.Name);
            sb.Append("(");

            GenerateMethodArguments(visual, entries, sb);

            sb.Append(");");

            return sb.ToString();
        }

        private static void GenerateMethodArguments(ITechneVisual visual, List<Entry> list, StringBuilder sb)
        {
            foreach (var item in list)
            {
                sb.Append(TypeToJavaHelper.Convert(item.Property.GetValue(visual, null), item.Property.PropertyType, item.Attribute));
                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
        }

        private static void CreateMethodArguments(ITechneVisual visual, MethodDefinition methodDefinition, StringBuilder sb)
        {
            foreach (var item in methodDefinition.PropertyExpressions)
            {
                var dependencyObject = (visual as DependencyObject);

                if (dependencyObject != null)
                {
                    var value = dependencyObject.GetValue(item);

                    sb.Append(TypeToJavaHelper.Convert(value, item.PropertyType, null));
                }
                else
                {
                    sb.Insert(0, "//");
                }

                sb.Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
        }

        private double ToRad(double p)
        {
            return p * Math.PI / 180;
        }

        public Guid Guid
        {
            get { return new Guid("1CD6C3E3-E9EA-419C-BC1B-3E12D48E554D"); }
        }

        public void OnLoad()
        {

        }

        public string DefaultExtension
        {
            get { return "java"; }
        }

        public string Filter
        {
            get { return "Java file (.java)|*.java"; }
        }

        public string MenuHeader
        {
            get { return "as Turbo Model Thingy java"; }
        }
    }
}

