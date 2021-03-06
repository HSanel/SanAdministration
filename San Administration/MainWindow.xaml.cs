﻿using System;
using System.Collections.Generic;
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


namespace San_Administration
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            
            DataContext = new Host.GuiHost();
            InitializeComponent();
        }

        private void RadioButton_Loaded(object sender, RoutedEventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            Model.PluginData pluginData = (Model.PluginData)rb.DataContext;
            if (pluginData.ID == 0)
                rb.IsChecked = true;
        }

        private void Einstellungen_Click(object sender, RoutedEventArgs e)
        {
            OptionWindow optionWindow = new OptionWindow(this);
            optionWindow.ShowDialog();
        }
    }
}
