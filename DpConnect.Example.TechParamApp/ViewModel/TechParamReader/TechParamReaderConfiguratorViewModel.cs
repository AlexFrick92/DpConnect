using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.ExampleWorker;
using DpConnect.ExampleWorker.Console;

using System.Collections.Generic;
using System.Linq;

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

        public ITechParamViewModel CreateTechParameter(IDpWorkerManager workerManager)
        {
            List<IDpConfiguration> configs = new List<IDpConfiguration>();

            foreach(var setting in Settings)
            {
                configs.Add(setting.SourceConfigurator.CreateConfiguration(setting.DpName));                
            }

            var worker = workerManager.CreateWorker<TechParamReader>();

            connection.BindProperties(worker, configs);
            
            TechParamReaderViewModel viewModel = new TechParamReaderViewModel(worker as TechParamReader);
            
            return viewModel;
        }

        IConnectionViewModel connection;

        public void SetConnection(IConnectionViewModel connection)
        {
            this.connection = connection;

            foreach (var setting in Settings)
            {
                setting.SourceConfigurator = connection.SourceConfigurator;
            }
        }
    }
}
