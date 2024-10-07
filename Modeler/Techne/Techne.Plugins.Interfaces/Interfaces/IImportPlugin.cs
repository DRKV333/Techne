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

namespace Techne.Plugins.Interfaces
{
    public interface IImportPlugin : ITechnePlugin, IImportExportPlugin
    {
        IEnumerable<ITechneVisual> Import(Dictionary<string, Techne.Plugins.Interfaces.IShapePlugin> shapes, string filename);
    }
}

