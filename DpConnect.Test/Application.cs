using DpConnect.Interface;
using Promatis.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
