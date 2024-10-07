/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
// <copyright file="Transform3DToTransform3DCollectionConverter.cs" company="$registerdorganization$">
// Copyright (c) 2011 Microsoft. All Right Reserved
// </copyright>
// <author>Alex</author>
// <email></email>
// <date>2011-07-23</date>
// <summary>A value converter for WPF and Silverlight data binding</summary>

using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Media3D;

namespace Techne.ValueConverters
{
    /// <summary>
    /// A Value converter
    /// </summary>
    public class Transform3DToTransform3DGroupConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Transform3DGroup collection = new Transform3DGroup();
            foreach (var item in values)
            {
                if (item is Transform3D)
                    collection.Children.Add(item as Transform3D);
            }
            return collection;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

