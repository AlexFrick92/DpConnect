using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.ExampleWorker;
using DpConnect.ExampleWorker.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class TechParamReaderConfiguratorViewModel : BaseViewModel, ITechParamConfiguratorViewModel
    {        
        public TechParamReaderConfiguratorViewModel()
        {
            List<NamedDpConfigSettingViewModel> namedDpConfigSettingViewModels = new List<NamedDpConfigSettingViewModel>();

            namedDpConfigSettingViewModels.Add(
                new NamedDpConfigSettingViewModel() { DpName = "TechParam" }
                );


            Settings = namedDpConfigSettingViewModels;
        }
        public string ConfiguratorName => "FloatParameter";

        public IEnumerable<NamedDpConfigSettingViewModel> Settings { get; private set; }

        public ITechParamViewModel CreateTechParameter(IDpBinder binder, IDpWorkerManager workerManager, IDpConnection connection)
        {
            List<IDpConfiguration> configs = new List<IDpConfiguration>();

            foreach(var setting in Settings)
            {
                configs.Add(setting.SourceConfigurator.CreateConfiguration(setting.DpName));                
            }

            var worker = workerManager.CreateWorker<TechParamReader>();            

            Settings.First().SourceConfigurator.BindProperties(worker, configs, binder, connection);
            
            TechParamReaderViewModel viewModel = new TechParamReaderViewModel(worker as TechParamReader);
            
            return viewModel;
        }        
    }
}
