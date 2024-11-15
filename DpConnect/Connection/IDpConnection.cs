


namespace DpConnect.Connection
{
    public interface IDpConnection
    { 

        string Id { get; }

        void Configure(IDpConnectionConfiguration configuration);
        void ConnectDpValue<T>(IDpValueSource<T> dpValue, IDpSourceConfiguration sourceConfiguration);

        void ConnectDpMethod(IDpActionSource dpMethod, IDpSourceConfiguration sourceConfiguration);
        void Open();
        void Close();

    }
}
