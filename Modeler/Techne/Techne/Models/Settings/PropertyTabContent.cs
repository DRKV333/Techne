/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using WPG;

namespace Techne.Model.Settings
{
    internal class PropertyTabContent
    {
        public PropertyGrid PropertyGrid
        {
            get;
            set;
        }

        public SettingCollectionAttribute Attribute
        {
            get;
            set;
        }
    }
}

