using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using TadesApi.Db.Entities;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.BusinessService.InquiryServices.Interfaces;
using TadesApi.BusinessService.MessageServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.CustomModels;
using TadesApi.Models.Global;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Models.ViewModels.Inquiry;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.BusinessService.Common.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUser _session;
        private readonly IMessageService _messageService;
        private readonly IInquiryService _inquiryService;

        public DashboardService(IMapper mapper,
            ICurrentUser session,
          
            IMessageService messageService,
            
            IInquiryService inquiryService)
        {
            _mapper = mapper;
            _session = session;
          
            _messageService = messageService;
          
            _inquiryService = inquiryService;
        }

       
        

     

        public PagedAndSortedResponse<MessageWithSenderDto> GetMessages(PagedAndSortedInput input)
        {
            return _messageService.GetReceivedMessages(input);
        }  
  
    }
}