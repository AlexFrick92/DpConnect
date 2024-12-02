using DpConnect.Example.TechParamApp.View;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            App app = new App();
            app.Run();
        }
    }
}
