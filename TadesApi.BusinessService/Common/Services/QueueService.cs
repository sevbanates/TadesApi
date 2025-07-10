using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.Core.Security;

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
