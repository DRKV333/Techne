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

namespace Techne.Plugins.Attributes.XML
{
    [AttributeUsageAttribute(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public class TechneXmlBaseAttribute : Attribute
    {
        private object defaultValue;
        private String versionAdded;
        private Type type;
        private string elementName;

        public object DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; }
        }

        public Type Type
        {
            get { return type; }
            set { type = value; }
        }

        public String VersionAdded
        {
            get { return versionAdded; }
            set { versionAdded = value; }
        }

        public string ElementName
        {
            get { return elementName; }
            set { elementName = value; }
        }

        public TechneXmlBaseAttribute(String elementName, Type type, String versionAdded, object defaultValue)
        {
            this.elementName = elementName;
            this.type = type;
            this.versionAdded = versionAdded;
            this.defaultValue = defaultValue;
        }
    }
}

