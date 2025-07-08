using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.CustomModels
{
    public class PotentialStudentInitialModel
    {
        public PotentialStudentInitialModel()
        {
            StateList = new();
            CountryList = new();
        }

        public List<TextValueDto> StateList { get; set; }
        public List<TextValueDto> CountryList { get; set; }
    }
}