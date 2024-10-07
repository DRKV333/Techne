/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.ComponentModel.Composition;

namespace Techne.Plugins.Attributes
{
    /// <summary>
    /// Every Plugin has to use this attribute, it will be ignored if it doens't
    /// </summary>
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PluginExportAttribute : ExportAttribute
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public String Name { get; private set; }
        /// <summary>
        /// Gets the version of the plugin.
        /// </summary>
        public String Version { get; private set; }
        /// <summary>
        /// Gets the author of the plugin.
        /// </summary>
        public String Author { get; private set; }
        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public String Description { get; private set; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public String Guid { get; private set; }

        public PluginExportAttribute(String name, String version, String author, String description, String guid)
        {
            this.Name = name;
            this.Version = version;
            this.Author = author;
            this.Description = description;
            this.Guid = guid;
        }
    }
}

