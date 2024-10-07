/* Copyright (c) Alexander S. Brunner
 *
 * This source is subject to the Microsoft Public License.
 * See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
 * All other rights reserved.
 */
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Techne.Models;
using Techne.Plugins;

namespace Techne
{
    /// <summary>
    /// Interaction logic for AnimationAxisControl.xaml
    /// </summary>
    public partial class AnimationAxisControl : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(AnimationDataModel), typeof(AnimationAxisControl), new UIPropertyMetadata(null, DataPropertyChanged));

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(String), typeof(AnimationAxisControl), new UIPropertyMetadata("Axis"));

        public AnimationAxisControl()
        {
            InitializeComponent();
        }

        public AnimationDataModel Data
        {
            get { return (AnimationDataModel)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public String Header
        {
            get { return (String)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public double Angle
        {
            get { return Data == null ? 0 : Data.Angle; }
            set
            {
                Data.Angle = value;
                var tmp = Data;
                Data = null;
                Data = tmp;
            }
        }

        public double Duration
        {
            get { return Data == null ? 0 : Data.Duration; }
            set
            {
                Data.Duration = value;
                var tmp = Data;
                Data = null;
                Data = tmp;
            }
        }

        public AnimationType AnimationType
        {
            get { return Data.AnimationType; }
        }

        public int AnimationTypeIndex
        {
            get { return Data == null ? -1 : (int)AnimationType; }
            set
            {
                Data.AnimationType = (AnimationType)value;
                var tmp = Data;
                Data = null;
                Data = tmp;
            }
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        protected static void DataPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var o = ((AnimationAxisControl)d);
            o.OnPropertyChanged("Header");
            o.OnPropertyChanged("AnimationTypeIndex");
            o.OnPropertyChanged("Duration");
            o.OnPropertyChanged("Angle");
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

