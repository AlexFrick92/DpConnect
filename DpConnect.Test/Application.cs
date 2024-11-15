

namespace DpConnect.Test
{
    public class Application : IApp
    {
        IDpBuilder builder;
        public Application(IDpBuilder dpBuilder)
        {
            builder = dpBuilder;
        }

        public void Start()
        {
            


            builder.Build();

            builder.ConnectionManager.OpenConnections();            
        }
    }
}
