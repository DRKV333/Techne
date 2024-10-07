using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace EditableBehaviorLibrary
{
    public class EditableTextBoxAdorner : EditableAdorner 
    {
        private TextBox _textBox;

        public EditableTextBoxAdorner(Label adornedElement, AdornerLayer adornerLayer, double minEditWidth)
            : base(adornedElement, adornerLayer, minEditWidth)
        {
            _textBox.MinWidth = minEditWidth;
            _textBox.Width = adornedElement.RenderSize.Width;
            _textBox.Height = adornedElement.RenderSize.Height;
            _textBox.Text = adornedElement.Content.ToString();
            _textBox.Background = new SolidColorBrush(Color.FromRgb(0x43, 0x43, 0x43));
            _textBox.Padding = new Thickness(1) { Top = 0 };
            _textBox.KeyDown += _textBox_KeyDown;
            _textBox.Focus();
            _textBox.CaretIndex = _textBox.Text.Length;
        }

        void _textBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                OnSave();
            }
            else if (e.Key == System.Windows.Input.Key.Escape)
            {
                OnCancel();
            }
        }

        protected override UIElement InputElement
        {
            get
            {
                if (_textBox == null)
                {
                    _textBox = new TextBox();
                }
                return _textBox;
            }
        }

        public override string TextEntered
        {
            get { return _textBox.Text; }
        }

        public override void ValidationError(string errorMessage)
        {
            _textBox.BorderBrush = Brushes.Red;
            _textBox.BorderThickness = new Thickness(1);
            _textBox.ToolTip = new ToolTip() { Content = errorMessage };
        }
    }
}
