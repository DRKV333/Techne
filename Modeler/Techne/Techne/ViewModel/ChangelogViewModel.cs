/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Cinch;

namespace Techne
{
    internal class ChangelogViewModel : ViewModelBase
    {
        public ChangelogViewModel(string result)
        {
            try
            {
                ParserContext Context = new ParserContext();
                Context.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
                Context.XmlnsDictionary.Add("x", "http://schemas.microsoft.com/winfx/2006/xaml");
                Context.XmlnsDictionary.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
                Canvas = XamlReader.Parse(result, Context) as UIElement;
            }
            catch
            {
                Canvas = new Label
                         {
                             Content = "Something went wrong :("
                         };
            }
        }

        public UIElement Canvas
        {
            get;
            set;
        }
    }
}

