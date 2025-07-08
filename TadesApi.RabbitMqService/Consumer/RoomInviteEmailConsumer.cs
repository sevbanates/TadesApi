using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.QueService.Email.RoomInviteManagement.Interfaces;
using VideoPortalApi.QueService.Email.ScheduleEventManagement.Interfaces;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService.Consumer
{
    public class RoomInviteConsumer : BaseConsumer<RoomInviteMailModel>
    {
        private readonly IRoomInviteJob _roomInviteJob;

        public RoomInviteConsumer(IRoomInviteJob roomInviteJob)
        {
            _roomInviteJob = roomInviteJob;
        }

        public override Task Consume(ConsumeContext<RoomInviteMailModel> context)
        {
            Console.WriteLine($"{Lsts.MassTransitJobName._ScheduleEventNotificationEmail} Job Started :{context.Message.UserId}");

            _roomInviteJob.SendRoomInvite(context.Message);

            return Task.CompletedTask;
        }
    }
}
