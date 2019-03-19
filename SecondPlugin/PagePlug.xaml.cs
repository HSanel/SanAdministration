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
using System.Windows.Navigation;
using System.Windows.Shapes;
using San.Base.Plugin;

namespace SecondPlugin
{
    /// <summary>
    /// Interaktionslogik für PagePlug.xaml
    /// </summary>
    public partial class PagePlug : Page
    {
        public event EventHandler StackUpdated;
        public PagePlug()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lbBsp.Background = Brushes.Red;
        }

        private void Control_Changed(object sender, RoutedEventArgs e)
        {
            var a = (FrameworkElement)sender;

            if(DataContext != null && StackUpdated != null)
                if (((bool)a.DataContext))           
                  StackUpdated(sender, e);

            a.DataContext = true;
        }
    }
}
