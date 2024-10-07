/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using Cinch;

namespace Techne
{
    public class ImportPluginMenuItemViewModel
    {
        public String Header
        {
            get;
            set;
        }

        public SimpleCommand<Object, Object> Command
        {
            get;
            set;
        }
    }
}

