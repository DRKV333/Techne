/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Collections.Generic;

namespace Techne
{
    internal class Changelog
    {
        public ICollection<ChangeLogEntry> Entries
        {
            get;
            set;
        }

        public bool IsNodeExpanded
        {
            get;
            set;
        }

        public string VersionString
        {
            get
            {
                if (Version == null)
                    Version = new Version();
                return Version.ToString();
            }
            set
            {
                try
                {
                    Version = Version.Parse(value);
                }
                catch
                {
                }
            }
        }

        public Version Version
        {
            get;
            set;
        }

        public String ReleaseDate
        {
            get;
            set;
        }
    }
}

