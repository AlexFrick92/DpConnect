using DpConnect.Building;
using DpConnect.Configuration;
using DpConnect.Connection;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaSourceConfiguratorViewModel<TWorker> : BaseViewModel, ISourceConfiguratorViewModel
        where TWorker : IDpWorker
    {
        OpcUaConnection connection;

        public OpcUaSourceConfiguratorViewModel(OpcUaConnection connection)
        {
            this.connection = connection;

            var pars = new List<NamedConfigSettingViewModel>();
            var nodeId = new NamedConfigSettingViewModel();
            nodeId.Name = "NodeId";

            pars.Add(nodeId);

            //Найдем все свойства воркера
            var settings = new List<NamedDpConfigSettingViewModel>();

            foreach (var prop in typeof(TWorker).GetProperties().Where(p => p.PropertyType.IsGenericType).Where(p =>
                p.PropertyType.GetGenericTypeDefinition() == typeof(IDpAction<>)
                || p.PropertyType.GetGenericTypeDefinition() == typeof(IDpValue<>)))
            {
                var config = new NamedDpConfigSettingViewModel(prop.Name, pars);
                settings.Add(config);
            }

            DpPropSettings = settings;

            Console.WriteLine(settings.Count);
        }        

        public IEnumerable<NamedDpConfigSettingViewModel> DpPropSettings { get; private set; }

        public void Bind(IDpBinder binder, IDpWorkerManager workerManager)
        {
            var dpConfigList = new List<DpConfiguration<OpcUaDpValueSourceConfiguration>>();

            foreach (var dp in DpPropSettings)
            {
                var dpConfig = new DpConfiguration<OpcUaDpValueSourceConfiguration>();
                dpConfig.PropertyName = dp.DpName;

                //Это NodeId. Необходимо придумать, как обратно конвертировать. Может нужно использовать dictionary<string, object>
                dpConfig.SourceConfiguration.NodeId = dp.SourceSettings.First().Value.ToString();

                dpConfigList.Add(dpConfig);
            }

            IDpWorker worker = workerManager.CreateWorker<TWorker>();

            binder.Bind(worker, connection, dpConfigList);

        }
    }
}
