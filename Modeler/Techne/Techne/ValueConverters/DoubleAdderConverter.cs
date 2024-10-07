/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace Techne.ValueConverters
{
    internal class DoubleAdderConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double result = values.Cast<double>().Sum();

            double multiplicator;

            if (parameter != null && Double.TryParse(parameter.ToString(), out multiplicator))
            {
                result *= multiplicator;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

