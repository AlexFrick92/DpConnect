namespace DpConnect.Interface
{
    public interface IDpMethod<I, O> : IDataPoint
    {
        O Call(I args);
    }
}
