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
    public partial class MainPagePlug : Page, IPluginPage
    {

        //next Page
        public MainPagePlug(ViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
        }

        //--------Beispiel Button----------------------
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            lbBsp.Background = Brushes.Red;
            //SaveData();
        }
        //------------------Sachen die in eine Page immer reinkommen--------------------------------------------------
        public event EventHandler StackUpdated;
        static ProjectMemory projectMemory = new ProjectMemory();

        public MainPagePlug NextPage
        {
            get { return (MainPagePlug)GetValue(NextPageProperty); }
            set { SetValue(NextPageProperty, value); }
        }

        public static readonly DependencyProperty NextPageProperty =
            DependencyProperty.Register(nameof(NextPage), typeof(MainPagePlug), typeof(MainPagePlug), new PropertyMetadata(default(MainPagePlug)));

        public Hashtable SaveData()
        {
            projectMemory.ClearOutputMemory();
            projectMemory.SaveData(this);

            NextPage?.SaveData();

            return projectMemory.GetOutData();
        }

        private void Control_Changed(object sender, RoutedEventArgs e)
        {
            FrameworkElement control = (FrameworkElement)sender;
            if (control.DataContext != null && StackUpdated != null)
                if (((bool)control.DataContext))
                    StackUpdated(control, e);

            control.DataContext = true;
        }

        public void OpenData(object inData)
        {
            projectMemory.ClearInputMemory();
            projectMemory.SetInData(inData);
            readData();
        }

        void readData()
        {
            projectMemory.ReadData(this);
            NextPage?.readData();
        }
    }
}
