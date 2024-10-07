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

namespace Techne.Plugins.Extensions
{
    public static class TypeExtensions
    {
        public static object CreateInstance(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes).Invoke(new object[0]);
        }
    }
}

