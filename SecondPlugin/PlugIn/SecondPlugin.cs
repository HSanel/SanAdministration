using System;
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
                return "First Plugin";
            }
        }

        public string Description
        {
            get
            {
                return "Das ist nur ein Beispiel";
            }
        }


        public IPluginPage View
        {
            get;
            set;
        }

        public string Category
        {
            get
            {
                return "First Category";
            }
        }


        public void OnLoad()
        {
            viewModel = new ViewModel();
            PagePlug pagePlug = new PagePlug(viewModel);
            pagePlug.StackUpdated += ControlChanged;
            //View = pagePlug;


           
        }

        public void ControlChanged(object sender, EventArgs e)
        {
            ControlChangedTrigger(sender, e);
        }

        public Hashtable OnSave()
        {
            if(View != null)
            {
                return View.SaveData();
            }

            return null;
        }

        public void OnOpen(object inData)
        {
            View.OpenData(inData);
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
            
        }


    }
}
