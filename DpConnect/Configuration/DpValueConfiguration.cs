using DpConnect.Interface;


namespace DpConnect.Configuration
{
    internal class DpValueConfiguration
    {
        public string PropertyName { get; set; }

        public string ConnectionId { get; set; }

        public IDpSourceConfiguration SourceConfiguration { get; set; }
    }
}
