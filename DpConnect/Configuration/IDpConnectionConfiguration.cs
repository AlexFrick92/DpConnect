

using System;

namespace DpConnect
{
    public interface IDpConnectionConfiguration
    {

        Type ConnectionType { get; }

        string ConnectionId { get; }

        bool Active { get; }
    }
}
