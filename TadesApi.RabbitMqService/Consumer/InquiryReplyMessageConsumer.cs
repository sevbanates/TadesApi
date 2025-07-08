using MassTransit;
using System;
using System.Threading.Tasks;
using VideoPortalApi.Models.ViewModels.Inquiry;
using VideoPortalApi.QueService.Email.Contact.Interfaces;
using VideoPortalApi.RabbitMqService.TaskManager;

namespace VideoPortalApi.RabbitMqService.Consumer
{
    public class InquiryReplyMessageConsumer : BaseConsumer<ReplyInquiryDto>
    {
        private readonly IGenericEmailJob _genericEmailJob;

        public InquiryReplyMessageConsumer(IGenericEmailJob genericEmailJob)
        {
            _genericEmailJob = genericEmailJob;
        }

        public override Task Consume(ConsumeContext<ReplyInquiryDto> context)
        {
            _genericEmailJob.SendGenericEmail(context.Message);
            return Task.CompletedTask;
        }
    }
}