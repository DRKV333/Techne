/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Techne
{
    public static class IEnumerableExtension
    {
        public static List<T> ConcatToList<T>(this IEnumerable<IEnumerable<T>> data)
        {
            List<T> list = new List<T>();

            foreach (var value in data)
            {
                foreach (var item in value)
                {
                    list.Add(item);
                }
            }

            return list;
        }

        public static ObservableCollection<T> ConcatToObservableCollection<T>(this IEnumerable<IEnumerable<T>> data)
        {
            ObservableCollection<T> list = new ObservableCollection<T>();

            foreach (var value in data)
            {
                foreach (var item in value)
                {
                    list.Add(item);
                }
            }

            return list;
        }
    }
}

