```c#

public interface IDpConnectionConfiguration
{

    Type ConnectionType { get; }

    string ConnectionId { get; }

    bool Active { get; }        

    void FromXml(XDocument config);
}

public interface IDpSourceConfiguration
{
}

public interface IDpConfigurableConnection<TConnectionConfiguration, TSourceConfiguration>
    where TConnectionConfiguration : IDpConnectionConfiguration
    where TSourceConfiguration : IDpSourceConfiguration
{
    void Configure(TConnectionConfiguration configuration);
}

public interface IOpcUaConnection : IDpConnection, IDpConfigurableConnection<OpcUaConnectionConfiguration, OpcUaDpValueSourceConfiguration> 
{
}
public class OpcUaConnectionConfiguration : IDpConnectionConfiguration
{
    public string ConnectionId { get; set; }

    public string Endpoint { get; set; }

    public bool Active { get; set; }

    public Type ConnectionType { get; private set; } = typeof(IOpcUaConnection);

    public void FromXml(XDocument config)
    {
        Endpoint = config.Root.Element("Endpoint").Value;
    }
}
public class OpcUaDpValueSourceConfiguration : IDpSourceConfiguration
{
    public string NodeId { get; set; }
}

public interface IDpConnectionManager
{
    /// <summary>
    /// Создать соединение на основе конфигурации
    /// </summary>
    /// <param name="configuration"></param>
    /// <returns></returns>
    //IDpConnection CreateConnection(IDpConnectionConfiguration configuration);
    IDpConnection CreateConnection<T, TConnectionConfig>(TConnectionConfig configuration)
        where TConnectionConfig : IDpConnectionConfiguration
        where T : IDpConfigurableConnection<TConnectionConfig, IDpSourceConfiguration>;

    IDpConnection GetConnection(string Id);

    IEnumerable<IDpConnection> ConfiguredConnections { get; }

    event EventHandler<IDpConnection> NewConnectionCreated;        

    void OpenConnections();

    void CloseConnections();
}

IDpConnectionManager man = new MyConnectionManager(); //Реализует IDpConnectionManager

var con = man.CreateConnection<IOpcUaConnection, OpcUaConnectionConfiguration>(new OpcUaConnectionConfiguration());


```