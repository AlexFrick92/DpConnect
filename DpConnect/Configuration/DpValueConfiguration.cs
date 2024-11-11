using DpConnect.Interface;


namespace DpConnect.Configuration
{
    public class DpValueConfiguration
    {
        public string PropertyName { get; set; }

        public string ConnectionId { get; set; }

        public IDpValueSourceConfiguration SourceConfiguration { get; set; }
    }
}
