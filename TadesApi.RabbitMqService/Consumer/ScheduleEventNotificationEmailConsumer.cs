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
    public class ScheduleEventNotificationEmailConsumer : BaseConsumer<ScheduleEventNotificationMailModel>
    {
        private readonly IScheduleEventJob _scheduleEventJob;

        public ScheduleEventNotificationEmailConsumer(IScheduleEventJob scheduleEventJob)
        {
            _scheduleEventJob = scheduleEventJob;
        }

        public override Task Consume(ConsumeContext<ScheduleEventNotificationMailModel> context)
        {
            Console.WriteLine($"{Lsts.MassTransitJobName._ScheduleEventNotificationEmail} Job Started :{context.Message.UserId}");

            _scheduleEventJob.SendScheduleEventNotificationMail(context.Message);

            return Task.CompletedTask;
        }

    }
}
