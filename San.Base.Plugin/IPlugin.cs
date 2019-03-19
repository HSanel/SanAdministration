﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace San.Base.Plugin
{
    public interface IPlugin
    {
        string Title { get; }
        string Category { get; }
        string Description { get; }
        Page View { get; set; }
        event EventHandler ControlChangedTrigger;

        void OnLoad();
        void OnResume();
        void OnPause();
        void ControlChanged(object sender, EventArgs e);
    }
}
