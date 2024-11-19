

namespace DpConnect
{
    public interface IDpConnectionConfiguration
    {
        string ConnectionId { get; }

        bool Active { get; }
    }
}
