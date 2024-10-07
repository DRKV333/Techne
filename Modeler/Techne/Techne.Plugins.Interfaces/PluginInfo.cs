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
using Techne.Plugins.Attributes;
using Techne.Plugins;
using System.Xml.Serialization;

namespace Techne
{
    [XmlRoot("Plugin")]
    public class PluginInfo
    {
        public PluginInfo()
        {
        }

        public PluginInfo(Plugins.Interfaces.IPythonPlugin source)
        {
            try { PluginName = source.Name; }
            catch { }
            try { Author = source.Author; }
            catch { }
            try { Description = source.Description; }
            catch { }
            try { Version = source.Version; }
            catch { }
            try { GUID = source.Guid.ToString(); }
            catch { }
            try { Type = source.PluginType; }
            catch { }
        }

        public PluginInfo(PluginExportAttribute item)
        {
            PluginName = item.Name;
            Author = item.Author;
            Description = item.Description;
            GUID = item.Guid;
            Version = item.Version;
        }

        [XmlIgnore]
        public List<PluginExportAttribute> Types
        {
            get;
            set;
        }
        [XmlElement("PluginName")]
        public string PluginName
        {
            get;
            set;
        }
        [XmlElement("Author")]
        public string Author
        {
            get;
            set;
        }
        [XmlElement("Description")]
        public string Description
        {
            get;
            set;
        }
        [XmlElement("Version")]
        public string Version
        {
            get;
            set;
        }
        [XmlElement("GUID")]
        public string GUID
        {
            get;
            set;
        }
        [XmlElement("Location")]
        public string Location
        {
            get;
            set;
        }
        [XmlElement("Type")]
        public PluginType Type
        {
            get;
            set;
        }
        [XmlElement("Activated")]
        public Boolean Activated { get; set; }
        [XmlIgnore]
        public Boolean Loaded { get; set; }
    }
}

