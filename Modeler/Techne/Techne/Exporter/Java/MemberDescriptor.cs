/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Techne.Plugins.Attributes;

namespace Techne.Plugins.FileHandler.TurboModelThingy
{
    internal class MemberDescriptor
    {
        private readonly Dictionary<string, EntryList> entries;

        public MemberDescriptor()
        {
            entries = new Dictionary<string, EntryList>();
        }

        public MemberDescriptor(JavaExporterDescriptionBase attribute, PropertyInfo property) : this()
        {
            AddAttribute(attribute, property);
        }

        public IEnumerable<List<Entry>> MethodEntries
        {
            get
            {
                return from entry in entries.Values
                       where entry.Type.Equals(typeof(JavaMethodAttribute))
                       select entry;
                //return entries.Values.Select(x => (x.FirstOrDefault() ?? new Entry(null, null)).Attribute.GetType().Equals(typeof(JavaMethodAttribute)));
            }
        }

        public IEnumerable<List<Entry>> FieldEntries
        {
            get
            {
                return from entry in entries.Values
                       where entry.Type.Equals(typeof(JavaFieldAttribute))
                       select entry;
            }
        }

        public List<Entry> ConstructorEntry
        {
            get
            {
                var entryList = GetDescription(typeof(JavaConstructorAttribute));

                return entryList;
            }
        }

        internal void AddAttribute(JavaExporterDescriptionBase attribute, PropertyInfo property)
        {
            if (entries.ContainsKey(attribute.Name))
            {
                entries[attribute.Name].Add(new Entry(attribute, property));

                //entries[attribute.Name] = entries[attribute.Name].OrderBy<Entry, int>(x => x.Attribute.Position).ToList() as EntryList;
            }
            else
            {
                entries.Add(attribute.Name,
                            new EntryList(attribute.GetType())
                            {
                                new Entry(attribute, property)
                            });
            }
        }

        internal List<Entry> GetDescription(Type type)
        {
            var result = (from entry in entries.Values
                          where entry.Type.Equals(type)
                          select entry).FirstOrDefault();

            if (result == null)
                return null;

            return result;
        }

        #region Nested type: EntryList
        private class EntryList : List<Entry>
        {
            private String name;
            private Type type;

            public EntryList(Type type)
            {
                this.type = type;
            }

            public String Name
            {
                get { return name; }
                set { name = value; }
            }

            public Type Type
            {
                get { return type; }
                set { type = value; }
            }
        }
        #endregion
    }

    internal class Entry
    {
        private readonly JavaExporterDescriptionBase attribute;

        private readonly PropertyInfo property;

        public Entry(JavaExporterDescriptionBase attribute, PropertyInfo property)
        {
            this.attribute = attribute;
            this.property = property;
        }

        public JavaExporterDescriptionBase Attribute
        {
            get { return attribute; }
        }

        public PropertyInfo Property
        {
            get { return property; }
        }
    }
}

