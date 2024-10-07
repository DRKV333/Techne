using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace HelixToolkit
{
    static class IListExtensions
    {
        public static List<T> ToList<T>(this IList iList)
        {
            List<T> result = new List<T>();

            foreach (T value in iList)
            {
                result.Add(value);
            }

            return result;
        }
    }
}
