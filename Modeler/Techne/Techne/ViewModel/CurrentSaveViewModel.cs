/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using Cinch;

namespace Techne.ViewModel
{
    internal class CurrentSaveViewModel : EditableValidatingViewModelBase
    {
        private string filename;

        public String Filename
        {
            get { return filename; }
            set { filename = value; }
        }
    }
}

