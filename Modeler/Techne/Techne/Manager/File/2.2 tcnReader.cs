/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using Techne.Models;
using Techne.Plugins.Attributes.XML;
using Techne.Plugins.Extensions;
using Techne.Plugins.Interfaces;
using Techne.Model;

namespace Techne.Manager
{
    internal class tcnReader
    {
        private Dictionary<string, IShapePlugin> shapes;

        public tcnReader(Dictionary<string, IShapePlugin> shapes = null)
        {
            this.shapes = shapes;
        }

        #region Deserializer
        internal TechneModel Deserialize(XElement tcnXml)
        {
            return (TechneModel)Deserialize(tcnXml, new TechneModel());
        }

        /// <summary>
        /// Parses a string with the correct typeconverter
        /// </summary>
        /// <param name="text">String to parse</param>
        /// <param name="type">Type to parse to</param>
        /// <returns>Instance of type <paramref name="type"/></returns>
        private object ParseString(string text, Type type)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(type);
            return converter.ConvertFromString(null, CultureInfo.InvariantCulture, text.Replace(';', ','));
        }

        private string ToString(object o)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(o);

            if (converter == null)
                return o.ToString();
            return converter.ConvertToInvariantString(o);
        }

        /// <summary>
        /// Deserializes an XElement to object
        /// </summary>
        /// <param name="xmlElement">XElement to deserialize</param>
        /// <param name="o">object to deserialize to</param>
        /// <returns><paramref name="o"/> with deserialized properties</returns>
        internal object Deserialize(XElement xmlElement, object o)
        {
            var properties = o.GetType().GetProperties();

            //we got all properties, now we begin to walk them
            foreach (var property in properties)
            {
                //why bother doing the rest since I can't change it anyway
                if (!property.CanWrite)
                    continue;

                //get attributes of property and the underlying type
                var attributes = Attribute.GetCustomAttributes(property, typeof(TechneXmlBaseAttribute), true).Cast<TechneXmlBaseAttribute>();
                var rootAttribute = Attribute.GetCustomAttributes(property.PropertyType, typeof(TechneXmlRootAttribute), true).FirstOrDefault() as TechneXmlRootAttribute;

                foreach (var attribute in attributes)
                {
                    string elementName = GetXmlElementName(property, attribute);

                    //I know this is not really nice, but it's clearer that way
                    var value = GetValueOrDefault(o, attribute, rootAttribute, property, xmlElement, elementName);

                    if (value != null)
                    {
                        if (attribute is TechneXmlElementAttribute)
                        {
                            HandleXmlElementAttribute(xmlElement, o, property, rootAttribute, attribute, (XElement)value);
                        }
                        else if (attribute is TechneXmlAttributeAttribute)
                        {
                            HandleXmlAttributeAttribute(xmlElement, o, property, attribute, (XAttribute)value);
                        }
                        else if (attribute is TechneXmlEnumerableAttribute)
                        {
                            HandleXmlEnumerableAttribute(xmlElement, o, property, attribute, (XElement)value);
                        }
                        else
                        {
                        }
                    }
                }
            }

            return o;
        }

        /// <summary>
        /// Gets an XObject with the value, or if XObject was null asigns default value to property
        /// </summary>
        /// <param name="xmlElement">XElement with the content</param>
        /// <param name="o">Object to deserialize to</param>
        /// <param name="property">Property that gets deserialized</param>
        /// <param name="rootAttribute">Attribute of underlying class</param>
        /// <param name="attribute">Property attribute</param>
        /// <param name="elementName">XElement name</param>
        /// <returns></returns>
        private XObject GetValueOrDefault(Object o, TechneXmlBaseAttribute attribute, TechneXmlRootAttribute rootAttribute, PropertyInfo property, XElement xmlElement, string elementName)
        {
            XObject value = null;
            if (attribute is TechneXmlAttributeAttribute)
                value = xmlElement.Attribute(elementName);
            else if (attribute is TechneXmlEnumerableAttribute && String.IsNullOrEmpty(((TechneXmlEnumerableAttribute)attribute).ParentName))
                value = xmlElement;
            else
                value = xmlElement.Element(elementName);

            if (value == null)
            {
                if (attribute.DefaultValue == null)
                    return null;

                if (attribute.DefaultValue.GetType().Equals(property.PropertyType))
                    property.SetValue(o, attribute.DefaultValue, null);
                else if (rootAttribute != null)
                    property.SetValue(o, property.CreateInstance(), null);
            }


            return value;
        }

        /// <summary>
        /// Parses xml defined through a TechneXmlElementAttibute
        /// </summary>
        /// <param name="xmlElement">XElement with the content</param>
        /// <param name="o">Object to deserialize to</param>
        /// <param name="property">Property that gets deserialized</param>
        /// <param name="rootAttribute">Attribute of underlying class</param>
        /// <param name="attribute">Property attribute</param>
        /// <param name="elementName">XElement name</param>
        private void HandleXmlElementAttribute(XElement xmlElement, object o, PropertyInfo property, TechneXmlRootAttribute rootAttribute, TechneXmlBaseAttribute attribute, XElement value)
        {
            if (rootAttribute == null)
                property.SetValue(o, ParseString(value.Value, property.PropertyType), null);
            else
                property.SetValue(o, Deserialize(value, property.CreateInstance()), null);
        }

        private void HandleXmlAttributeAttribute(XElement xmlElement, object o, PropertyInfo property, TechneXmlBaseAttribute attribute, XAttribute value)
        {
            //If I can represent a custom object in a string I can write a typeconverter that gets it back
            property.SetValue(o, ParseString(value.Value, property.PropertyType), null);
        }

        private void HandleXmlEnumerableAttribute(XElement xmlElement, object o, PropertyInfo property, TechneXmlBaseAttribute attribute, XElement value)
        {
            var propertyType = property.PropertyType;

            if (o is ITechneVisualCollection)
            {
                var addMethod = o.GetType().GetMethod("AddChild");
                HandleEnumerableList(o, property, value, propertyType, o, addMethod);
            }
            else
            {
                var collection = propertyType.CreateInstance();
                
                var addMethod = collection.GetType().GetMethod("Add");
                var addMethodParameters = addMethod.GetParameters();

                //I don't care if it's a List, Collection, anything - if it has one parameter I'll pass it one, if it's two I'll pass a key and a value
                //if it has neither, well, screw you
                if (addMethodParameters.Length == 1)
                {
                    {
                        HandleEnumerableList(o, property, value, propertyType, collection, addMethod);
                    }
                }
                else if (addMethodParameters.Length == 2)
                {
                    {
                        HandleEnumerableDictionary(propertyType);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Add method parameter is neither 1 nor 2");
                    //booboo
                }
            }
        }

        private static void HandleEnumerableDictionary(Type propertyType)
        {
            var keyType = propertyType.GetGenericArguments().First();
            var subType = propertyType.GetGenericArguments().Last();

            //Fuck you future self, have fun figuring this method out.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a List of T, deserializes its content and asigns it to <paramref name="property"/>
        /// </summary>
        /// <param name="o">The original object whose property we want to set</param>
        /// <param name="property">The property to set</param>
        /// <param name="value">XElement with the content</param>
        /// <param name="propertyType">Type of the property</param>
        /// <param name="collection">Collection to fill</param>
        /// <param name="addMethod">Add-method with one parameter</param>
        private void HandleEnumerableList(object o, PropertyInfo property, XElement value, Type propertyType, object collection, MethodInfo addMethod)
        {
            var subType = propertyType.GetGenericArguments().First();

            if (subType == typeof(ITechneVisual))
            {
                var washit = false;
                //maybe make that being fetched from the interface
                foreach (var item in value.Elements("Shape"))
                {
                    var type = item.Attribute("type").Value;

                    var instance = ExtensionManager.GetShape(type);
                    instance = (ITechneVisual)Deserialize(item, instance);
                    addMethod.Invoke(collection, new[] { instance });

                    washit = true;
                }

                foreach (var item in value.Elements("Folder"))
                {
                    var instance = new TechneVisualFolder();
                    instance = (TechneVisualFolder)Deserialize(item, instance);
                    addMethod.Invoke(collection, new[] { instance });

                    washit = true;
                }

                foreach (var item in value.Elements("Piece"))
                {
                    var instance = new TechneVisualCollection();
                    instance = (TechneVisualCollection)Deserialize(item, instance);
                    addMethod.Invoke(collection, new[] { instance });

                    washit = true;
                }
            }
            else
            {
                var subTypeRootAttribute = (TechneXmlRootAttribute)subType.GetCustomAttributes(typeof(TechneXmlRootAttribute), true).FirstOrDefault();

                //if (subTypeRootAttribute == null)
                //    return;

                foreach (var item in value.Elements(subTypeRootAttribute.ElementName))
                {
                    var instance = subType.CreateInstance();
                    instance = Deserialize(item, instance);
                    addMethod.Invoke(collection, new[] {instance});
                }
            }

            if (!o.Equals(collection))
                property.SetValue(o, collection, null);
        }
        #endregion

        #region Serialize
        internal XElement Serialize(TechneModel techneModels)
        {
            return Serialize((object)techneModels);
        }

        internal XElement Serialize(object o, string rootElementName)
        {
            var properties = o.GetType().GetProperties().OrderBy(x => x.Name);
            XElement result = new XElement(rootElementName);

            foreach (var property in properties)
            {
                var attributes = Attribute.GetCustomAttributes(property, typeof(TechneXmlBaseAttribute), true).Cast<TechneXmlBaseAttribute>();
                var rootAttribute = Attribute.GetCustomAttributes(property.PropertyType, typeof(TechneXmlRootAttribute), true).FirstOrDefault() as TechneXmlRootAttribute;

                foreach (var attribute in attributes)
                {
                    string elementName = GetXmlElementName(property, attribute);
                    Type type = GetXmlElementType(property, attribute);

                    var value = property.GetValue(o, null);

                    if (attribute is TechneXmlElementAttribute)
                    {
                        SerializeElement(result, property, rootAttribute, elementName, value);
                    }
                    else if (attribute is TechneXmlAttributeAttribute)
                    {
                        SerializeAttribute(result, elementName, value);
                    }
                    else if (attribute is TechneXmlEnumerableAttribute)
                    {
                        SerializeEnumerable(result, attribute, value);
                    }
                    else
                    {
                    }
                }
            }
            return result;
        }

        private void SerializeAttribute(XElement result, string elementName, object value)
        {
            string content;

            if (value == null)
                content = "";
            else
                content = this.ToString(value);
            result.Add(new XAttribute(elementName, content));
        }

        private void SerializeElement(XElement result, PropertyInfo property, TechneXmlRootAttribute rootAttribute, string elementName, object value)
        {
            string content;

            if (value == null)
                content = "";
            else
                content = this.ToString(value);

            if (rootAttribute == null)
                result.Add(new XElement(elementName, content));
            else
                result.Add(Serialize(value, rootAttribute.ElementName));
        }

        private void SerializeEnumerable(XElement result, TechneXmlBaseAttribute attribute, object value)
        {
            //var values = item.GetGetMethod().Invoke(o, null);
            var models = value as IEnumerable;

            if (models != null)
            {
                var enumerableAttribute = attribute as TechneXmlEnumerableAttribute;

                if (String.IsNullOrEmpty(enumerableAttribute.ParentName))
                    foreach (var model in models)
                    {
                        result.Add(Serialize(model));
                    }
                else
                    result.Add(Serialize(models, enumerableAttribute.ParentName));
            }
        }

        internal XElement Serialize(object o)
        {
            var rootAttribute = o.GetType().GetCustomAttributes(typeof(TechneXmlRootAttribute), true).FirstOrDefault() as TechneXmlRootAttribute;
            if (rootAttribute == null)
                return new XElement("Error");

            return Serialize(o, rootAttribute.ElementName);
        }

        private XElement Serialize(IEnumerable enumerable, String elemenName)
        {
            var result = new XElement(elemenName);

            foreach (var item in enumerable)
            {
                result.Add(Serialize(item));
            }

            return result;
        }

        private XElement[] Serialize(IEnumerable enumerable)
        {
            var results = new List<XElement>();

            foreach (var item in enumerable)
            {
                results.Add(Serialize(item));
            }

            return results.ToArray();
        }
        #endregion

        private Type GetXmlElementType(PropertyInfo item, TechneXmlBaseAttribute attribute)
        {
            //if (attribute.Type == null)
            return item.PropertyType;

            return attribute.Type;
        }

        private string GetXmlElementName(PropertyInfo item, TechneXmlBaseAttribute attribute)
        {
            if (attribute.ElementName == null || String.IsNullOrEmpty(attribute.ElementName))
                return item.Name;

            return attribute.ElementName;
        }
    }
}

