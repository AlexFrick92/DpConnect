using DpConnect.Example.TechParamApp.View;
using DpConnect.Example.TechParamApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp
{
    internal class App : System.Windows.Application
    {
        public App()
        {
            
        }
        public new void Run()
        {


            MainViewModel mainViewModel = new MainViewModel();
            MainView view = new MainView(mainViewModel);
            view.Show();

            base.Run();
        }
    }
}
