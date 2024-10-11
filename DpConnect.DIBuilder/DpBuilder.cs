using DpConnect.Interface;
using Promatis.Core;
using Promatis.Core.Logging;

using System.Linq;

namespace DpConnect.DIBuilder
{
    internal class DpBuilder : IDpBuilder
    {
        private readonly ILogger _logger;
        private readonly IDpProviderConfigurator _providerConfigurator;
        private readonly IIoCContainer _container;  
        public DpBuilder(ILogger logger, IDpProviderConfigurator providerConfigurator, IIoCContainer container)
        {            
            _logger = logger;
            _container = container;
            _logger.Info("Создан DpBuilder");
            _providerConfigurator = providerConfigurator;
        }
        public void Bind(IDpProcessor processor)
        {
            _logger.Info($"Привязка для процессора" + processor.Name);

            _providerConfigurator.ConfigureProviders(new System.Xml.Linq.XDocument());
            

            //Как будто, здесь нужно получить из ProviderConfigurator нужный провайдер. 
            //Можно сначала загрузить конфигурацию в провайдер.
            //И он создаст список нужных провайдеров. Однако, в таком случае, мы должны будем туда конфиг провайдеров загрузить.
            //И он сам должен будет разбираться с xml.
            //Либо, мы можем создавать провайдеры через, например ProviderConfigurator.Create<OpcUaProvider>("Name", config)
            //config - конфигурация OpcUaProvider, Эта конфигурация должна быть как-то связана с OpcUaProvider. Как это сделать?
            //Тогда смысла в этом конфигураторе нету, если мы ему все данные даём, ему останется только вызвать new OpcUaProvider(name, config)
            
            
            //Думаю, что выбор такой: Либо избавится от ProviderConfigurator, и создавать провайдеры здесь. Либо оставить как есть - передать конфигурацию в виде xml в провайдер,
            //пусть он сразу всё создаст, а затем получить от него нужный провайдер по имени. Это так же позволит централизованно управлять запуском и остановом всех провайдеров.            
            
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
