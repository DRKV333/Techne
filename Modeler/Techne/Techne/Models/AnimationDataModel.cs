/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using Techne.Plugins;
using Techne.Plugins.Interfaces;

namespace Techne.Models
{
    public class AnimationDataModel
    {
        public AnimationDataModel(ITechneVisual SelectedVisual, int axis)
        {
            if (SelectedVisual != null)
            {
                switch (axis)
                {
                    case 0:
                        Angle = SelectedVisual.AnimationAngles.X;
                        Duration = SelectedVisual.AnimationDuration.X;
                        AnimationType = (AnimationType)SelectedVisual.AnimationType.X;
                        break;
                    case 1:
                        Angle = SelectedVisual.AnimationAngles.Y;
                        Duration = SelectedVisual.AnimationDuration.Y;
                        AnimationType = (AnimationType)SelectedVisual.AnimationType.Y;
                        break;
                    case 2:
                        Angle = SelectedVisual.AnimationAngles.Z;
                        Duration = SelectedVisual.AnimationDuration.Z;
                        AnimationType = (AnimationType)SelectedVisual.AnimationType.Z;
                        break;
                    default:
                        break;
                }
            }
        }

        public double Angle
        {
            get;
            set;
        }

        public double Duration
        {
            get;
            set;
        }

        public AnimationType AnimationType
        {
            get;
            set;
        }
    }
}

