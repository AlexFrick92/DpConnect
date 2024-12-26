using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class NamedDpConfigSettingViewModel : BaseViewModel
    {
        public string DpName { get; set; }

        ISourceConfiguratorViewModel sourceConfigurator;
        public ISourceConfiguratorViewModel SourceConfigurator
        {
            get => sourceConfigurator;
            set
            {
                sourceConfigurator = value;
                OnPropertyChanged(nameof(SourceConfigurator));
            }
        }
    }
}
