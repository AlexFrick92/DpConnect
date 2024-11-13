


namespace DpConnect.Interface
{
    public interface IDpConnection
    {

        string Id { get; }

        void Configure(IDpConnectionConfiguration configuration);
        void ConnectDpValue<T>(IDpValue<T> dpValue, IDpSourceConfiguration sourceConfiguration);

        void ConnectDpMethod(IDpMethod dpMethod, IDpSourceConfiguration sourceConfiguration);
        void Open();
        void Close();

    }
}
