/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.Windows.Media.Imaging;
using Cinch;

namespace Techne.Model
{
    public class ShapeToolItemViewModel
    {
        #region Public Properties
        public String Text
        {
            get;
            set;
        }

        public String IconUrl
        {
            get;
            set;
        }

        public BitmapSource Icon
        {
            get;
            set;
        }

        public SimpleCommand<Object, Object> Command
        {
            get;
            private set;
        }
        #endregion

        #region Ctor
        public ShapeToolItemViewModel(string text, SimpleCommand<Object, Object> command)
        {
            Text = text;
            Command = command;
        }
        #endregion
    }
}

