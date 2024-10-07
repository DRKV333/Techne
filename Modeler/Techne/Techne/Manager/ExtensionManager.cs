/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Collections.Generic;
using Techne.Plugins.Interfaces;

namespace Techne
{
    public class ExtensionManager
    {
        static ExtensionManager()
        {
            ShapePlugins = new Dictionary<string, IShapePlugin>();
        }

        public static Dictionary<string, IShapePlugin> ShapePlugins
        {
            get;
            set;
        }

        internal static ITechneVisual GetShape(string guidString)
        {
            if (!ShapePlugins.ContainsKey(guidString.ToLower()))
            {
                switch (guidString.ToLower())
                {
                    case "9ec93754-b48d-4c70-ae4e-84d81ae55396":
                        return new Techne.Model.TechneVisualCollection();
                        break;
                    case "f8bf7d5b-37bf-455b-93f9-b6f9e81620e1":
                        return new Techne.Model.TechneVisualFolder();
                        break;
                    default:
                        return ShapePlugins["D9E621F7-957F-4B77-B1AE-20DCD0DA7751".ToLower()].CreateVisual();
                        break;
                }
            }
            
            
            return ShapePlugins[guidString.ToLower()].CreateVisual();
        }

        internal static void RegisterShape(IShapePlugin shapePlugin)
        {
            ShapePlugins.Add(shapePlugin.Guid.ToString().ToLower(), shapePlugin);
        }
    }
}

