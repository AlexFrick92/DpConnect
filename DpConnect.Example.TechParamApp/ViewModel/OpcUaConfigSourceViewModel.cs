using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class OpcUaConfigSourceViewModel
    {

        private OpcUaDpValueSourceConfiguration sourceConfiguration;

        public string NodeId { get; set; }
    }
}
