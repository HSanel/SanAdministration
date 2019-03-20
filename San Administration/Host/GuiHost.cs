using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using San.Base.Plugin;
using System.Windows.Controls;
using San.Base;
using System.Windows.Input;
using System.Windows;
using System.Collections;
using San.Base.Memory;


namespace San_Administration.Host
{
    public class GuiHost  : NotifyPropertyChanged
    {
        string pluginPath = @"C:\Users\sMBsahasi\Arbeitsplatz\Arbeit\repos\SanAdministration\San Administration\bin\Debug\PlugIns";
        PluginLoader plugLoader;
        List<IPlugin> plugins;
        IPluginPage currentView;
        Stack<IInputElement> focusedElements;
        string path = "";


        public GuiHost()
        {
            InitializeViewModel();
            plugLoader = new PluginLoader(pluginPath);
            plugins = plugLoader.LoadPlugins().ToList<IPlugin>();
            List<ListViewItem> _pluginsItems = new List<ListViewItem>();
            foreach(IPlugin plug in plugins)
            {
                ListViewItem item = new ListViewItem();
                item.Content = plug.Title;
                plug.ControlChangedTrigger += Controll_GotFocus;
                _pluginsItems.Add(item);
            }
  
            currentView = plugins[0].View;
            Title = plugins[0].Title;
           
        }

        public IPluginPage CurrentView
        {
            get
            {
                return currentView;
            }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public List<ListViewItem> MenuItems
        {
            get;
            private set;
        }

        string title;
        public string Title
        {
            get { return title; }
            set { title = value; OnPropertyChanged(); }
        }

        //------------------------ViewModel----------------------------------------------

        public ICommand UndoCommand { get; private set; }
        public ICommand RedoCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        private Logic.UndoRedoController undoRedoController;
        Hashtable outMemory;
        Hashtable inMemory;
        DataJsonSerilizer serilizer;
        private void InitializeViewModel()
        {
            serilizer = new DataJsonSerilizer();
            outMemory = new Hashtable(); 
            focusedElements = new Stack<IInputElement>();
            UndoCommand = new CustomCommand(UndoHandler);
            RedoCommand = new CustomCommand(RedoHandler);
            OpenCommand = new CustomCommand(OpenHandler);
            SaveCommand = new CustomCommand(SaveHandler);
            SaveAsCommand = new CustomCommand(SaveAsHandler);

            undoRedoController = new Logic.UndoRedoController();
        }

        private void Controll_GotFocus(object sender, EventArgs e)
        {

            undoRedoController.Push((FrameworkElement)sender);
        }

        private void OpenHandler()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".san";
            dlg.Filter = "San-Administration File |*.san";

            Nullable<bool> result = dlg.ShowDialog();
            if(result == true)
            {
                path = dlg.FileName;
                serilizer.DeserilizeAllData(path, ref inMemory);

                foreach (IPlugin plug in plugins)
                {
                    plug.OnOpen(inMemory[plug.Title]);
                }
            }
        }

        private void SaveHandler()
        {
            if(path == "")
            {
                SaveAsHandler();
            }
            else
            {
                outMemory.Clear();
                foreach (IPlugin plug in plugins)
                {
                    outMemory.Add(plug.Title, plug.OnSave());
                }
                serilizer.SerilizeAllData(path, ref outMemory);
            }
        }

        private void SaveAsHandler()
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "SanDocument";
            dlg.DefaultExt = ".san";
            dlg.Filter = "San-Administration File |*.san";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                path = dlg.FileName;
                foreach (IPlugin plug in plugins)
                {
                    outMemory.Add(plug.Title, plug.OnSave());
                }
                serilizer.SerilizeAllData(path, ref outMemory);
            }
        }

        private void RedoHandler()
        {
            undoRedoController.Redo();
        }

        public void UndoHandler()
        {
            undoRedoController.Undo();
        }
    }
}
