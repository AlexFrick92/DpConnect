using DpConnect.Configuration;
using DpConnect.OpcUa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DpConnect.Example.TechParamApp.ViewModel
{
    public class ConnectionConfiguratorViewModel : BaseViewModel
    {
        IDpConnectionConfiguration connectionConfig;
        public ConnectionConfiguratorViewModel(Type configType)
        {            
            List<NamedConfigSettingViewModel> pars = new List<NamedConfigSettingViewModel>();


            connectionConfig = Activator.CreateInstance(configType) as IDpConnectionConfiguration;
            
            foreach(var prop in configType.GetProperties())
            {
                var attribute = prop.GetCustomAttributes(false).OfType<DpConfigPropertyAttribute>().FirstOrDefault();
                if(attribute != null)
                {
                    var par = new NamedConfigSettingViewModel();

                    par.Name = attribute.DisplayName;

                    par.ValueChanged += (s, v) =>
                    {

                        object convertedValue = null;

                        convertedValue = ParseToPropertyValue(v, prop.PropertyType);
                        prop.SetValue(connectionConfig, convertedValue);
                    };



                    pars.Add(par);
                }

            }

            Settings = pars;
        }        

        object ParseToPropertyValue(object v, Type propertyType)
        {
            object convertedValue = null;

            if (propertyType == typeof(string))
            {
                // Преобразование для string
                convertedValue = v as string;
            }
            else if (propertyType == typeof(int))
            {
                // Преобразование для int
                convertedValue = Convert.ChangeType(v, propertyType);
            }
            else if (propertyType == typeof(float))
            {
                // Преобразование для float
                convertedValue = Convert.ChangeType(v, propertyType);
            }
            else if (propertyType == typeof(bool))
            {
                // Используем TryParse для преобразования string в bool
                if (bool.TryParse(v.ToString(), out bool boolValue))
                {
                    convertedValue = boolValue;
                }
                else
                {
                    Console.WriteLine(($"Невозможно преобразовать значение '{v}' в тип {propertyType.Name}"));
                }
            }
            else
            {
                // Для других типов используем Convert.ChangeType
                convertedValue = Convert.ChangeType(v, propertyType);
            }



            return convertedValue;

        }

        public IEnumerable<NamedConfigSettingViewModel> Settings { get; }           

        public void CreateConnection(IDpConnectionManager manager)
        {
            //manager.CreateConnection<IOpcUaConnection, OpcUaConnectionConfiguration>(Config);
        }

        public IDpConnectionConfiguration CreateConfiguration()
        {
            return connectionConfig;
        }
    }
}
