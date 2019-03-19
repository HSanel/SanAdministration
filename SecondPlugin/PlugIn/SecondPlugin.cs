using System;
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


        public Page View
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
            PagePlug pagePlug = new PagePlug();
            pagePlug.StackUpdated += ControlChanged;
            View = pagePlug;

           
        }

        public void ControlChanged(object sender, EventArgs e)
        {
            ControlChangedTrigger(sender, e);
        }

        public void OnPause()
        {
            
        }

        public void OnResume()
        {
            
        }
    }
}
