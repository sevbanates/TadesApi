using Lsts;
using System;
using System.Collections.Generic;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.BusinessService.InvoiceServices.Interfaces
{
    public interface IInvoiceService : IBaseServiceNg<InvoiceCreateDto, InvoiceUpdateDto, InvoiceDto, PagedAndSortedInput>
    {
        ActionResponse<bool> CreateInvoice(InvoiceCreateDto model);
        ActionResponse<List<CustomerSelectModel>> GetCustomers();
    }
}