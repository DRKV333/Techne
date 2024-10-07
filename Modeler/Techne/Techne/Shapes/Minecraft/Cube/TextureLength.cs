/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Windows;
using System.Windows.Media.Media3D;
using Techne.Plugins.Interfaces;

namespace Techne.Plugins.Shapes
{
    internal class TextureLength
    {
        private readonly double height;
        private readonly double length;
        private readonly bool useOffset = true;
        private readonly double width;
        private Vector textureOffset;

        private Vector textureSize;
        //private ITechneVisual visual;
        private bool usePercent;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextureLength"/> class.
        /// </summary>
        /// <param name="visual">The visual.</param>
        /// <param name="textureSize">Size of the texture.</param>
        public TextureLength(ITechneVisual visual, Vector textureSize) : this(visual, textureSize, true)
        {
        }

        public TextureLength(ITechneVisual visual, Vector textureSize, Boolean useOffset) : this(visual.TextureOffset, textureSize, useOffset, visual.Width, visual.Length, visual.Height)
        {
        }

        public TextureLength(Vector textureOffset, Vector textureSize, Vector3D visualSize, Boolean useOffset)
            : this(textureOffset, textureSize, useOffset, visualSize.X, visualSize.Y, visualSize.Z)
        {
        }

        public TextureLength(Vector textureOffset, Vector textureSize, Boolean useOffset, double width, double length, double height)
        {
            this.textureOffset = textureOffset;
            this.textureSize = textureSize;
            //this.textureSize = new Vector(64, 32);
            this.useOffset = useOffset;
            this.width = width;
            this.height = height;
            this.length = length;
        }

        /// <summary>
        /// Calculates the horizontal offset in a viewbox
        /// </summary>
        /// <param name="count">Specifies the side to calculate the offset to.</param>
        /// <returns></returns>
        public double Width(int count)
        {
            double result = 0;

            if (useOffset)
                result = textureOffset.X;

            if (usePercent)
            {
                result += (height / textureSize.X) * Math.Floor((count + 1) / (double)2);

                if (count > 1)
                {
                    result += (width / textureSize.X) * Math.Floor(count / (double)2);
                }
            }
            else
            {
                result += count > 0 ? height : 0;
                result += count > 1 ? width : 0;
                result += count > 2 ? height : 0;
                result += count > 3 ? width : 0;
            }

            return result;
        }

        /// <summary>
        /// Calculates the vertical offset in a viewbox
        /// </summary>
        /// <param name="count">Specifies the side to calculate the offset to.</param>
        /// <returns></returns>
        public double Height(int count)
        {
            double result = 0;

            if (useOffset)
                result = textureOffset.Y;

            if (usePercent)
            {
                result += (height / textureSize.Y) * Math.Floor((count + 1) / (double)2);

                if (count > 1)
                {
                    result += (length / textureSize.Y) * Math.Floor(count / (double)2);
                }
            }
            else
            {
                result += count > 0 ? height : 0;
                result += count > 1 ? length : 0;
            }

            return result;
        }
    }
}

