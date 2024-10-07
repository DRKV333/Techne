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

namespace Techne.Plugins
{
    public class JavaRendererDefinition
    {
        public MethodDefinition ModelRendererConstructor { get; set; }
        public MethodDefinition ShapeAddMethod { get; set; }
        public MethodDefinition PositionMethod { get; set; }
        public MethodDefinition OffsetMethod { get; set; }

        public FieldDefinition RotationX { get; set; }
        public FieldDefinition RotationY { get; set; }
        public FieldDefinition RotationZ { get; set; }
    }
}

