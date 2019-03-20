using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
using San.Base;
using San.Base.Memory;


namespace SecondPlugin
{
    /// <summary>
    /// Interaktionslogik für PagePlug.xaml
    /// </summary>
    public partial class PagePlug : Page
    {
        public event EventHandler StackUpdated;
        //next Page
        public PagePlug(ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lbBsp.Background = Brushes.Red;
            //SaveData();
        }
        //--------------------------------------------------------------------



        private void Control_Changed(object sender, RoutedEventArgs e)
        {
            //var a = (FrameworkElement)sender;

            //if (DataContext != null && StackUpdated != null)
            //    if (((bool)a.DataContext))
            //        StackUpdated(sender, e);

            //a.DataContext = true;
        }

        //    //------------------------
        //    ProjectMemory projectMemory = new ProjectMemory();
        //    public Hashtable SaveData()
        //    {
        //        projectMemory.ClearOutputMemory();
        //        projectMemory.SaveData(this);
        //        //nextPage.SaveData(projectMemory);
        //        //projectMemory.SerilizeData(@"C:\Users\sMBsahasi\Desktop\Pr\data.txt");
        //        return projectMemory.GetOutData();
        //    }

        //    public void OpenData(object inData)
        //    {
        //        projectMemory.ClearInputMemory();
        //        //projectMemory.DeserilizeData(@"C:\Users\sMBsahasi\Desktop\Pr\data.txt");
        //        projectMemory.SetInData(inData);
        //        projectMemory.ReadData(this);
        //        //nextPage.ReadData(projectMemory);

        //    }
    }
}
