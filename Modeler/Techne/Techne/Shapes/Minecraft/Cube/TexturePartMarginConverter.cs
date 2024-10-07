/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Techne.Plugins.Shapes
{
    public class TexturePartMarginConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 3)
                return new Thickness(0);

            var visualSize = values[0] as Vector3D?;
            var textureSize = values[1] as Vector?;
            var cubeSide = parameter as CubeSide?;

            if (!visualSize.HasValue || !textureSize.HasValue || !cubeSide.HasValue)
                return new Thickness(0);

            TextureLength length = new TextureLength(new Vector(), textureSize.Value, visualSize.Value, false);

            double top = 0;
            double left = 0;

            //should change textureCoordinate order to CubeSideOrder (or the other way round)
            switch (cubeSide.Value)
            {
                case CubeSide.Left:
                    //doens't matter which point I chose for Top
                    //top = textureCoordinates[1][0].Y * textureSize.Value.Y;

                    top = length.Height(1);
                    break;
                case CubeSide.Right:
                    //doens't matter which point I chose for Top
                    //top = textureCoordinates[1][0].Y * textureSize.Value.Y;
                    //left = textureCoordinates[0][2].X * textureSize.Value.X;

                    top = length.Height(1);
                    left = length.Width(2);
                    break;
                case CubeSide.Front:
                    //doens't matter which point I chose for Top
                    //top = textureCoordinates[1][0].Y * textureSize.Value.Y;
                    //left = textureCoordinates[2][0].X * textureSize.Value.X;

                    top = length.Height(1);
                    left = length.Width(1);
                    break;
                case CubeSide.Back:
                    //doens't matter which point I chose for Top
                    //top = textureCoordinates[1][0].Y * textureSize.Value.Y;
                    //left = textureCoordinates[5][0].X * textureSize.Value.X;

                    top = length.Height(1);
                    left = length.Width(3);
                    break;
                case CubeSide.Top:
                    //left = textureCoordinates[2][0].X * textureSize.Value.X;
                    left = length.Width(1);
                    break;
                case CubeSide.Bottom:
                    //left = textureCoordinates[3][0].X * textureSize.Value.X;
                    left = length.Width(2);
                    break;
                default:
                    break;
            }
            return new Thickness(left, top, 0, 0);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

