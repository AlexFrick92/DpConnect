namespace DpConnect.Configuration
{
    public interface IDpFluentBuilder
    {
        IDpFluentBuilder AddConfiguration(params string[] configPath);

        IDpFluentBuilder Build ();

        void StartProviders();
    }
}