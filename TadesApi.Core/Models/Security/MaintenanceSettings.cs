using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Security
{
    public class MaintenanceSettings
    {
        public MaintenanceSettings()
        {
            Actions = new string[0];
            ConstraintFields = new string[0];
            rowActions = new List<CustomAction>();
        }
        public string TableName { get; set; }
        public string IdentifierField { get; set; }
        public bool isGotoList { get; set; } = true;
        public bool DefaultEditMode { get; set; } = true;
        public string[] Actions { get; set; }
        public string[] ConstraintFields { get; set; }
        public string[] ReadonlyFields { get; set; }
        public List<CustomAction> AvailableActions { get; set; } = new List<CustomAction>();
        //public List<string> LocalState { get; set; } = new List<string>();
        public List<string> LocalActions { get; set; } = new List<string>();

        public List<CustomAction> rowActions { get; set; }

    }
}
