﻿using DpConnect.Example.TechParamApp.ViewModel;
using System;
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

namespace DpConnect.Example.TechParamApp.View
{
    /// <summary>
    /// Interaction logic for OpcUaConnectionSettingsView.xaml
    /// </summary>
    public partial class OpcUaConnectionConfigurationView : UserControl, IConnectionConfigurationView

    {
        public OpcUaConnectionConfigurationView()
        {
            InitializeComponent();
            DataContext = new OpcUaConnectionConfigurationViewModel(new OpcUa.OpcUaConnectionConfiguration());           
        }

        public IDpConnectionConfiguration Configuration => (DataContext as OpcUaConnectionConfigurationViewModel).Config;

        public string ConnectionName { get; set; } = "OpcUa";

        public UIElement View => this;
    }
}
