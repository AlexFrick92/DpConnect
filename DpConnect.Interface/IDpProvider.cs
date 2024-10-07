using Promatis.Core.Logging;
using System.Xml.Linq;

namespace DpConnect.Interface
{
    public interface IDpProvider
    {
        string Name { get; set; }

        IDpProvider Clone();

        void SetLogger(ILogger logger);

        void ConfigureHost(XDocument xmlConfig);

        void RegisterDp<T>(IDpValueSource<T> dp, XDocument xmlConfig);         

        void RegisterMethod(IDpMethodSource method, XDocument xmlConfig);

        void Start();
        void Stop();             
    }
}
