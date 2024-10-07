/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;

namespace Techne.Model.Settings
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class SettingAttribute : Attribute
    {
        public SettingAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Description
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    internal sealed class SettingCollectionAttribute : Attribute
    {
        public SettingCollectionAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Description
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }
}

