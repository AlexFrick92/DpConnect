


namespace DpConnect.Interface
{
    public interface IDpConnection
    {

        string Id { get; }

        void Configure(IDpConnectionConfiguration configuration);
        void ConnectDpValue<T>(IDpValue<T> dpValue, IDpValueSourceConfiguration sourceConfiguration);
        void Open();
        void Close();

    }
}
