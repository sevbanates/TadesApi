using AutoMapper;
using Lsts;
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
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using static Amazon.S3.Util.S3EventNotification;

namespace TadesApi.BusinessService.InvoiceServices.Services
{
    public class InvoiceService : BaseServiceNg<Invoice, InvoiceDto, InvoiceCreateDto, InvoiceUpdateDto>, IInvoiceService
    {
        private readonly IQueueService _queueService;
        private readonly IRepository<Customer> _customeRepository;

        public InvoiceService(
            IRepository<Invoice> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session, IQueueService queueService, IRepository<Customer> customeRepository) : base(entityRepository, locManager, mapper, session)
        {
            _queueService = queueService;
            _customeRepository = customeRepository;
        }
        public ActionResponse<bool> CreateInvoice(InvoiceCreateDto model)
        {
            var response = new ActionResponse<bool>();

            var entity = _mapper.Map<Invoice>(model);
             _entityRepository.Insert(entity);
             _queueService.AddLog<Invoice>(entity, "Fatura oluşturuldu.", _session.SecurityModel);
            return response;
        }

        public ActionResponse<List<CustomerSelectModel>> GetCustomers()
        {
            var ss =_customeRepository.TableNoTracking.Select(x => new CustomerSelectModel
            { Id = x.Id, FullName = x.Name + " " + x.Surname, Name = x.Name, Surname = x.Surname, VknTckn = x.VknTckn, IsCompany = x.IsCompany, Title = x.Title}).ToList();

            _queueService.AddLog<List<CustomerSelectModel>>(ss, "Cari verileri çekildi.", _session.SecurityModel);

            return new ActionResponse<List<CustomerSelectModel>>
            {
                Entity = ss,
                IsSuccess = true,
                //Message = "Müşteriler başarıyla alındı."
            };
        }
    }
}