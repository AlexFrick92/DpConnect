using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class NamedDpConfigSettingViewModel
    {
        public string DpName { get; set; }    
        public IEnumerable<NamedConfigSettingViewModel> SourceSettings { get; set; }
    }
}
