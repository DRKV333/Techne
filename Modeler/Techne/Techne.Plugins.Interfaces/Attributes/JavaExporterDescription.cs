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

namespace Techne.Plugins.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class JavaExporterDescriptionBase : Attribute
    {
        public String Name
        {
            get;
            private set;
        }
        public Type[] ValueConverter
        {
            get;
            private set;
        }
        public int Position
        {
            get;
            private set;
        }

        public JavaExporterDescriptionBase(String name, int position, Type[] converter)
        {
            this.Name = name;
            this.ValueConverter = converter;
            this.Position = position;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class JavaConstructorAttribute : JavaExporterDescriptionBase
    {
        public JavaConstructorAttribute(String name, int i, Type[] converter)
            : base(name, i, converter)
        {
        }

        public JavaConstructorAttribute(String name, int i, Type converter)
            : base(name, i, new Type[] { converter })
        {
        }

        public JavaConstructorAttribute(String name, int i)
            : base(name, i, null)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class JavaMethodAttribute : JavaExporterDescriptionBase
    {
        public JavaMethodAttribute(String name, int i, Type[] converter)
            : base(name, i, converter)
        {
        }

        public JavaMethodAttribute(String name, int i, Type converter)
            : base(name, i, new Type[] { converter })
        {
        }

        public JavaMethodAttribute(String name, int i)
            : base(name, i, null)
        {
        }
    }
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class JavaFieldAttribute : JavaExporterDescriptionBase
    {
        public JavaFieldAttribute(String name, Type[] converter)
            : base(name, 1, converter)
        {
        }

        public JavaFieldAttribute(String name, Type converter)
            : base(name, 1, new Type[] { converter })
        {
        }

        public JavaFieldAttribute(String name)
            : base(name, 1, null)
        {
        }
    }
}

