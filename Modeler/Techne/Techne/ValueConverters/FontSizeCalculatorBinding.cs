/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Techne.ValueConverters
{
    public class FontSizeCalculatorBinding : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return 12;

            var height = values[0] as double?;
            var width = values[1] as double?;
            //var margin = values[2] as Thickness?;
            var text = parameter as string;

            if (!height.HasValue || !width.HasValue || text == null)
                return 12;

            TextBlock tmp = new TextBlock
                            {
                                FontSize = 20,
                                Text = text
                            };

            tmp.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            while ((tmp.DesiredSize.Height >= height || tmp.DesiredSize.Height > width) && (height >= 5 && width >= 5 && tmp.FontSize - 1 > 0.5))
            {
                var newSize = tmp.FontSize - 1;
                tmp.FontSize = newSize <= 0 ? 0.01 : newSize;
                tmp.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            }

            if (height < 5 || width < 5)
            {
                tmp.FontSize = 0.01;
            }

            return tmp.FontSize;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

