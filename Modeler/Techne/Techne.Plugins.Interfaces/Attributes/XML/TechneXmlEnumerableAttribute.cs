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
    public class TechneXmlEnumerableAttribute : TechneXmlBaseAttribute
    {
        public string ParentName { get; set; }

        public TechneXmlEnumerableAttribute(String versionAdded, String parentName = "")
            : base(elementName: "", type: typeof(object), versionAdded: versionAdded, defaultValue: new object())
        {
            this.ParentName = parentName;
        }

        public TechneXmlEnumerableAttribute(String elementName, Type type, String versionAdded, object defaultValue)
            : base(elementName: elementName, type: type, versionAdded: versionAdded, defaultValue: defaultValue)
        {
        }
    }
}

