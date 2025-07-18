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
using TadesApi.BusinessService.CustomerServices.Interfaces;
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
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;

namespace TadesApi.BusinessService.CustomerServices.Services
{
    public class CustomerService : BaseServiceNg<Customer, CustomerDto, CustomerCreateDto, CustomerUpdateDto>, ICustomerService
    {
        private readonly CustomerValidationService _validationService;
        private readonly IQueueService _queueService;
        public CustomerService(
            IRepository<Customer> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session, IQueueService queueService)
            : base(entityRepository, locManager, mapper, session)
        {
            _queueService = queueService;
            _validationService = new CustomerValidationService(entityRepository);
        }

        public ActionResponse<bool> CreateCustomer(CustomerCreateDto model)
        {
            var validation = _validationService.ValidateCreate(model);
            if (!validation.IsSuccess)
                return validation;

            var entity = _mapper.Map<Customer>(model);
            _entityRepository.Insert(entity);
            _queueService.AddLog<Customer>(entity, "Müşteri oluşturuldu.", _session.SecurityModel);
            return new ActionResponse<bool>
            {
                IsSuccess = true,
                Entity = true
            };
        }

        public ActionResponse<bool> UpdateCustomer(int id, CustomerUpdateDto model)
        {
            var validation = _validationService.ValidateUpdate(id, model);
            if (!validation.IsSuccess)
                return validation;

            var entity = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == id);
            _mapper.Map(model, entity);
            _entityRepository.Update(entity);
            _queueService.AddLog<Customer>(entity, "Müşteri güncellendi.", _session.SecurityModel);
            return new ActionResponse<bool>
            {
                IsSuccess = true,
                Entity = true
            };
        }
    }
}