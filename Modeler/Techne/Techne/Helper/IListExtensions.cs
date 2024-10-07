/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Collections;
using System.Collections.Generic;

namespace Techne.Helper
{
    internal static class IListExtensions
    {
        public static IList<T> ToList<T>(this IList iList)
        {
            IList<T> result = new List<T>();

            foreach (T value in iList)
            {
                result.Add(value);
            }

            return result;
        }
    }
}

