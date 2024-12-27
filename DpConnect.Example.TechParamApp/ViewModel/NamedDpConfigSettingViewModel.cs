using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class NamedDpConfigSettingViewModel : BaseViewModel
    {

        public NamedDpConfigSettingViewModel(string DpName, IEnumerable<NamedConfigSettingViewModel> sourceSettings)
        {
            this.DpName = DpName;
            SourceSettings = sourceSettings;

        }
        public string DpName { get; set; }

        public IEnumerable<NamedConfigSettingViewModel> SourceSettings { get; private set; }
    }
}
