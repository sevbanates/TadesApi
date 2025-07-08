using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.CustomModels
{
    public class ClientInitialModel
    {
        public ClientInitialModel()
        {
            GradeList = new();
            ServiceTypeList = new();
            EvaluationTypeList = new();
            FrequencyList = new();
            IntervalList = new();
            MedicaidList = new();
            PhoneTypeList = new();
            ObjectiveStatusList = new();
            AttendanceList = new();
            StateList = new();
            StartTimeList = new();
        }

        public List<TextValueDto> GradeList { get; set; }
        public List<TextValueDto> ServiceTypeList { get; set; }
        public List<TextValueDto> EvaluationTypeList { get; set; }
        public List<TextValueDto> FrequencyList { get; set; }
        public List<TextValueDto> IntervalList { get; set; }
        public List<TextValueDto> MedicaidList { get; set; }
        public List<TextValueDto> PhoneTypeList { get; set; }
        public List<TextValueDto> ObjectiveStatusList { get; set; }
        public List<TextValueDto> AttendanceList { get; set; }
        public List<TextValueDto> StateList { get; set; }
        public List<TextValueDto> StartTimeList { get; set; }
    }
}