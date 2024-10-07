/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Techne.Plugins.Interfaces;
using System.Windows.Media.Media3D;
using System.Windows;

namespace Techne.Models
{
    public class SelectedVisualModel
    {
        readonly IList<ITechneVisual> selectedVisuals;

        private Double width;
        private Double height;
        private Double length;
        private Boolean isMirrored;
        private Double offsetX;
        private Double offsetY;
        private Double offsetZ;
        private Double positionX;
        private Double positionY;
        private Double positionZ;
        private Double textureOffsetX;
        private Double textureOffsetY;
        private Double rotationX;
        private Double rotationY;
        private Double rotationZ;

        public IList<ITechneVisual> SelectedVisual
        {
            get
            {
                return new List<ITechneVisual>(selectedVisuals);
            }
        }

        public Double Width
        {
            get
            {
                return width;
            }
            set
            {
                if (value < 0)
                    return;

                var delta = value - width;

                foreach (var item in selectedVisuals)
                {
                    item.Width += delta;
                }

                width = selectedVisuals.Max(x => x.Width);
            }
        }

        public Double Height
        {
            get
            {
                return height;
            }
            set
            {
                if (value < 0)
                    return;

                var delta = value - height;

                foreach (var item in selectedVisuals)
                {
                    item.Height += delta;
                }

                height = selectedVisuals.Max(x => x.Height);
            }
        }
        public Double Length
        {
            get
            {
                return length;
            }
            set
            {
                if (value < 0)
                    return;

                var delta = value - length;

                foreach (var item in selectedVisuals)
                {
                    item.Length += delta;
                }

                length = selectedVisuals.Max(x => x.Length);
            }
        }

        public Boolean IsMirrored
        {
            get
            {
                return isMirrored;
            }
            set
            {
                foreach (var item in selectedVisuals)
                    item.IsMirrored = value;

                if (selectedVisuals.Where(x => !x.IsMirrored).Count() == selectedVisuals.Count)
                    isMirrored = false;
                else if (selectedVisuals.Where(x => x.IsMirrored).Count() == selectedVisuals.Count)
                    isMirrored = true;
                else
                    isMirrored = false;
            }
        }

        public Double OffsetX
        {
            get
            {
                return offsetX;
            }
            set
            {
                if (selectedVisuals.Count == 0)
                    return;

                var delta = value - offsetX;

                foreach (var item in selectedVisuals)
                {
                    item.Offset = new Vector3D(item.Offset.X + delta, item.Offset.Y, item.Offset.Z);
                }

                offsetX = selectedVisuals.Max(x => x.Offset.X);
            }
        }

        public Double OffsetY
        {
            get 
            {
                return offsetY;
            }
            set
            {
                if (selectedVisuals.Count == 0)
                    return;

                var delta = value - offsetY;

                foreach (var item in selectedVisuals)
                {
                    item.Offset = new Vector3D(item.Offset.X, item.Offset.Y + delta, item.Offset.Z);
                }

                offsetY = selectedVisuals.Max(x => x.Offset.Y);
            }
        }

        public Double OffsetZ
        {
            get
            {
                return offsetZ;
            }
            set
            {
                if (selectedVisuals.Count == 0)
                    return;

                var delta = value - offsetZ;

                foreach (var item in selectedVisuals)
                {
                    item.Offset = new Vector3D(item.Offset.X, item.Offset.Y, item.Offset.Z + delta);
                }

                offsetZ = selectedVisuals.Max(x => x.Offset.Z);
            }
        }

        public Double PositionX
        {
            get 
            {
                return positionX;
            }
            set
            {
                if (selectedVisuals.Count == 0)
                    return;

                var delta = value - positionX;

                foreach (var item in selectedVisuals)
                {
                    item.Position = new Vector3D(item.Position.X + delta, item.Position.Y, item.Position.Z);
                }

                positionX = selectedVisuals.Max(x => x.Position.X);
            }
        }

        public Double PositionY
        {
            get 
            {
                return positionY;
            }
            set
            {
                if (selectedVisuals.Count == 0)
                    return;

                var delta = value - positionY;

                foreach (var item in selectedVisuals)
                {
                    item.Position = new Vector3D(item.Position.X, item.Position.Y + delta, item.Position.Z);
                }

                positionY = selectedVisuals.Max(x => x.Position.Y);
            }
        }

        public Double PositionZ
        {
            get 
            {
                return positionZ;
            }
            set
            {
                if (selectedVisuals.Count == 0)
                    return;

                var delta = value - positionZ;

                foreach (var item in selectedVisuals)
                {
                    item.Position = new Vector3D(item.Position.X, item.Position.Y, item.Position.Z + delta);
                }

                positionZ = selectedVisuals.Max(x => x.Position.Z);
            }
        }

        public Double TextureOffsetX
        {
            get
            {
                return textureOffsetX;
            }
            set
            {
                var delta = value - textureOffsetX;

                foreach (var item in selectedVisuals)
                {
                    item.TextureOffset = new Vector(item.TextureOffset.X + delta, item.TextureOffset.Y);
                }

                textureOffsetX = selectedVisuals.Max(x => x.TextureOffset.X);
            }
        }

        public Double TextureOffsetY
        {
            get
            {
                return textureOffsetY;
            }
            set
            {
                var delta = value - textureOffsetY;

                foreach (var item in selectedVisuals)
                {
                    item.TextureOffset = new Vector(item.TextureOffset.X, item.TextureOffset.Y + delta);
                }

                textureOffsetY = selectedVisuals.Max(x => x.TextureOffset.Y); 
            }
        }

        public String ElementName
        {
            get
            {
                if (selectedVisuals.Count > 1)
                {
                    //room for returning common string
                    return "selection";
                }
                if (selectedVisuals.Count == 1)
                    return selectedVisuals[0].Name;
                return "";
            }
            set
            {
                if (selectedVisuals.Count == 1)
                {
                    selectedVisuals[0].Name = value;
                }
            }
        }

        public Double RotationX
        {
            get
            {
                return rotationX;
            }
            set
            {
                var delta = value - rotationX;

                foreach (var item in selectedVisuals)
                {
                    item.RotationX += delta;
                }

                rotationX = selectedVisuals.Max(x => x.RotationX); 
            }
        }

        public Double RotationY
        {
            get
            {
                return rotationY;
            }
            set
            {
                var delta = value - rotationY;

                foreach (var item in selectedVisuals)
                {
                    item.RotationY += delta;
                }

                rotationY = selectedVisuals.Max(x => x.RotationY); 
            }
        }

        public Double RotationZ
        {
            get
            {
                return rotationZ;
            }
            set
            {
                var delta = value - rotationZ;

                foreach (var item in selectedVisuals)
                {
                    item.RotationZ += delta;
                }

                rotationZ = selectedVisuals.Max(x => x.RotationZ);
            }
        }

        public Point3D Center
        {
            get
            {
                if (selectedVisuals.Count == 0)
                    return new Point3D();

                var positions = selectedVisuals.Select(x => x.Position + x.Offset).ToList();
                return new Point3D(positions.Average(x => x.X), positions.Average(x => x.Y), positions.Average(x => x.Z));
            }
        }


        public SelectedVisualModel()
        {
            selectedVisuals = new List<ITechneVisual>();
        }


        public void SelectVisual(ITechneVisual visual)
        {
            selectedVisuals.Add(visual);
            RecalculateProperties();
        }

        public void ClearSelection()
        {
            selectedVisuals.Clear();
            RecalculateProperties();
        }

        public void AddVisual(ITechneVisual visual)
        {
            if (selectedVisuals.Contains(visual))
                return;

            selectedVisuals.Add(visual);

            RecalculateProperties();
        }

        public void RemoveVisual(ITechneVisual visual)
        {
            selectedVisuals.Remove(visual);
            RecalculateProperties();
        }

        public bool IsSelected(ITechneVisual visual)
        {
            return selectedVisuals.Contains(visual);
        }

        private void RecalculateProperties()
        {
            if (selectedVisuals.Count > 0)
            {
                width = selectedVisuals.Max(x => x.Width);
                height = selectedVisuals.Max(x => x.Height);
                length = selectedVisuals.Max(x => x.Length);
                if (selectedVisuals.Where(x => !x.IsMirrored).Count() == selectedVisuals.Count)
                    isMirrored = false;
                else if (selectedVisuals.Where(x => x.IsMirrored).Count() == selectedVisuals.Count)
                    isMirrored = true;
                else
                    isMirrored = false;
                offsetX = selectedVisuals.Max(x => x.Offset.X);
                offsetY = selectedVisuals.Max(x => x.Offset.Y);
                offsetZ = selectedVisuals.Max(x => x.Offset.Z);
                positionX = selectedVisuals.Max(x => x.Position.X);
                positionY = selectedVisuals.Max(x => x.Position.Y);
                positionZ = selectedVisuals.Max(x => x.Position.Z);
                textureOffsetX = selectedVisuals.Max(x => x.TextureOffset.X);
                textureOffsetY = selectedVisuals.Max(x => x.TextureOffset.Y);
                rotationX = selectedVisuals.Max(x => x.RotationX);
                rotationY = selectedVisuals.Max(x => x.RotationY);
                rotationZ = selectedVisuals.Max(x => x.RotationZ);
            }
            else
            {
                width = 0;
                height = 0;
                length = 0;
                isMirrored = false;
                offsetX = 0;
                offsetY = 0;
                offsetZ = 0;
                positionX = 0;
                positionY = 0;
                positionZ = 0;
                textureOffsetX = 0;
                textureOffsetY = 0;
                rotationX = 0;
                rotationY = 0;
                rotationZ = 0;
            }
        }

        internal bool HasSelected()
        {
            return selectedVisuals.Count > 0;
        }

        internal void ToggleSelection(ITechneVisual visual)
        {
            if (selectedVisuals.Contains(visual))
                selectedVisuals.Remove(visual);
            else
                selectedVisuals.Add(visual);
        }
    }
}

