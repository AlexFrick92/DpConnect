[Назад](readme.md)


## Данные как технологические параметры

![](ExampleWeightComm.drawio.svg)

```C#

var client = OpcUaClient("10.10.10.95:4840");

WeightProcessing(client);


#DeviceExchangeManager:

    public void Start()
    {
        string text = "[DeviceExchange." + MethodBase.GetCurrentMethod()?.Name + "]";
        List<Plc> listPlcWoCollections = GetListPlcWoCollections();
        Logger.Trace(text + " " + $"Получение из базы списка PLC, записей:{listPlcWoCollections?.Count}".GetStatString(DateTime.Now));
        if (listPlcWoCollections == null || listPlcWoCollections.Count == 0)
        {
            Logger.Info(text + " нет устройств для создания соединения.");
            return;
        }

        Parallel.ForEach(listPlcWoCollections, delegate (Plc plc)
        {
            StartListener(plc);
        });
    }
    // Summary:
    //     Получает из базы список Plc. Каждый Plc из списка не будет содержать вложенные
    //     коллекции
    private List<Plc> GetListPlcWoCollections()
    {
        using (UnitOfWork.StartReadOnly())
        {
            return Repository.Resolve<Plc>().Query.ToList();
        }
    }
```



```C#
#Listener:
//Метод подписки на изменение ноды в ПЛК
private MonitoredItem ScalesSubscription()
{
    var scales = new NodeValue<ushort>(GetScalesStateNode(), ScalesStateSubscriptionHandler);
    return Client.OpcUa.Subscription(scales);
}
// Настройка ноды (общая для всех ПЛК)
private string GetScalesStateNode() => ConfigurationManager.AppSettings["OpcUzgmWs3SHET"];

//Пришло новое значение
private void ScalesStateSubscriptionHandler(object sender, ushort e)
{
    using (UnitOfWork.StartReadOnly()) 
    {
        var nodeVes = new NodeValue<float>(ConfigurationManager.AppSettings[$"OpcUzgmWs3VES"]);
        float vesFloat = Client.OpcUa.GetNodeValue<float>(nodeVes);
        decimal ves = Convert.ToDecimal(vesFloat);
        Processor.ScalesProcessing(e, ves, Plc);
    }
}

//
public void ScalesProcessing(int vesSchet, decimal ves, Plc plc)
{
     string actionName = $"[{MethodBase.GetCurrentMethod()?.Name} {plc?.Host} ves:{ves} vesSchet:{vesSchet}]";
     try
     {
         Guid unitUId = _aggregateManager.GetPlcMachineUnitUId(plc, MachineType.Scales);//GetPlcMachineUnitUId(plc, MachineType.Scales);

         ...     
        var message = new ScalesDPStateMsg
        {
            ChangeType = changeType,
            UnitUId = unitUId,
            Date = time,
            Weight = ves, //Вес!
            Counter = vesSchet
        };
        BusProcessingProvider.SendToBus(EventNames.ScalesDPState, message, $"{actionName} состояние весов");

         ...    
```


[Вперед](TPIndependentOfComm.md)