using System;

namespace DpConnect.Interface
{
    public interface IDpValueSource<T>
    {
        string Name { get; }

        void UpdateValue(T value);

        Action<T> ModifyValue { get; set; }
        
    }
}
