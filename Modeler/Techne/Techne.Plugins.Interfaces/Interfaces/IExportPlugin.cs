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
using Techne.Models;

namespace Techne.Plugins.Interfaces
{
    public interface IExportPlugin : ITechnePlugin, IImportExportPlugin
    {
        void Export(string filename, IEnumerable<ITechneVisual> visuals, Dictionary<string, IShapePlugin> shapes, TechneModel techneModel);
    }
}

