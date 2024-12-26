using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class TechParamReaderConfigViewModel : BaseViewModel, ITechParameterConfiguratorViewModel
    {

        public TechParamReaderConfigViewModel()
        {
            List<NamedDpConfigSettingViewModel> namedDpConfigSettingViewModels = new List<NamedDpConfigSettingViewModel>();

            namedDpConfigSettingViewModels.Add(
                new NamedDpConfigSettingViewModel() { DpName = "Значение"}
                );


            Settings = namedDpConfigSettingViewModels;
        }
        public string ConfiguratorName => "FloatParameter";

        public IEnumerable<NamedDpConfigSettingViewModel> Settings { get; private set; }

        public void CreateTechParameter()
        {
            throw new NotImplementedException();
        }        
    }
}
