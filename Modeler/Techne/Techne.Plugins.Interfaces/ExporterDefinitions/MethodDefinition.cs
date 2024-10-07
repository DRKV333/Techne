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
using System.Linq.Expressions;
using System.Windows;

namespace Techne.Plugins
{
    public class MethodDefinition
    {
        public IEnumerable<DependencyProperty> PropertyExpressions { get; set; }
        public String Name { get; set; }
    }
}

