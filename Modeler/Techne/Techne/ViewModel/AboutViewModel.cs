/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using Cinch;
using MEFedMVVM.ViewModelLocator;

namespace Techne.ViewModel
{
    [ExportViewModel("AboutViewModel")]
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            CloseCommand = new SimpleCommand<object, object>(ExecuteCloseCommand);
        }

        public SimpleCommand<Object, Object> CloseCommand
        {
            get;
            set;
        }

        internal void ExecuteCloseCommand(Object obj)
        {
            RaiseCloseRequest(true);
        }
    }
}

