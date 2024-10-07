using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace EditableBehaviorLibrary
{
    public class EditableTextBoxBehavior : EditableBehavior 
    {
        protected override EditableAdorner GetEditableAdorner()
        {
            return new EditableTextBoxAdorner(this.AssociatedObject, 
                AdornerLayer.GetAdornerLayer(this.AssociatedObject), this.MinimumEditWidth);
        }
    }
}
