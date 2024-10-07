/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows.Media.Media3D;

namespace Techne.Helper
{
    internal class EditableVector3D
    {
        private Vector3D vector;

        private EditableVector3D(Vector3D vector)
        {
            this.vector = vector;
        }

        public double X
        {
            get { return vector.X; }
            set { }
        }

        public double Y
        {
            get { return vector.Y; }
            set { }
        }

        public double Z
        {
            get { return vector.Z; }
            set { }
        }
    }
}

