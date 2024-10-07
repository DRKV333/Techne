using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;

namespace TestApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Regex regex = new Regex(@"^\d+$");
        
        public Window1()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            grid.DataContext = new Person() { FirstName = "Ed", LastName = "Foh", Age = 25, Gender='M' };
            comboBoxBehavior.ItemsSource = ReferenceData.GetGenders();
        }

        private void AgeEditableBehavior_OnSaving(object arg1, EditableBehaviorLibrary.OnSaveEventArgs arg2)
        {
            if (string.IsNullOrEmpty(arg2.NewValue) || !regex.IsMatch(arg2.NewValue))
            {
                arg2.Cancel = true;
                arg2.ErrorMessage = "Please enter a valid age";
            }
        }

        private void NameEditableBehavior_OnSaving(object arg1, EditableBehaviorLibrary.OnSaveEventArgs arg2)
        {
            if (string.IsNullOrEmpty(arg2.NewValue))
            {
                arg2.Cancel = true;
                arg2.ErrorMessage = "Please enter a value";
            }
        }

    }
}
