using MassTransit;
using System;
using System.Threading.Tasks;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.QueService.Email.AuthManagement.Interfaces;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService.Consumer
{
    public class GeneratedUserEmailConsumer : BaseConsumer<NewCustomerMailModel>
    {
        private readonly IAuthMailJob _authMailJob;
        public GeneratedUserEmailConsumer(IAuthMailJob authMailJob)
        {
            _authMailJob = authMailJob;
        }

        public override Task Consume(ConsumeContext<NewCustomerMailModel> context)
        {
            Console.WriteLine($"Generated Customer User Job Started userId:{context.Message.UserId}");

           _authMailJob.SendGeneratedUserMail(context.Message);

            return Task.CompletedTask;
        }
    }
}
