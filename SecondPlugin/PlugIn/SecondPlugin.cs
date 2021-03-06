﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using San.Base.Plugin;
using SecondPlugin;

namespace SecondPlugin.PlugIn
{
    public class SecondPlugin : IPlugin
    {
        public event EventHandler ControlChangedTrigger;
        private ViewModel viewModel;

        public string Title
        {
            get
            {
                return "Second Plugin";
            }
        }

        public string Description
        {
            get
            {
                return "Das ist nur ein Beispiel";
            }
        }


        public IPluginPage MainView
        {
            get;
            set;
        }

        public string Category
        {
            get
            {
                return "Second Category";
            }
        }

        public Page ConfigurationView
        {
            get;
            set;
        }

        public void OnLoad()
        {
            viewModel = new ViewModel();
            MainPagePlug pagePlug = new MainPagePlug(viewModel);
            pagePlug.StackUpdated += ControlChanged;
            MainView = pagePlug;

            ConfigurationPage configurationPage = new ConfigurationPage(viewModel);
            ConfigurationView = configurationPage;
        }

        public void ControlChanged(object sender, EventArgs e)
        {
            ControlChangedTrigger(sender, e);
        }

        public Hashtable OnSave()
        {
            if(MainView != null)
            {
                return MainView.SaveData();
            }

            return null;
        }

        public void OnOpen(object inData)
        {
            MainView.OpenData(inData);
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
            
        }


    }
}
