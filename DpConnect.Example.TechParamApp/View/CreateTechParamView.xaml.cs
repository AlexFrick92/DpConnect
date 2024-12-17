using DpConnect.Example.TechParamApp.ViewModel;
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
using System.Windows.Shapes;

namespace DpConnect.Example.TechParamApp.View
{
    /// <summary>
    /// Interaction logic for CreateTechParamView.xaml
    /// </summary>
    public partial class CreateTechParamView : Window
    {
        public CreateTechParamView(CreateTechParamViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}
