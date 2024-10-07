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
using System.Xml.Serialization;

namespace Techne.Plugins.Attributes.XML
{
    [AttributeUsageAttribute(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class TechneXmlRootAttribute : XmlRootAttribute
    {
        public TechneXmlRootAttribute(String elementName)
            : base (elementName)
        {
        }
    }
}

