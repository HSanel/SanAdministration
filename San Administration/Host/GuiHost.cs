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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace San_Administration.Host
{
    public class GuiHost  : NotifyPropertyChanged
    {
        string pluginPath = @"C:\Users\sMBsahasi\Arbeitsplatz\Arbeit\repos\SanAdministration\San Administration\bin\Debug\PlugIns";
        PluginLoader plugLoader;
        PluginInstaller plugInstaller;
        List<IPlugin> plugins;
        IPluginPage currentView;
        Page configurationView;
        Stack<IInputElement> focusedElements;
        string path = "";
        string appDataPath = "";
        ObservableCollection<Model.PluginData> _pluginMenu;

        public ICommand SelectCategoryCommand { get; private set; }
        public ICommand SelectPlugin { get; private set; }
        public ICommand InstallPluginCommand { get; private set; }

        public GuiHost()
        {
            InitializeViewModel();
            plugLoader = new PluginLoader();
            plugInstaller = new PluginInstaller(pluginPath);
            plugInstaller.ProgressUpdated += ProgressUpdated_Handler;
            plugInstaller.ExtractedInfoFile += ExtractedInfoFile_Handler;
            plugins = plugLoader.LoadPlugins(pluginPath).ToList<IPlugin>();
            _pluginMenu = new ObservableCollection<Model.PluginData>();
            SelectPlugin = new CustomCommand<int>(SelectPluginHandler);
            SelectCategoryCommand = new CustomCommand<string>(SelectCategoryHandler);
            InstallPluginCommand = new CustomCommand(InstallPluginHandler);
            int id = 0;
            undoRedoController = new Logic.UndoRedoController();

            foreach (IPlugin plug in plugins)
            {
                undoRedoController.AddPlug = new List<FrameworkElement>();
                plug.ControlChangedTrigger += Controll_GotFocus;
                PluginMenu.Add(new Model.PluginData(plug.Title, id));
                fillCategorieAndPluginList(plug.Category, plug.Title);
                id++;
            }

            if(plugins.Count != 0)
            {
                currentView = plugins[0].MainView;
                configurationView = plugins[0].ConfigurationView;
                Title = plugins[0].Title;
                undoRedoController.SelectPlugin(0);
            }
            Categories.Add("Alle Plugins");
            categoryList.Add("Alle Plugins", AllPluginTitles);
        }

        Hashtable categoryList = new Hashtable();
        ObservableCollection<string> AllPluginTitles = new ObservableCollection<string>();
        ObservableCollection<string> _categories = new ObservableCollection<string>();

        string _title;
        string _slectedCategory;
        int _installationProgress;
        bool _progressBarVisible = false;
        bool _installMessageVisible = false;

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

        public Page CurrentConfigurationView
        {
            get
            {
                return configurationView;
            }
            set
            {
                configurationView = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Model.PluginData> PluginMenu
        {
            get
            {
                return _pluginMenu;
            }
            private set
            {
                _pluginMenu = value;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public int InstallationProgress
        {
            get
            {
                return _installationProgress;
            }
            set
            {
                _installationProgress = value;
                OnPropertyChanged();
            }
        }

        public bool ProgressBarVisible
        {
            get
            {
                return _progressBarVisible;
            }
            set
            {
                _progressBarVisible = value;
                OnPropertyChanged();
            }
        }

        public bool InstallMessageVisible
        {
            get
            {
                return _installMessageVisible;
            }
            set
            {
                _installMessageVisible = value;
                OnPropertyChanged();
            }
        }

        private void SelectPluginHandler(int id)
        {
            for(int i =0; i < plugins.Count; i++)
            {
                if(i == id)
                {
                    CurrentView = plugins[i].MainView;
                    CurrentConfigurationView = plugins[i].ConfigurationView;
                    Title = plugins[i].Title;
                    undoRedoController.SelectPlugin(i);
                    return;
                }
            }
        }
  
        public ObservableCollection<string> Categories
        {
            get
            {
                return _categories;
            }
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        private void SelectCategoryHandler(string categoryName)
        {
            _slectedCategory = categoryName;
            OnPropertyChanged("PluginTitles");
        }
       
        public ObservableCollection<string> PluginTitles
        {
            get
            {
                if (_slectedCategory != null)
                    return (ObservableCollection<string>)categoryList[_slectedCategory];
                else
                    return AllPluginTitles;
            }
        }
        
        private void fillCategorieAndPluginList(string category, string pluginTitle)
        {
            if(Categories.Contains(category))
            {
                ((ObservableCollection<string>)categoryList[category]).Add(pluginTitle);
            }
            else
            {
                Categories.Add(category);
                ObservableCollection<string> list = new ObservableCollection<string>();
                list.Add(pluginTitle);
                categoryList.Add(category, list);
            }
            AllPluginTitles.Add(pluginTitle);
        }


        private void InstallPluginHandler()
        {
            string plugZipPath = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".zip";
            dlg.Filter = "Plugin |*.zip";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                appDataPath = Path.Combine(appDataPath, "San Administration");
                if(!Directory.Exists(appDataPath))
                {
                    Directory.CreateDirectory(appDataPath);
                }

                InstallationProgress = 0;
                ProgressBarVisible = true;
                plugZipPath = dlg.FileName;
                plugInstaller.AsynchronInstall(plugZipPath, appDataPath);               
            }
        }
    
        private void ProgressUpdated_Handler(object sender, EventArgs e)
        {
            InstallationEventArgs eArg = (InstallationEventArgs)e;
            InstallationProgress = (int)(10.0 * ((float)eArg.ActualEntryNumber / (float)eArg.EntrieCount));

            if(eArg.ActualEntryNumber == eArg.EntrieCount)
            {
                ProgressBarVisible = false;
                InstallMessageVisible = true;
            }   
        }

        private void ExtractedInfoFile_Handler(object sender, EventArgs e)
        {
            InstallationEventArgs eArg = (InstallationEventArgs)e;
            if(appDataPath != "")
            {
                using (FileStream stream = File.Open(appDataPath, FileMode.Open))
                {
                    //Hier wird anschließend kontrolliert ob das Plugin und alle benötigten .dll Dateien bereits installiert sind
                }
            }
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
