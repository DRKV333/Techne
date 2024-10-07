using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Collections;
using System.Windows;

namespace EditableBehaviorLibrary
{
    public class EditableComboBoxBehavior : EditableBehavior 
    {
        protected override EditableAdorner GetEditableAdorner()
        {
            return new EditableComboBoxAdorner(this.AssociatedObject, 
                AdornerLayer.GetAdornerLayer(this.AssociatedObject), this.MinimumEditWidth, 
                this.DisplayMemberPath, this.SelectedValuePath, this.ItemsSource); 
        }

        public string DisplayMemberPath { get; set; }

        public string SelectedValuePath { get; set; }

        public static readonly DependencyProperty ItemsSourceProperty = 
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(EditableComboBoxBehavior),
            new FrameworkPropertyMetadata(null));

        public IEnumerable ItemsSource 
        {
            get { return (IEnumerable)base.GetValue(ItemsSourceProperty); }
            set { base.SetValue(ItemsSourceProperty, value); }
        }    
    }
}
