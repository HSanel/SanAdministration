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

namespace San_Administration.Host
{
    public class GuiHost  : NotifyPropertyChanged
    {
        string pluginPath = @"C:\Users\sMBsahasi\Arbeitsplatz\Arbeit\repos\SanAdministration\San Administration\bin\Debug\PlugIns";
        PluginLoader plugLoader;
        List<IPlugin> plugins;
        Page currentView;
        Stack<IInputElement> focusedElements;


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

        public Page CurrentView
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
        public ICommand OpenCommand { get; private set; }
        private Logic.UndoRedoController undoRedoController;
        private void InitializeViewModel()
        {
            focusedElements = new Stack<IInputElement>();
            UndoCommand = new CustomCommand(UndoHandler);
            RedoCommand = new CustomCommand(RedoHandler);
            OpenCommand = new CustomCommand(OpenHandler);
            SaveCommand = new CustomCommand(SaveHandler);

            undoRedoController = new Logic.UndoRedoController();
        }

        private void Controll_GotFocus(object sender, EventArgs e)
        {

            undoRedoController.Push((FrameworkElement)sender);
        }

        private void OpenHandler()
        {
            
        }

        private void SaveHandler()
        {

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
