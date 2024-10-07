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
using System.Windows.Data;

namespace Techne.Plugins.ValueConverter
{
    public class DegreeToRadiansConverter : IValueConverter
    {
        static DegreeToRadiansConverter()
        {
            ValueConverterHelper.RegisterConverter(new DegreeToRadiansConverter());
        }

        static DegreeToRadiansConverter instance;

        public static DegreeToRadiansConverter Instance
        {
            get { return DegreeToRadiansConverter.instance; }
            set { DegreeToRadiansConverter.instance = value; }
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return Math.PI / 180 * (double)value;
            }
            else if (value is float)
            {
                return (float)(Math.PI / 180 * (float)value);
            }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                return 180 / Math.PI * (double)value;
            }
            else if (value is float)
            {
                return (float)(180 / Math.PI * (float)value);
            }

            return 0;
        }

        public static DegreeToRadiansConverter GetConverter()
        {
            if (instance == null)
                instance = new DegreeToRadiansConverter();

            return instance;
        }
    }
}

