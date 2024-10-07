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
using System.Windows;
using Techne.Plugins.Interfaces;

namespace Techne.Plugins.Shapes
{
    public static class TextureHelper
    {
        /// <summary>
        /// Gets the viewbox.
        /// </summary>
        /// <param name="visual">The visual.</param>
        /// <returns>A rect that describes the viewbox</returns>
        public static Rect GetViewboxRect(ITechneVisual visual)
        {
            return GetViewboxRect(visual, true);
        }

        /// <summary>
        /// Gets the viewbox.
        /// </summary>
        /// <param name="visual">The visual.</param>
        /// <param name="offset">if set to <c>true</c> calculate offset, otherwise ignore.</param>
        /// <returns>A rect that describes the viewbox</returns>
        public static Rect GetViewboxRect(ITechneVisual visual, bool offset)
        {
            if (visual == null || visual.Texture == null)
                return new Rect();

            double viewBoxX = (visual.Width + visual.Height) * 2;
            double viewBoxY = (visual.Length + visual.Height);

            double offsetX = visual.TextureOffset.X > visual.Texture.PixelWidth ? visual.Texture.PixelWidth : visual.TextureOffset.X;
            double offsetY = visual.TextureOffset.Y > visual.Texture.PixelHeight ? visual.Texture.PixelHeight : visual.TextureOffset.Y;

            offsetX = offsetX < 0 ? 0 : offsetX;
            offsetY = offsetY < 0 ? 0 : offsetY;

            if (offset)
            {

                if (viewBoxX + offsetX > visual.Texture.PixelWidth)
                {
                    viewBoxX = visual.Texture.PixelWidth - offsetX;
                }
                if (viewBoxY + offsetY > visual.Texture.PixelHeight)
                {
                    viewBoxY = visual.Texture.PixelHeight - offsetY;
                }

            }
            else
            {
                offsetX = 0;
                offsetY = 0;
            }

            return new Rect(visual.TextureOffset.X, visual.TextureOffset.Y, viewBoxX, viewBoxY);
        }

        /// <summary>
        /// Gets the offset in texture coordinates.
        /// </summary>
        /// <param name="visual">The visual.</param>
        /// <param name="textureSize">Size of the texture.</param>
        /// <param name="side">The side.</param>
        /// <returns>Vector pointing to the upper left corner of the view box</returns>
        /// <remarks>what did I use that for?</remarks>
        internal static Vector GetOffset(ITechneVisual visual, Vector textureSize, CubeSide side)
        {
            Vector result = new Vector();
            TextureLength textureSide = new TextureLength(visual, textureSize);

            switch (side)
            {
                case CubeSide.Left:
                    result = new Vector(0, textureSide.Height(1));
                    break;
                case CubeSide.Right:
                    result = new Vector(textureSide.Height(2), textureSide.Height(1));
                    break;
                case CubeSide.Front:
                    result = new Vector(textureSide.Height(1), textureSide.Height(1));
                    break;
                case CubeSide.Back:
                    result = new Vector(textureSide.Height(3), textureSide.Height(1));
                    break;
                case CubeSide.Top:
                    result = new Vector(textureSide.Height(1), textureSide.Height(0));
                    break;
                case CubeSide.Bottom:
                    result = new Vector(textureSide.Height(2), textureSide.Height(0));
                    break;
                default:
                    break;
            }

            return result;
        }


        /// <summary>
        /// Gets the viewbox rect.
        /// </summary>
        /// <param name="visual">The visual.</param>
        /// <param name="cubeSide">The cube side.</param>
        /// <returns>A rect that describes the viewbox</returns>
        internal static Rect GetViewboxRect(ITechneVisual visual, CubeSide cubeSide)
        {
            Rect result = new Rect();

            if (visual.TextureSize.X <= 0 || visual.TextureSize.Y <= 0)
                return result;

           
                TextureLength textureSide = new TextureLength(visual, visual.TextureSize);

                switch (cubeSide)
                {
                    case CubeSide.Left:
                        result = new Rect(textureSide.Width(0), textureSide.Height(1), visual.Height, visual.Length);
                        break;
                    case CubeSide.Right:
                        result = new Rect(textureSide.Width(2), textureSide.Height(1), visual.Height, visual.Length);
                        break;
                    case CubeSide.Front:
                        result = new Rect(textureSide.Width(1), textureSide.Height(1), visual.Width, visual.Length);
                        break;
                    case CubeSide.Back:
                        result = new Rect(textureSide.Width(3), textureSide.Height(1), visual.Width, visual.Length);
                        break;
                    case CubeSide.Top:
                        result = new Rect(textureSide.Width(1), textureSide.Height(0), visual.Width, visual.Height);
                        break;
                    case CubeSide.Bottom:
                        result = new Rect(textureSide.Width(2), textureSide.Height(0), visual.Width, visual.Height);
                        break;
                    default:
                        break;
                }
            
            return result;
        }

        /// <summary>
        /// Turns a rect into Int32Rect
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public static Int32Rect ToInt32Rect(this Rect rect)
        {
            return new Int32Rect((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static Point[] ToPoints(this Rect rect)
        {
            return new Point[4]
            {
                rect.TopLeft,
                rect.TopRight,
                rect.BottomRight,
                rect.BottomLeft
            };
        }

        public static Point[] Rotate(this Point[] points, int count)
        {
            Point[] temp = new Point[4];

            Array.Copy(points, 0, temp, 0, count);
            Array.Copy(points, count, points, 0, points.Length - count);
            Array.Copy(temp, 0, points, points.Length - count, count);

            return points;
        }

        /// <summary>
        /// Converts a Rect to relative coordinates
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="textureSize">Size of the texture.</param>
        /// <returns></returns>
        public static Rect ToRelative(this Rect rect, Vector textureSize)
        {
            //textureSize = new Vector(64, 32);

            var result =  new Rect(
                rect.Left / textureSize.X,
                rect.Top / textureSize.Y,
                rect.Width / textureSize.X,
                rect.Height / textureSize.Y);

            //result.Scale(0.9, 0.9);

            return result;
        }

        public static Point[] Mirror(this Point[] points)
        {
            //int done = 0;
            for (int i = 0; i < points.Length; i++)
            {
                var tmp = points[i];
                points[i] = points[i + 1];

                i++;

                points[i] = tmp;
            }

            return points;
        }
    }
}

