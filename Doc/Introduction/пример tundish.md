[Назад](readme.md)

## Пример с Tundish

### TundishDp - комплексный тип 
```c#
public class TundishDb : ComplexType
{

    public long UpMeltingNumber => Convert.ToInt64(GetValue<uint>("UpMeltingNumber"));

    public long MeltNumberDown => Convert.ToInt64(GetValue<uint>("DownMeltingNumber"));

    public decimal MetalWeightAll => Convert.ToDecimal(GetValue<float>("WeightAllMetalTundish"));

    public decimal MetalWeightDown => Convert.ToDecimal(GetValue<float>("WeightDownMetalTundish"));

    public long CastingMeltNo => MeltsBorderExists ? MeltNumberUp : MeltNumberDown;

    public bool MeltsBorderExists => MetalWeightDown != MetalWeightAll;

}
```
 
Далее в Listener вызывается метод AddSubscription в котором вызываются все подписки:

```C#
    protected override void AddSubscription()
    {
    // Подписка на события ковша
        _ = TundishStateSubscription();    
    }

    private MonitoredItem TundishStateSubscription()
    {
        // Подписка на события ковша
        var tundish = new NodeValue<TundishDb>(TundishStateNodeId, OnTundishStateChanged);
        return Client.OpcUa.Subscription(tundish);
    
    }

    private void OnTundishStateChanged(object sender, TundishDb tundishDb)
    {
        using (UnitOfWork.StartReadOnly()) TundishStateChanged(tundishDb);
    }


    private void TundishStateChanged(TundishDb e) => Processor.TundishProcessing(e, Plc, LineNo);

    public void TundishProcessing(TundishDb tundishDb, Plc plc, int lineNo)
    {
        string actionName = $"{MethodBase.GetCurrentMethod()?.Name} {plc?.Host}";

        if (tundishDb == null) return;

        // Получим UId промковша по данному Plc
        Guid unitUId = _aggregateManager.GetPlcMachineUnitUId(plc, MachineType.Tundish);

        if (unitUId == Guid.Empty) return;

        var message = new TundishDPStateMsg
        {
            ChangeType = ChangeType.Update,
            UnitUId = unitUId,
            TopMeltNo = tundishDb.MeltNumberUp,
            BottomMeltNo = tundishDb.MeltNumberDown,
            TotalWeight = tundishDb.MetalWeightAll,
            BorderPosition = tundishDb.MetalWeightDown,
            LineNo = lineNo
        };
        BusProcessingProvider.SendToBus(EventNames.TundishDPState, message, $"{actionName} состояние ковша");
    }
```
 



## Как по-другому


```c#
 public class TundishDb 
 {

     public long MeltNumberUp {get; set;}

     public long MeltNumberDown {get; set;}

     public decimal MetalWeightAll {get; set;}

     public decimal MetalWeightDown {get; set;}    

     public long CastingMeltNo {get; set;}

     public bool MeltsBorderExists {get; set;}

 }
```

```C#
public class TundishReader : IDpWorker
{
    public IDpValue<TundishDb> TundishState {get; set;}

    Guid Tundish; //Какой это ковш? Ковш не зависит от ПЛК. Это предметные данные

    public OnDpBound()
    {
        TundishState.ValueChanged =+ (s, v) => TundishProcessing(v, tundishId);
    }

    public void TundishProcessing(TundishDb tundishDb, Guid Tundish)
    {
        BusProcessingProvider.SendToBus(EventNames.TundishDPState, Tundish, $"{actionName} состояние ковша");
    }

}
```

```xml
<DpConfiguration>

	<Connections>
		<Connection TypeName="DpConnect.OpcUa.IOpcUaConnection, DpConnect.OpcUa" ConnectionId ="Stend AKF">
			<Endpoint>opc.tcp://10.10.10.95:4840</Endpoint>
		</Connection>		
	</Connections>

	<Workers>
        <!--Подписка на Tundish1 -->
		<Worker TypeName="CLG.TundishReader, CLG">
			<DpValue PropertyName="TundishState">
				<SourceConfiguration ConnectionId ="Stend AKF">
					<NodeId>ns=3;s="Tundish"."Tundish1"</NodeId>
				</SourceConfiguration>
			</DpValue>
		</Worker>	

        <!--Подписка на Tundish2 -->
        <Worker TypeName="CLG.TundishReader, CLG">
            <DpValue PropertyName="TundishState">
                <SourceConfiguration ConnectionId ="Stend AKF">
                    <NodeId>ns=3;s="Tundish"."Tundish1"</NodeId>
                </SourceConfiguration>
            </DpValue>
		</Worker>				
	</Workers>
	
</DpConfiguration>	

```