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
using System.Windows.Media.Media3D;

namespace Techne.Plugins.Extensions
{
    public static class Vector3DExtensions
    {
        public static Point3D ToPoint3D(this Vector3D vector)
        {
            return new Point3D(vector.X, vector.Y, vector.Z);
        }
    }
}

