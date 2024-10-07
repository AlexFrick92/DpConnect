using System;

namespace DpConnect.Interface
{
    public interface IDpComplexValueConfig
    {
        IDataPoint AddProperty(Type type, string name);
    }
}
