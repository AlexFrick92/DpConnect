using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;

using System.Linq;

namespace DpConnect.DIBuilder
{
    internal class DpBinder : IDpBinder
    {
        private readonly ILogger _logger;
        private readonly IDpProviderConfigurator _providerConfigurator;
        private readonly IIoCContainer _container;  
        public DpBinder(ILogger logger, IDpProviderConfigurator providerConfigurator, IIoCContainer container)
        {            
            _logger = logger;
            _container = container;
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

        public T CreateProcessor<T>(string name) where T : IDpProcessor
        {
            //Здесь мы должны достать из контейнера фабрику, затем 
            //Фабрика создаст нам объект с заданным именем
            //Затем мы этот объект свяжем с провайдером!

            //А вообще, это позволит создать процессор уже со связанными точками в будущем. Через конструктор или еще через какую-то фигню. В общем подумаем

            IDpProcessorFactory<T> factory =  _container.Resolve<IDpProcessorFactory<T>>(); //Так легко?!?!? И даже без рефлексии???!??!

            T processor = factory.Create(name);

            Bind(processor);

            processor.OnDpInitialized();

            return processor;

        }
    }
}
