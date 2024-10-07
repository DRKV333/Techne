/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
//using System;
//using System.Collections;
//using System.Dynamic;
//using System.Linq;
//using YaTools.Yaml;

//namespace Techne
//{
//    public static class YamlDoc
//    {
//        public static dynamic Load(string fileName)
//        {
//            var document = YamlLanguage.FileTo(fileName);

//            object result;
//            if (TryMapValue(document, out result))
//                return result;

//            throw new Exception("Unexpected parsed value");
//        }

//        private static object MapValue(object value)
//        {
//            object result;
//            TryMapValue(value, out result);
//            return result;
//        }

//        internal static bool TryMapValue(object value, out object result)
//        {
//            if (value is string)
//            {
//                result = value as string;
//                return true;
//            }

//            if (value is ArrayList)
//            {
//                result = (value as ArrayList).Cast<object>().Select(MapValue).ToList();
//                return true;
//            }

//            if (value is Hashtable)
//            {
//                result = new YamlMapping(value as Hashtable);
//                return true;
//            }

//            if (value is TaggedScalar)
//            {
//                result = (value as TaggedScalar).Value;
//                return true;
//            }

//            result = null;
//            return false;
//        }
//    }

//    class YamlMapping : DynamicObject
//    {
//        private readonly Hashtable _mapping;

//        public YamlMapping(Hashtable mapping)
//        {
//            _mapping = mapping;
//        }

//        public override bool TryGetMember(GetMemberBinder binder, out object result)
//        {
//            if (TryGetValue(binder.Name, out result))
//                return true;

//            return base.TryGetMember(binder, out result);
//        }

//        private bool TryGetValue(string key, out object result)
//        {
//            if (_mapping.ContainsKey(key))
//            {
//                var value = _mapping[key];

//                if (YamlDoc.TryMapValue(value, out result))
//                    return true;
//            }

//            result = null;
//            return false;
//        }

//        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
//        {
//            var key = indexes[0] as string;
//            if (key != null)
//            {
//                if (TryGetValue(key, out result))
//                    return true;
//            }

//            return base.TryGetIndex(binder, indexes, out result);
//        }
//    }
//}


