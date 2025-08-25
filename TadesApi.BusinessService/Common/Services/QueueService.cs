using Hangfire;
using SendWithBrevo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.Core.Security;
using TadesApi.Models.CustomModels;
using static Amazon.S3.Util.S3EventNotification;

namespace TadesApi.BusinessService.CommonServices.services
{
    public class QueueService : IQueueService
    {
        private readonly IJobService _jobService;
        public QueueService(IJobService jobService )
        {
            _jobService = jobService;
            
        }
        public void AddLog<T>(T entity, string message, SecurityModel securityModel)
        {
            BackgroundJob.Enqueue<IJobService>(x => x.AddLog(entity, message, securityModel));
        }

        public void SendAccounterRequestMail<T>(T entity, string subject, string messageText, string toEmail, string toName, string actionUrl, string senderName)
        {
            BackgroundJob.Enqueue<IJobService>(x => x.SendAccounterRequestMail(entity, subject, messageText, toEmail, toName, actionUrl, senderName));
        }

        public void SendTicketCreatedMail(CreatedTicketMailModel model)
        {
            BackgroundJob.Enqueue<IJobService>(x => x.SendTicketCreatedMail(model));
        }

        //public void CheckImzager()
        //{
        //    BackgroundJob.Enqueue<IJobService>(x => x.CheckImzagerApprove());
        //}

        //public void CalculateDelaerOrder()
        //{
        //    BackgroundJob.Enqueue<IJobService>(x => x.ExecuteCalculateDealerSpend());
        //}
    }
}
