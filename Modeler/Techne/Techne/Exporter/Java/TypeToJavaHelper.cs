/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;
using Techne.Plugins.Attributes;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    internal static class TypeToJavaHelper
    {
        private static readonly TypeConverter converter = new TypeConverter();
        private static readonly Dictionary<Type, MethodInfo> javaConverters = new Dictionary<Type, MethodInfo>();

        static TypeToJavaHelper()
        {
            var t = typeof(TypeConverter).GetMethods();

            foreach (var item in t)
            {
                var attributes = item.GetCustomAttributes(typeof(TypeDescriptionAttribute), true);

                if (attributes == null || attributes.Length == 0)
                    continue;

                var attribute = attributes[0] as TypeDescriptionAttribute;

                if (attribute == null)
                    continue;

                javaConverters.Add(attribute.Type, item);
            }
        }

        internal static string Convert(object value, Type type, JavaExporterDescriptionBase propertyAttribute)
        {
            if (value == null)
                return "";

            if (javaConverters.ContainsKey(type))
            {
                var t = typeof(TypeConverter).GetMethods();

                if (propertyAttribute != null && propertyAttribute.ValueConverter != null)
                {
                    foreach (var valueConverter in propertyAttribute.ValueConverter)
                    {
                        value = (ValueConverterHelper.GetConverter(valueConverter) ?? new NullConverter()).Convert(value, null, null, CultureInfo.CurrentCulture);
                    }
                }

                if (javaConverters.ContainsKey(value.GetType()))
                {
                    return javaConverters[value.GetType()].Invoke(converter, new[] {value}).ToString();
                }
            }

            return value.ToString().Replace(',', '.');
        }

        private static string ToSeperatedString(this string[] ie)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in ie)
            {
                sb.Append(item);
            }

            return sb.ToString();
        }

        private static string ToSeperatedString(this string[] ie, char separator)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in ie)
            {
                sb.Append(item);
                sb.Append(separator);
            }

            return sb.ToString().TrimEnd(separator);
        }

        private static string ToSeperatedString(this string[] ie, string separator)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in ie)
            {
                sb.Append(item);
                sb.Append(separator);
            }

            return sb.ToString().TrimEnd(separator.ToCharArray());
        }

        #region Nested type: NullConverter
        private class NullConverter : IValueConverter
        {
            #region IValueConverter Members
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return value;
            }
            #endregion
        }
        #endregion

        #region Nested type: TypeConverter
        private class TypeConverter
        {
            [TypeDescriptionAttribute(typeof(Int32))]
            public string Int32ToJava(int i)
            {
                return i.ToString();
            }

            [TypeDescriptionAttribute(typeof(double))]
            public string DoubleToJava(double d)
            {
                return d.ToString(CultureInfo.InvariantCulture.NumberFormat);
            }

            [TypeDescriptionAttribute(typeof(float))]
            public string FloatToJava(float f)
            {
                return (new[]
                        {
                            f.ToString(CultureInfo.InvariantCulture.NumberFormat),
                            "F"
                        }).ToSeperatedString();
            }

            [TypeDescriptionAttribute(typeof(Boolean))]
            public string BooleanToJava(Boolean value)
            {
                if (value)
                    return "true";
                else
                    return "false";
            }

            [TypeDescriptionAttribute(typeof(Vector))]
            public string VectorToJava(Vector vector)
            {
                return new[]
                       {
                           vector.X.ToString().Replace(',', '.'),
                           vector.Y.ToString().Replace(',', '.')
                       }.ToSeperatedString(", ");
            }

            [TypeDescriptionAttribute(typeof(Vector3D))]
            public string Vector3DToJava(Vector3D vector)
            {
                return new[]
                       {
                           vector.X.ToString().Replace(',', '.'),
                           vector.Y.ToString().Replace(',', '.'),
                           vector.Z.ToString().Replace(',', '.')
                       }.ToSeperatedString(", ");
            }

            [TypeDescriptionAttribute(typeof(Point))]
            public string PointToJava(Point vector)
            {
                return new[]
                       {
                           vector.X.ToString().Replace(',', '.'),
                           vector.Y.ToString().Replace(',', '.')
                       }.ToSeperatedString(", ");
            }

            [TypeDescriptionAttribute(typeof(Point3D))]
            public string Point3DToJava(Point3D vector)
            {
                return new[]
                       {
                           vector.X.ToString().Replace(',', '.'),
                           vector.Y.ToString().Replace(',', '.'),
                           vector.Z.ToString().Replace(',', '.')
                       }.ToSeperatedString(", ");
            }

            [TypeDescriptionAttribute(typeof(FloatVector3D))]
            public string FloatVector3DToJava(FloatVector3D vector)
            {
                return new[]
                       {
                           Convert(vector.X, vector.X.GetType(), null),
                           Convert(vector.Y, vector.Y.GetType(), null),
                           Convert(vector.Z, vector.Z.GetType(), null),
                       }.ToSeperatedString(", ");
                //return new string[]
                //{
                //    vector.X.ToString().Replace(',', '.') + "F",
                //    vector.Y.ToString().Replace(',', '.') + "F",
                //    vector.Z.ToString().Replace(',', '.') + "F"
                //}.ToSeperatedString(", ");
            }

            [TypeDescriptionAttribute(typeof(FloatPoint3D))]
            public string FloatPoint3DToJava(FloatPoint3D vector)
            {
                return new[]
                       {
                           vector.X.ToString().Replace(',', '.') + "F",
                           vector.Y.ToString().Replace(',', '.') + "F",
                           vector.Z.ToString().Replace(',', '.') + "F"
                       }.ToSeperatedString(", ");
            }
        }
        #endregion

        #region Nested type: TypeDescriptionAttribute
        [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
        private sealed class TypeDescriptionAttribute : Attribute
        {
            public TypeDescriptionAttribute(Type type)
            {
                Type = type;
            }

            public Type Type
            {
                get;
                private set;
            }
        }
        #endregion
    }
}

