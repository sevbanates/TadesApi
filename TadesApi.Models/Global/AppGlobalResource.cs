using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Models.Global
{
    public class AppGlobalResource
    {
        
    }
    
    public class AssessmentPlanStatus
    {
        public const string Pending = "Pending";
        public const string InProgress = "InProgress";
        public const string Completed = "Completed";
        public const string Cancelled = "Cancelled";
    }

    public enum ClientStatus
    {
        Potential = 0,
        Active = 1,
        Inactive = 2,
        Archived = 3
    }

    public enum ScheduledEventStatus
    {
        Scheduled = 0,
        Completed = 1,
        Cancelled = 2
    }
}