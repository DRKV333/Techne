﻿using System;
using System.Windows;
using System.Globalization;
using System.Windows.Input;

namespace Microsoft.Windows.Controls
{
    public class NumericUpDown : UpDownBase
    {
        #region Properties

        #region Minimum

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MinValue, OnMinimumPropertyChanged));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        private static void OnMinimumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown nud = d as NumericUpDown;
            nud.SetValidSpinDirection();
        }

        #endregion Minimum

        #region Maximum

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(NumericUpDown), new PropertyMetadata(Double.MaxValue, OnMaximumPropertyChanged));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        private static void OnMaximumPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown nud = d as NumericUpDown;
            nud.SetValidSpinDirection();
        }

        #endregion Maximum

        #region Increment

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register("Increment", typeof(double), typeof(NumericUpDown), new PropertyMetadata(1.0));
        public double Increment
        {
            get { return (double)GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        #endregion

        #region FormatString

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register("FormatString", typeof(string), typeof(NumericUpDown), new PropertyMetadata("F0", OnStringFormatPropertyPropertyChanged));
        public string FormatString
        {
            get { return (string)GetValue(StringFormatProperty); }
            set { SetValue(StringFormatProperty, value); }
        }

        private static void OnStringFormatPropertyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown nud = d as NumericUpDown;
            nud.OnStringFormatChanged(e.OldValue.ToString(), e.NewValue.ToString());
        }

        protected virtual void OnStringFormatChanged(string oldValue, string newValue)
        {
            SyncTextAndValueProperties(NumericUpDown.TextProperty, Value);
        }

        #endregion //FormatString

        #region SelectAllOnGotFocus

        public static readonly DependencyProperty SelectAllOnGotFocusProperty = DependencyProperty.Register("SelectAllOnGotFocus", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));
        public bool SelectAllOnGotFocus
        {
            get { return (bool)GetValue(SelectAllOnGotFocusProperty); }
            set { SetValue(SelectAllOnGotFocusProperty, value); }
        }

        #endregion //SelectAllOnGotFocus

        #region ResetValueOnMiddleClick

        public static readonly DependencyProperty ResetValueOnMiddleClickProperty = DependencyProperty.Register("ResetValueOnMiddleClick", typeof(bool), typeof(NumericUpDown), new PropertyMetadata(false));

        public Boolean ResetValueOnMiddleClick
        {
            get { return (bool)GetValue(ResetValueOnMiddleClickProperty); }
            set { SetValue(ResetValueOnMiddleClickProperty, value); }
        }

        #endregion

        #region ResetDefaultValue

        public static readonly DependencyProperty ResetDefaultValueProperty = DependencyProperty.Register("ResetDefaultValue", typeof(string), typeof(NumericUpDown), new PropertyMetadata("1"));
        private Point? mousePosition;
        private bool hasMoved;
        private double deltaX;

        public string ResetDefaultValue
        {
            get { return (string)GetValue(ResetDefaultValueProperty); }
            set { SetValue(ResetDefaultValueProperty, value); }
        }

        #endregion

        #region IskeyboardFocusWithin

        //public static readonly DependencyProperty IsKeyboardFocusWithinProperty = DependencyProperty.Register("IsKeyboardFocusWithin", typeof(bool), typeof(InputBase), new PropertyMetadata(true));
        public new bool IsKeyboardFocusWithin
        {
            get { return TextBox.IsFocused; }
            //set { SetValue(IskeyboardFocusWithinProperty, value); }
        }

        #endregion //IskeyboardFocusWithin

        #endregion

        #region Constructors

        static NumericUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(NumericUpDown)));
            ValueTypeProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(typeof(double)));
            ValueProperty.OverrideMetadata(typeof(NumericUpDown), new FrameworkPropertyMetadata(default(Double)));
        }

        #endregion //Constructors

        #region Base Class Overrides

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //muahahha, that's so going to screw me one day
            SelectAllOnGotFocus = true;

            SetValidSpinDirection();

            if (ResetValueOnMiddleClick)
                TextBox.PreviewMouseDown += new MouseButtonEventHandler(TextBox_PreviewMouseDown);

            if (SelectAllOnGotFocus)
            {
                //in order to select all the text we must handle both the keybord (tabbing) and mouse (clicking) events
                TextBox.GotKeyboardFocus += OnTextBoxGotKeyBoardFocus;
                TextBox.PreviewMouseUp += OnTextBoxPreviewMouseUp;
            }

        }
        private void ResetValue()
        {
            TextBox.Text = Text = ResetDefaultValue;
        }

        protected override void OnValueChanged(object oldValue, object newValue)
        {
            SetValidSpinDirection();
            base.OnValueChanged(oldValue, newValue);
        }

        protected override object OnCoerceValue(object value)
        {
            if (value == null) return value;

            double val = Convert.ToDouble(value);

            if (val < Minimum)
            {
                return Minimum;
            }
            else if (val > Maximum)
            {
                return Maximum;
            }
            else
            {
                return value;
            }
        }

        protected override object ConvertTextToValue(string text)
        {
            object result = null;

            NumberFormatInfo info = NumberFormatInfo.GetInstance(CultureInfo.InvariantCulture);
            text = text.Replace(',', '.');

            try
            {
                result = FormatString.Contains("P") ? ParsePercent(text, ValueType, info) : ParseDataValue(text, ValueType, info);
            }
            catch
            {
                TextBox.Text = Text = ConvertValueToText(Value);
                return Value;
            }

            return result;
        }

        protected override string ConvertValueToText(object value)
        {
            return (Convert.ToDecimal(Value)).ToString(FormatString, CultureInfo.CurrentCulture);
        }

        protected override void OnIncrement()
        {
            double newValue = (double)(Convert.ToDecimal(Value) + (decimal)Increment);
            Value = ValueType != typeof(Double) ? Convert.ChangeType(newValue, ValueType) : newValue;
        }

        protected override void OnDecrement()
        {
            double newValue = (double)(Convert.ToDecimal(Value) - (decimal)Increment);
            Value = ValueType != typeof(Double) ? Convert.ChangeType(newValue, ValueType) : newValue;
        }

        #endregion //Base Class Overrides

        #region Event Handlers

        private void OnTextBoxGotKeyBoardFocus(object sender, RoutedEventArgs e)
        {
            TextBox.SelectAll();
        }

        void TextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                ResetValue();
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                mousePosition = e.GetPosition(this);
                hasMoved = false;
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            e.Handled = true;
            base.OnMouseLeftButtonDown(e);
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            e.Handled = true;

            if (IsMouseCaptured)
                this.ReleaseMouseCapture();
            //hasMoved = false;
            //base.OnMouseLeftButtonUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && mousePosition.HasValue)
            {
                if (!IsMouseCaptured)
                    CaptureMouse();

                var currentPos = e.GetPosition(this);
                var delta = currentPos - mousePosition.Value;

                deltaX += delta.X;

                if (deltaX < 5 && deltaX > -5)
                {
                    return;
                }
                else
                {
                    hasMoved = true;
                    double newValue = (double)(Convert.ToDecimal(Value) + (decimal)Increment * (decimal)(deltaX / 15));
                    Value = ValueType != typeof(Double) ? Convert.ChangeType(newValue, ValueType) : newValue;

                    deltaX = 0;
                    mousePosition = currentPos;
                }
            }
        }

        void OnTextBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released && !hasMoved)
            {
                if (!TextBox.IsKeyboardFocused)
                {
                    e.Handled = true;
                    TextBox.Focus();
                }
            }

            if (IsMouseCaptured)
                this.ReleaseMouseCapture();
        }

        #endregion //Event Handlers

        #region Methods

        /// <summary>
        /// Sets the valid spin direction based on current value, minimum and maximum.
        /// </summary>
        private void SetValidSpinDirection()
        {
            ValidSpinDirections validDirections = ValidSpinDirections.None;

            if (Convert.ToDouble(Value) < Maximum)
            {
                validDirections = validDirections | ValidSpinDirections.Increase;
            }

            if (Convert.ToDouble(Value) > Minimum)
            {
                validDirections = validDirections | ValidSpinDirections.Decrease;
            }

            if (Spinner != null)
            {
                Spinner.ValidSpinDirection = validDirections;
            }
        }

        #region Parsing

        private static object ParseDataValue(string text, Type dataType, NumberFormatInfo info)
        {
            try
            {
                if (typeof(double) == dataType)
                {
                    return ParseDouble(text, info);
                }
                else if (typeof(float) == dataType)
                {
                    return ParseFloat(text, info);
                }
                else if (typeof(byte) == dataType || typeof(sbyte) == dataType ||
                         typeof(short) == dataType || typeof(ushort) == dataType || typeof(Int16) == dataType ||
                         typeof(int) == dataType || typeof(uint) == dataType || typeof(Int32) == dataType ||
                         typeof(long) == dataType || typeof(ulong) == dataType || typeof(Int64) == dataType)
                {
                    return ParseWholeNumber(text, dataType, info);
                }
                else if (typeof(decimal) == dataType)
                {
                    return ParseDecimal(text, info);
                }
                else
                {
                    throw new ArgumentException("Type not supported");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static double ParseDouble(string text, NumberFormatInfo info)
        {
            return double.Parse(text, NumberStyles.Any, info);
        }

        private static float ParseFloat(string text, NumberFormatInfo info)
        {
            double result = double.Parse(text, NumberStyles.Any, info);
            return (float)result;
        }

        private static decimal ParseDecimal(string text, NumberFormatInfo info)
        {
            return decimal.Parse(text, NumberStyles.Any, info);
        }

        private static object ParseWholeNumber(string text, Type dataType, NumberFormatInfo info)
        {
            decimal result = decimal.Parse(text, NumberStyles.Any, info);
            return Convert.ChangeType(result, dataType, info);
        }

        private static object ParsePercent(string text, Type dataType, NumberFormatInfo info)
        {
            text = text.Replace(info.PercentSymbol, null);

            decimal result = decimal.Parse(text, NumberStyles.Any, info);
            result = result / 100;

            return Convert.ChangeType(result, dataType, info);
        }

        #endregion

        #endregion //Methods
    }
}
