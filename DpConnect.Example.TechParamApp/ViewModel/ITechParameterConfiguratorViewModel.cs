using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public interface ITechParameterConfiguratorViewModel
    {
        string ConfiguratorName { get; }

        IEnumerable<NamedDpConfigSettingViewModel> Settings { get; }        

        void CreateTechParameter();
    }
}
