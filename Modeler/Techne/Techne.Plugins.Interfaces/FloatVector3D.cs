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
using System.Globalization;

namespace Techne.Plugins
{
    public struct FloatVector3D
    {
        private float x;
        private float y;
        private float z;

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public FloatVector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(x.ToString(CultureInfo.InvariantCulture.NumberFormat));
            sb.Append(", ");
            sb.Append(y.ToString(CultureInfo.InvariantCulture.NumberFormat));
            sb.Append(", ");
            sb.Append(Z.ToString(CultureInfo.InvariantCulture.NumberFormat));
            return sb.ToString();
        }
    }

    public class FloatPoint3D
    {
        private float x;
        private float y;
        private float z;

        public float X
        {
            get { return x; }
            set { x = value; }
        }
        public float Y
        {
            get { return y; }
            set { y = value; }
        }
        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public FloatPoint3D()
        {
        }

        public FloatPoint3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(x.ToString(CultureInfo.InvariantCulture.NumberFormat));
            sb.Append(", ");
            sb.Append(y.ToString(CultureInfo.InvariantCulture.NumberFormat));
            sb.Append(", ");
            sb.Append(Z.ToString(CultureInfo.InvariantCulture.NumberFormat));
            return sb.ToString();
        }
    }
}

