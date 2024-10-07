using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Input;

namespace EditableBehaviorLibrary
{
    public class EditIconAdorner : Adorner
    {
        private Button _button;
        private AdornerLayer _adornerLayer;
        private DoubleAnimation _fadeIn;
        private DoubleAnimation _fadeOut;
        private const double SIZE = 24.0;

        public event EventHandler Edit;

        public EditIconAdorner(Label adornedElement, AdornerLayer adornerLayer) : base(adornedElement)
        {
            _button = new Button() 
            { 
                Width = SIZE, 
                Height = SIZE, 
                Opacity = 0.0,
            };
            Image img = new Image() 
            { 
                Source = Utility.GetBitmapImage(Utility.EditIconUri),
                Width = SIZE, 
                Height = SIZE,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            _button.Content = img;

            _fadeIn = new DoubleAnimation(1.0, TimeSpan.FromSeconds(1.0));
            _fadeOut = new DoubleAnimation(0.0, TimeSpan.FromSeconds(1.0));

            _button.Click += new RoutedEventHandler(_button_Click);
            _button.MouseEnter += new MouseEventHandler(_button_MouseEnter);
            _button.MouseLeave += new MouseEventHandler(_button_MouseLeave);

            base.AddVisualChild(_button);
            base.AddLogicalChild(_button);

            _adornerLayer = adornerLayer;
            adornerLayer.Add(this);
        }

        void _button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_button.Opacity != 0.0)
            {
                FadeIn();
            }
        }

        void _button_MouseLeave(object sender, MouseEventArgs e)
        {
            FadeOut();
        }

        void _button_Click(object sender, RoutedEventArgs e)
        {
            if (Edit != null)
            {
                Edit(this, EventArgs.Empty);
            }
        }

        protected override Size MeasureOverride(Size constraint)
        {
            _button.Measure(constraint);
            return _button.DesiredSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double originX = this.AdornedElement.RenderSize.Width + 3;
            _button.Arrange(new Rect(new Point(originX, 0), new Size(SIZE, SIZE)));
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _button;
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        public void FadeIn()
        {
            _button.BeginAnimation(Button.OpacityProperty, _fadeIn);
        }

        public void FadeOut()
        {
            _button.BeginAnimation(Button.OpacityProperty, _fadeOut);
        }

        public void Destroy()
        {
            if (_adornerLayer != null)
            {
                _button.Click -= new RoutedEventHandler(_button_Click);
                _button.MouseEnter -= new MouseEventHandler(_button_MouseEnter);
                _button.MouseLeave -= new MouseEventHandler(_button_MouseLeave);
                base.RemoveLogicalChild(_button);
                base.RemoveVisualChild(_button);
                _adornerLayer.Remove(this);
            }
        }
    }
}
