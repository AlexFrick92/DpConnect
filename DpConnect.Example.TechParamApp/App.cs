using DpConnect.Example.TechParamApp.View;
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
            MainView view = new MainView();
            view.Show();

            base.Run();
        }
    }
}
