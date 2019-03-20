using San.Base.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using San.Base.Plugin;

namespace San.Base
{
    public class PageBase : Page, IPluginPage
    {
        public event EventHandler StackUpdated;
        static ProjectMemory projectMemory = new ProjectMemory();

        public PageBase NextPage
        {
            get { return (PageBase)GetValue(NextPageProperty); }
            set { SetValue(NextPageProperty, value); }
        }

        public static readonly DependencyProperty NextPageProperty =
            DependencyProperty.Register(nameof(NextPage), typeof(PageBase), typeof(PageBase), new PropertyMetadata(default(PageBase)));



        private void StackUpdatedTrigger(FrameworkElement control, RoutedEventArgs e)
        {
            if (DataContext != null && StackUpdated != null)
                if (((bool)control.DataContext))
                    StackUpdated(control, e);

            control.DataContext = true;
        }

        public Hashtable SaveData()
        {
            projectMemory.ClearOutputMemory();
            projectMemory.SaveData(this);

            NextPage?.SaveData();

            return projectMemory.GetOutData();
        }

        public void OpenData(object inData)
        {
            projectMemory.ClearInputMemory();
            projectMemory.SetInData(inData);
            readData();
        }

        private void readData()
        {
            projectMemory.ReadData(this);
            NextPage?.readData();
        }
    }
}
