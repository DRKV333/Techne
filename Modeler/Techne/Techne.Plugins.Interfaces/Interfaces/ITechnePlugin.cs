/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;

namespace Techne.Plugins.Interfaces
{
    public interface ITechnePlugin
    {
        /// <summary>
        /// Gets or sets the Guid for this plugin
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Called when plugin is loaded
        /// </summary>
        void OnLoad();
    }
}

