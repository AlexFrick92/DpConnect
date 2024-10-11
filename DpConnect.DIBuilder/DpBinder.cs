using DpConnect.Interface;
using Promatis.Core.Logging;

using System.Linq;

namespace DpConnect.DIBuilder
{
    internal class DpBinder : IDpBinder
    {
        private readonly ILogger _logger;
        private readonly IDpProviderConfigurator _providerConfigurator;
        public DpBinder(ILogger logger, IDpProviderConfigurator providerConfigurator)
        {            
            _logger = logger;
            _logger.Info("Создан DpBinder");
            _providerConfigurator = providerConfigurator;
        }
        public void Bind(IDpProcessor processor)
        {
            _logger.Info($"Привязка для процессора" + processor.Name);

            _providerConfigurator.ConfigureProviders(new System.Xml.Linq.XDocument());
            var prov = _providerConfigurator.ConfiguredProviders.FirstOrDefault();

            var processorProps = processor.GetType().GetProperties();
            var prop = processorProps.FirstOrDefault(x => x.Name == "BoolNode");



            

            DpValue<bool> dpValue = new DpValue<bool>();

            prov.RegisterDp<bool>(dpValue, new System.Xml.Linq.XDocument());
            
            prop.SetValue(processor, dpValue);
                
        }
    }
}
