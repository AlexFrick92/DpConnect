


namespace DpConnect.Connection
{
    public interface IDpConnection
    { 

        string Id { get; }

        bool Active { get; }

        void ConnectDpValue<T>(IDpValueSource<T> dpValue, IDpSourceConfiguration sourceConfiguration);

        void ConnectDpMethod(IDpActionSource dpMethod, IDpSourceConfiguration sourceConfiguration);

        void Open();
        void Close();

    }   
}
