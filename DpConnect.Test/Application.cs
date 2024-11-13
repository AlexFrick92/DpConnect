using DpConnect.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
