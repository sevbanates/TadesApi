using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.QueService.Email.ScheduleEventManagement.Interfaces;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService.Consumer
{
    internal class ScheduleEventConsultantNotificationEmailConsumer : BaseConsumer<ScheduleEventNotificationMailModel>
    {
        private readonly IScheduleEventJob _scheduleEventJob;

        public ScheduleEventConsultantNotificationEmailConsumer(IScheduleEventJob scheduleEventJob)
        {
            _scheduleEventJob = scheduleEventJob;
        }

        public override Task Consume(ConsumeContext<ScheduleEventNotificationMailModel> context)
        {
            _scheduleEventJob.SendScheduleEventConsultantNotificationMail(context.Message);
            return Task.CompletedTask;
        }
    }
}
