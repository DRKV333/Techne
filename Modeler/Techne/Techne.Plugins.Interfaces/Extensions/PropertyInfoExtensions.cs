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
using System.Reflection;

namespace Techne.Plugins.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static object CreateInstance(this PropertyInfo property)
        {
            return property.PropertyType.CreateInstance();
        }
    }
}

