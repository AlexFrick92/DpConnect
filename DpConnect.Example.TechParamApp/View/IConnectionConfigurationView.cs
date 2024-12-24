using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DpConnect.Example.TechParamApp.View
{
    public interface IConnectionConfigurationView
    {
        string ConnectionName { get; set;  }

        IDpConnectionConfiguration Configuration { get; }

        UIElement View { get; }
    }
}
