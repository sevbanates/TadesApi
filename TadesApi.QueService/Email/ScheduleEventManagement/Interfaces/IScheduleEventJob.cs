using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core.Masstransit.Model;

namespace VideoPortalApi.QueService.Email.ScheduleEventManagement.Interfaces
{
    public interface IScheduleEventJob
    {
        public bool SendScheduleEventNotificationMail(ScheduleEventNotificationMailModel input);
        public bool SendScheduleEventConsultantNotificationMail(ScheduleEventNotificationMailModel input);
    }
}