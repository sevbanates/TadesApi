using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.QueService.Email;
using VideoPortalApi.QueService.Email.AuthManagement.Interfaces;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService.Consumer
{
    public class ForgotPasswordConsumer : BaseConsumer<ForgotPasswordMailModel>
    {
        private readonly IAuthMailJob _authMailJob;
        public ForgotPasswordConsumer(IAuthMailJob authMailJob)
        {
            _authMailJob = authMailJob;
        }

        public override Task Consume(ConsumeContext<ForgotPasswordMailModel> context)
        {
            _authMailJob.SendForgotPasswordMail(context.Message);
            return Task.CompletedTask;
        }
    }
}
