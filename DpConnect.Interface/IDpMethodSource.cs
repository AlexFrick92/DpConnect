using System.Collections.Generic;

namespace DpConnect.Interface
{
    public delegate IList<object> CommonDelegate(params object[] args);
    public interface IDpMethodSource : IDataPoint
    {    
        CommonDelegate CallLower { get; set; }

    }
}
