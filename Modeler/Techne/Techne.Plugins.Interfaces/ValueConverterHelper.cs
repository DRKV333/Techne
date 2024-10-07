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

namespace Techne.Plugins
{
    public static class ValueConverterHelper
    {
        private static Dictionary<Type, IValueConverter> converterBase;
        
        static ValueConverterHelper()
        {
            converterBase = new Dictionary<Type, IValueConverter>();
        }

        public static void RegisterConverter(IValueConverter converter)
        {
            if (!converterBase.ContainsKey(converter.GetType()))
                converterBase.Add(converter.GetType(), converter);
            else
                throw new ArgumentException("A converter of this type has been added");
        }

        public static IValueConverter GetConverter(Type type)
        {
            //if (!converterBase.ContainsKey(type))
            //    return null;

            //return converterBase[type];
            if (type == null)
                return null;

            return type.GetConstructor(new Type[0]).Invoke(null) as IValueConverter;
        }
    }
}

