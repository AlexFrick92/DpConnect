﻿

using DpConnect.Connection;

namespace DpConnect.OpcUa
{
    public interface IOpcUaConnection 
        : IDpConnection, 
        IDpConfigurableConnection<OpcUaConnectionConfiguration>,
        IDpBindableConnection<OpcUaDpValueSourceConfiguration>

    {
    }
}
