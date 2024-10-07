using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections;
using System.Windows.Media.Imaging;

namespace EditableBehaviorLibrary
{
    public abstract class EditableAdorner : Adorner
    {
        private AdornerLayer _adornerLayer;
        private StackPanel _stackPanel;
        private Button _saveButton;
        private Button _cancelButton;
        private const double SIZE = 24.0;

        public event EventHandler Save;
        public event EventHandler Cancel;

        public EditableAdorner(Label adornedElement, AdornerLayer adornerLayer, double minEditWidth)
            : base(adornedElement)
        {
            _adornerLayer = adornerLayer;
            _stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };
         
            _saveButton = CreateButtonWithImageContent(Utility.SaveIconUri);
            _saveButton.Click += new RoutedEventHandler(button_Click);

            _cancelButton = CreateButtonWithImageContent(Utility.CancelIconUri);
            _cancelButton.Click += new RoutedEventHandler(_cancelButton_Click);

            _stackPanel.Children.Add(InputElement);
            _stackPanel.Children.Add(_saveButton);
            _stackPanel.Children.Add(_cancelButton);
            base.AddLogicalChild(_stackPanel);
            base.AddVisualChild(_stackPanel);
            adornerLayer.Add(this);
        }

        protected abstract UIElement InputElement { get; }
        
        public abstract string TextEntered { get; }

        public abstract void ValidationError(string errorMessage);

        private Button CreateButtonWithImageContent(string imageUri)
        {
            Button button = new Button()
            {
                Width = SIZE + 1,
                Height = SIZE + 1,
                Margin = new Thickness(4, 0, 0, 0),
                Padding = new Thickness(1)
            };
            Image img = new Image()
            {
                Source = Utility.GetBitmapImage(imageUri),
                Width = SIZE,
                Height = SIZE,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            button.Content = img;
            return button;
        }

        void _cancelButton_Click(object sender, RoutedEventArgs e)
        {
            OnCancel();
        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            OnSave();
        }

        protected void OnCancel()
        {
            if (Cancel != null)
            {
                Cancel(this, EventArgs.Empty);
            }
        }

        protected void OnSave()
        {
            if (Save != null)
            {
                Save(this, EventArgs.Empty);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _stackPanel.Measure(constraint);
            return _stackPanel.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            _stackPanel.Arrange(new Rect(finalSize));
            return finalSize;
        }

        protected override System.Windows.Media.Visual GetVisualChild(int index)
        {
            return _stackPanel;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override IEnumerator LogicalChildren
        {
            get
            {
                ArrayList list = new ArrayList();
                list.Add(this._stackPanel);
                return (IEnumerator)list.GetEnumerator();
            }
        }

        public void Destroy()
        {
            if (_adornerLayer != null)
            {
                _saveButton.Click -= new RoutedEventHandler(button_Click);
                _cancelButton.Click -= new RoutedEventHandler(_cancelButton_Click);
                base.RemoveLogicalChild(_stackPanel);
                base.RemoveVisualChild(_stackPanel);
                _adornerLayer.Remove(this);
            }
        }

    }
}
