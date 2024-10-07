/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Globalization;
using System.Windows.Data;

namespace Techne.ValueConverters
{
    public class ScaleConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var desiredWidth = values[0] as double?;
            var desiredHeight = values[1] as double?;
            var actualWidth = values[2] as double?;
            var actualHeight = values[3] as double?;

            if (!desiredWidth.HasValue || !desiredHeight.HasValue || !actualWidth.HasValue || !actualHeight.HasValue || actualWidth.Value == 0 || actualHeight.Value == 0)
                return 1;

            desiredHeight = desiredHeight.Value == 0 ? 64 : desiredHeight.Value;
            desiredWidth = desiredWidth.Value == 0 ? 64 : desiredWidth.Value;

            var scaleX = desiredWidth.Value / actualWidth.Value;
            var scaleY = desiredHeight.Value / actualHeight.Value;

            int axis;

            if (!Int32.TryParse(parameter.ToString(), out axis))
                return Math.Min(scaleX, scaleY);

            return axis == 0 ? scaleX : scaleY;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

