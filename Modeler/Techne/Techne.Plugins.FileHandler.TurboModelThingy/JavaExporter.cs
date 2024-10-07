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
using Techne.Models;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    [PluginExportAttribute("Java Exporter", "0.5", "ZeuX", "Allows models to be exported as java", "1CD6C3E3-E9EA-419C-BC1B-3E12D48E554D")]
    [Export(typeof(IExportPlugin))]
    public class JavaExporter : IExportPlugin
    {
        private Dictionary<string, MemberDescriptor> exportDefinitions = new Dictionary<string, MemberDescriptor>();

        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel saveModel)
        {
            Export(filename, visuals, shapes, saveModel, 0);
        }

        public void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel saveModel, int i)
        {
            var model = saveModel.Models.FirstOrDefault();
            var baseClass = model == null ? "ModelBase" : String.IsNullOrEmpty(model.BaseClass) ? "ModelBase" : model.BaseClass;

            IndendingStringBuilder constructorContents = new IndendingStringBuilder();
            IndendingStringBuilder renderContents = new IndendingStringBuilder();
            IndendingStringBuilder fieldDefinitions = new IndendingStringBuilder();
            IndendingStringBuilder animationCode = new IndendingStringBuilder();

            CreateContents(visuals, constructorContents, renderContents, fieldDefinitions, animationCode);

            IndendingStringBuilder javaClass = new IndendingStringBuilder();

            javaClass.AppendLine("package net.minecraft.src;");
            javaClass.AppendLine("//Exported java file");
            javaClass.AppendLine("//Keep in mind that you still need to fill in some blanks");
            javaClass.AppendLine("// - ZeuX");
            javaClass.AppendLine();

            javaClass.Append("public class ");
            javaClass.Append(String.IsNullOrEmpty(saveModel.Name) ? "Model" : saveModel.Name.Replace(' ', '_'));
            javaClass.Append(" extends ");
            javaClass.AppendLine(baseClass);

            javaClass.AppendLine("{");
            javaClass.IncreaseIndent();

            javaClass.Append("public ");
            javaClass.Append((String.IsNullOrEmpty(saveModel.Name) ? "Model" : saveModel.Name.Replace(' ', '_')).Trim());
            javaClass.AppendLine("()");

            javaClass.AppendLine("{");
            javaClass.IncreaseIndent();

            if (!baseClass.Equals("ModelBase"))
            {
                javaClass.AppendLine("super();");
            }

            javaClass.AppendLines(constructorContents);

            javaClass.DecreaseIndent();
            javaClass.AppendLine("}");

            javaClass.AppendLine();

            javaClass.AppendLine("public void render(Entity entity, float f, float f1, float f2, float f3, float f4, float f5)");
            javaClass.AppendLine("{");
            javaClass.IncreaseIndent();

            javaClass.AppendLine("super.render(entity, f, f1, f2, f3, f4, f5);");
            javaClass.AppendLine("setRotationAngles(f, f1, f2, f3, f4, f5);");
            javaClass.AppendLines(renderContents);

            javaClass.DecreaseIndent();
            javaClass.AppendLine("}");

            javaClass.AppendLine();

            javaClass.AppendLine("public void setRotationAngles(float f, float f1, float f2, float f3, float f4, float f5)");
            javaClass.AppendLine("{");
            javaClass.IncreaseIndent();

            javaClass.AppendLine("super.setRotationAngles(f, f1, f2, f3, f4, f5);");
            javaClass.AppendLines(animationCode);

            javaClass.DecreaseIndent();
            javaClass.AppendLine("}");

            javaClass.AppendLine();

            javaClass.AppendLine("//fields");
            javaClass.AppendLines(fieldDefinitions);

            javaClass.DecreaseIndent();
            javaClass.AppendLine("}");

            File.WriteAllText(filename, javaClass.ToString());
        }

        private void CreateContents(IEnumerable<ITechneVisual> visuals, IndendingStringBuilder constructorContents, IndendingStringBuilder renderContents, IndendingStringBuilder fieldDefinitions, IndendingStringBuilder animationCode)
        {
            foreach (var visual in visuals)
            {
                if (visual.Guid.ToString().Equals("9EC93754-B48D-4C70-AE4E-84D81AE55396", StringComparison.InvariantCultureIgnoreCase))
                {
                    CreateContents((visual as System.Windows.Media.Media3D.ModelVisual3D).Children.Cast<ITechneVisual>(), constructorContents, renderContents, fieldDefinitions, animationCode);
                }
                else
                {
                    string originalName = visual.Name ?? "";

                    visual.Name = originalName.Replace(' ', '_');

                    visual.Name = String.IsNullOrEmpty(visual.Name) || String.IsNullOrWhiteSpace(visual.Name) ? "I_NEED_A_NAME" : visual.Name;

                    CreateExportDefinition(visual);

                    CreateVisualConstructorContents(constructorContents, visual, fieldDefinitions);
                    CreateVisualRenderContents(renderContents, visual);

                    CreateAnimationCode(visual, animationCode);

                    visual.Name = originalName;
                }
            }
        }

        private void CreateAnimationCode(ITechneVisual visual, IndendingStringBuilder animationCode)
        {
            if (visual.AnimationAngles.X != 0 && visual.AnimationDuration.X > 0)
            {
                animationCode.Append(visual.Name);
                animationCode.Append(".rotateAngleX = ");
                animationCode.Append("MathHelper.cos(f / (1.919107651F * ");
                animationCode.Append(visual.AnimationDuration.X.ToString().Replace(',', '.'));
                animationCode.Append(") * ");
                animationCode.Append(ToRad(visual.AnimationAngles.X).ToString().Replace(',', '.'));
                animationCode.Append(" * f");
                animationCode.Append(((int)visual.AnimationType[0] + 1).ToString());
                animationCode.Append(" + ");
                animationCode.Append(ToRad(visual.RotationX).ToString().Remove(',', '.'));
                animationCode.Append(";");
            }

            if (visual.AnimationAngles.Y != 0 && visual.AnimationDuration.Y > 0)
            {
                animationCode.Append(visual.Name);
                animationCode.Append(".rotateAngleY = ");
                animationCode.Append("MathHelper.cos(f / (1.919107651F * ");
                animationCode.Append(visual.AnimationDuration.Y.ToString().Replace(',', '.'));
                animationCode.Append(") * ");
                animationCode.Append(ToRad(visual.AnimationAngles.Y).ToString().Replace(',', '.'));
                animationCode.Append(" * f");
                animationCode.Append(((int)visual.AnimationType[1] + 1).ToString());
                animationCode.Append(" + ");
                animationCode.Append(ToRad(visual.RotationY).ToString().Remove(',', '.'));
                animationCode.Append(";");
            }

            if (visual.AnimationAngles.Z != 0 && visual.AnimationDuration.Z > 0)
            {
                animationCode.Append(visual.Name);
                animationCode.Append(".rotateAngleZ = ");
                animationCode.Append("MathHelper.cos(f / (1.919107651F * ");
                animationCode.Append(visual.AnimationDuration.Z.ToString().Replace(',', '.'));
                animationCode.Append(")) * ");
                animationCode.Append(ToRad(visual.AnimationAngles.Z).ToString().Replace(',', '.'));
                animationCode.Append(" * f");
                animationCode.Append(((int)visual.AnimationType[2] + 1).ToString());
                animationCode.Append(" + ");
                animationCode.Append(ToRad(visual.RotationZ).ToString().Remove(',', '.'));
                animationCode.Append(";");
            }
        }

        private void CreateVisualRenderContents(IndendingStringBuilder renderContents, ITechneVisual visual)
        {
            if (!visual.IsDecorative)
            {
                renderContents.Append(visual.Name);
                renderContents.AppendLine(".render(f5);");
            }
        }

        private void CreateVisualConstructorContents(IndendingStringBuilder constructorContents, ITechneVisual visual, IndendingStringBuilder fieldDefinitions)
        {
            //appending constructor
            if (!visual.IsDecorative)
                constructorContents.AppendLine(GenerateConstructor(visual, exportDefinitions[visual.GetType().ToString()]));

            //appending method calls
            foreach (var item in exportDefinitions[visual.GetType().ToString()].MethodEntries)
            {
                constructorContents.AppendLine(CallMethod(visual, item));
            }
            constructorContents.AppendLine();

            foreach (var item in exportDefinitions[visual.GetType().ToString()].FieldEntries)
            {
                constructorContents.AppendLine(SetField(visual, item.FirstOrDefault()));
            }
            constructorContents.AppendLine();

            //todo:use that for GenerateConstructor!
            if (!visual.IsDecorative)
            {
                var cosntructor = exportDefinitions[visual.GetType().ToString()].ConstructorEntry;
                fieldDefinitions.Append("public ");
                fieldDefinitions.Append(cosntructor[0].Attribute.Name);
                fieldDefinitions.Append(" ");
                fieldDefinitions.Append(visual.Name);
                fieldDefinitions.AppendLine(";");
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

                JavaExporterDescriptionBase attribute = attributes.FirstOrDefault();

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
            foreach (var item in list.OrderBy(x => x.Attribute.Position))
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
            get { return "as java"; }
        }
    }
}

