using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;

namespace TadesApi.BusinessService.InvoiceServices.Services
{
    public class InvoiceService : BaseServiceNg<Invoice, InvoiceDto, InvoiceCreateDto, InvoiceUpdateDto>, IInvoiceService
    {
        private readonly IQueueService _queueService;

        public InvoiceService(
            IRepository<Invoice> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session, IQueueService queueService) : base(entityRepository, locManager, mapper, session)
        {
            _queueService = queueService;
        }
        public ActionResponse<bool> CreateInvoice(InvoiceCreateDto model)
        {
            var response = new ActionResponse<bool>();

            var entity = _mapper.Map<Invoice>(model);
             _entityRepository.Insert(entity);
             _queueService.AddLog<Invoice>(entity, "Fatura oluşturuldu.", _session.SecurityModel);
            return response;
        }
    }
}