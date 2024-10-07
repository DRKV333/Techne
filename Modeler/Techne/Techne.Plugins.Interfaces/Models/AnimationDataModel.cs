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
using Techne.Plugins.Attributes.XML;

namespace Techne.Plugins.Models
{
    [TechneXmlRoot("Animation")]
    public class AnimationDataModel
    {
        [TechneXmlElement("2.1")]
        public Vector3D AnimationAngles
        {
            get;
            set;
        }

        [TechneXmlElement("2.1")]
        public Vector3D AnimationDuration
        {
            get;
            set;
        }

        [TechneXmlElement("2.1")]
        public Vector3D AnimationType
        {
            get;
            set;
        }
    }
}

