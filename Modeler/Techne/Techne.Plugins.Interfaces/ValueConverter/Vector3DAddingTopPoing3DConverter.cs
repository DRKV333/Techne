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
using System.Windows.Media.Media3D;

namespace Techne.Plugins.ValueConverter
{
    public class Vector3DAddingToRect3DConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values.Length != 3)
                return null;

            var start = values[0] as Vector3D?;
            var end = values[1] as Vector3D?;

            var point = values[2] as Vector3D?;

            if (!start.HasValue || !end.HasValue || !point.HasValue)
                return null;

            return ToRect3D(start.Value + end.Value, point.Value);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private Rect3D ToRect3D(Vector3D p, Vector3D n)
        {
            return new Rect3D(p.X, p.Y, p.Z, n.X, n.Y, n.Z);
        }
    }
}


