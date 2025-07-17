using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Library;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TadesApi.Db.Entities;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.CustomerServices.Interfaces;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Invoice;

namespace TadesApi.BusinessService.CustomerServices.Services
{
    public class CustomerService : BaseServiceNg<Customer, CustomerDto, CustomerCreateDto, CustomerUpdateDto>, ICustomerService
    {
        

        public CustomerService(
            IRepository<Customer> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session) : base(entityRepository, locManager, mapper, session)
        {
        }

        public ActionResponse<bool> CreateCustomer(CustomerCreateDto model)
        {
            throw new NotImplementedException();
        }
    }
}