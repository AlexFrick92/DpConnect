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

        public OpcUaDpValueSourceConfiguration sourceConfiguration = new OpcUaDpValueSourceConfiguration();

        public string NodeId
        {
            get => sourceConfiguration.NodeId;
            set => sourceConfiguration.NodeId = value;
        }
    }
}
