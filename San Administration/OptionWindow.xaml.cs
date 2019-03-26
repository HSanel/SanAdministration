using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace San_Administration
{
    /// <summary>
    /// Interaktionslogik für OptionWindow.xaml
    /// </summary>
    public partial class OptionWindow : Window
    {
        Window mainWindow;
        public OptionWindow(Window mainWindow)
        {
            this.mainWindow = mainWindow;
            DataContext = mainWindow.DataContext;
            InitializeComponent();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Category_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ((RadioButton)sender).IsChecked = true;
        }

        private void RadioButton_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
