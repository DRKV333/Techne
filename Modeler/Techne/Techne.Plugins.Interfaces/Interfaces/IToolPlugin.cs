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
using System.Windows.Media.Imaging;
using Techne.Models;

namespace Techne.Plugins.Interfaces
{
    public interface IToolPlugin : ITechnePlugin
    {
        BitmapSource Icon { get; set; }
        String MenuHeader { get; set; }

        void OnClick(SaveModel saveModel, Dictionary<string, IShapePlugin> shapes);
    }
}

