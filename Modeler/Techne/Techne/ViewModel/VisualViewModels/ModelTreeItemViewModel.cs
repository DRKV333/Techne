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
using Cinch;
using Techne.Plugins.Interfaces;

namespace Techne.ViewModel
{
    public class ModelTreeItemViewModel : ViewModelBase
    {
        private ITechneVisual techneVisual;
        private ModelTreeItemViewModel parent;
        private readonly IList<ModelTreeItemViewModel> children;

        public ModelTreeItemViewModel(ITechneVisual techneVisual, ModelTreeItemViewModel parent = null)
        {
            this.techneVisual = techneVisual;
            this.parent = parent;

            children = new List<ModelTreeItemViewModel>(
                      (from child in techneVisual.Children
                       select new ModelTreeItemViewModel(child, this))
                       .ToList<ModelTreeItemViewModel>());
        }

        public IList<ModelTreeItemViewModel> Children
        {
            get
            {
                return children;
            }
        }
    }
}

