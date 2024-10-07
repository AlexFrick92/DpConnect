namespace DpConnect.Interface
{
    public interface IDpProcessor
    {
        string Name { get; set; }
        //void RegisterDp(IDataPoint dataPoint, XDocument xmlConfig);

        void OnDpInitialized();                
    }
}
