

namespace DpConnect.Connection
{
    public interface IDpConfigurableConnection<TConnectionConfiguration>
        where TConnectionConfiguration : IDpConnectionConfiguration
    {
        void Configure(TConnectionConfiguration configuration);
    }
}
