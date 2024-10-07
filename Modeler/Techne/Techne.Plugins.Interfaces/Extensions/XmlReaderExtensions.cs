/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows.Media.Media3D;
using System.Xml;
using System.Windows;

namespace Techne
{
    public static class XmlReaderExtensions
    {
        public static Vector3D ReadContentAsVector3D( this XmlReader reader)
        {
            return Vector3D.Parse(reader.ReadContentAsString());
        }

        public static Vector ReadContentAsVector( this XmlReader reader)
        {
            return Vector.Parse(reader.ReadContentAsString());
        }
    }
}

