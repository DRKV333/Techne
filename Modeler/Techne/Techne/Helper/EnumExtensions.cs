/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows.Media.Media3D;
using Techne.Models.Enums;

namespace Techne
{
    public static class EnumExtensions
    {
        public static Vector3D GetAxisNormal(this EnumAxis enumAxis)
        {
            switch (enumAxis)
            {
                case EnumAxis.X:
                    return new Vector3D(0, 0, 1);
                    break;
                case EnumAxis.Y:
                    return new Vector3D(1, 0, 0);
                    break;
                case EnumAxis.Z:
                    return new Vector3D(0, 1, 0);
                    break;
                default:
                    return new Vector3D(0, 0, 0);
                    break;
            }
        }

        public static Vector3D GetAxisVector(this EnumAxis enumAxis)
        {
            switch (enumAxis)
            {
                case EnumAxis.X:
                    return new Vector3D(1, 0, 0);
                    break;
                case EnumAxis.Y:
                    return new Vector3D(0, 1, 0);
                    break;
                case EnumAxis.Z:
                    return new Vector3D(0, 0, 1);
                    break;
                default:
                    return new Vector3D(0, 0, 0);
                    break;
            }
        }
    }
}

