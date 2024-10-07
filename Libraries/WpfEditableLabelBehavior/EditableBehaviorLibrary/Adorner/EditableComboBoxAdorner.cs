using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows;
using System.Collections;
using System.Windows.Media;

namespace EditableBehaviorLibrary
{
    public class EditableComboBoxAdorner : EditableAdorner 
    {
        private ComboBox _comboBox;

        public EditableComboBoxAdorner(Label adornedElement, AdornerLayer adornerLayer, double minEditWidth,
            string displayMemberPath, string selectedValuePath, IEnumerable itemsSource)
            : base(adornedElement, adornerLayer, minEditWidth)
        {
            _comboBox.MinWidth = minEditWidth;
            _comboBox.Width = adornedElement.RenderSize.Width;
            _comboBox.Height = adornedElement.RenderSize.Height;
            _comboBox.DisplayMemberPath = displayMemberPath;
            _comboBox.SelectedValuePath = selectedValuePath;
            _comboBox.ItemsSource = itemsSource;
            _comboBox.SelectedValue = adornedElement.Content.ToString();
        }

        protected override UIElement InputElement
        {
            get 
            {
                if (_comboBox == null)
                {
                    _comboBox = new ComboBox();
                }
                return _comboBox;
            }
        }

        public override string TextEntered
        {
            get { return _comboBox.SelectedValue.ToString(); }
        }

        public override void ValidationError(string errorMessage)
        {
            _comboBox.BorderBrush = Brushes.Red;
            _comboBox.BorderThickness = new Thickness(1);
            _comboBox.ToolTip = new ToolTip() { Content = errorMessage };
        }
    }
}
