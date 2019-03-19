using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using San.Base.Plugin;

namespace San_Administration
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    public partial class App : Application
    {
        public App() {}

     

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
        }
    }
}
