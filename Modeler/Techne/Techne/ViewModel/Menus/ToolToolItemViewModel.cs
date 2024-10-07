/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using Cinch;
using Techne.Plugins.Interfaces;

namespace Techne
{
    public class ToolToolItemViewModel
    {
        private string guid;

        private IToolPlugin toolPlugin;

        public ToolToolItemViewModel(string guid, IToolPlugin toolPlugin)
        {
            // TODO: Complete member initialization
            this.guid = guid;
            this.toolPlugin = toolPlugin;
        }

        public string Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public IToolPlugin ToolPlugin
        {
            get { return toolPlugin; }
            set { toolPlugin = value; }
        }

        public SimpleCommand<Object, Object> Command
        {
            get;
            set;
        }
    }
}

