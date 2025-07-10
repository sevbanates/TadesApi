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
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ViewModels.Invoice;

namespace TadesApi.BusinessService.InvoiceServices.Services
{
    public class InvoiceService : BaseServiceNg<Invoice, InvoiceDto, InvoiceCreateDto, InvoiceUpdateDto>, IInvoiceService
    {
        

        public InvoiceService(
            IRepository<Invoice> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session) : base(entityRepository, locManager, mapper, session)
        {
        }
        public ActionResponse<bool> CreateInvoice(InvoiceCreateDto model)
        {
            var response = new ActionResponse<bool>();

            var entity = _mapper.Map<Invoice>(model);
             _entityRepository.Insert(entity);
             return response;
        }
    }
}