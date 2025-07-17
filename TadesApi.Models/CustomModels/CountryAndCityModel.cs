using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lsts;

namespace TadesApi.Models.CustomModels
{
    public class CountryAndCityModel
    {
        public List<SelectNumberModel> Countries{ get; set; }
        public List<SelectNumberModel> Cities{ get; set; }
    }
}
