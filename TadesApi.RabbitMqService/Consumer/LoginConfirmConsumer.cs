using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.QueService.Email;
using VideoPortalApi.QueService.Email.AuthManagement.Interfaces;
using VideoPortalApi.QueService.Email.AuthManagement.Services;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService.Consumer
{
    public class LoginConfirmConsumer : BaseConsumer<LoginConfirmMailModel>
    {
        private readonly IAuthMailJob _authMailJob;
        public LoginConfirmConsumer(IAuthMailJob authMailJob)
        {
            _authMailJob = authMailJob;
        }

        public override Task Consume(ConsumeContext<LoginConfirmMailModel> context)
        {
            Console.WriteLine($"Login Confirm Job Started :{context.Message.UserId}");

            _authMailJob.SendLoginConfirmationMail(context.Message);

            return Task.CompletedTask;
        }
    }
}
