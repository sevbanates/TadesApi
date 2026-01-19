using Lsts;
using Microsoft.AspNetCore.Mvc;
using System;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.Invoice
{
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : BaseController
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        //[SecurityState((int)LibrarySecurity.Save)]
        [HttpPost]
        [Route("create-invoice")]
        public ActionResponse<bool> CreateInvoice([FromBody] InvoiceCreateDto input)
        {
            try
            {
                var response = _invoiceService.CreateInvoice(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "CreateInvoice :" + ex.Message);
            }
        }


        [SecurityState((int)CustomerSecurity.List, "Customer")]
        [HttpGet]
        [Route("get-customers")]
        public ActionResponse<List<CustomerSelectModel>> GetCustomers()
        {
            try
            {
                var response = _invoiceService.GetCustomers();
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<List<CustomerSelectModel>>(), "GetCustomers :" + ex.Message);
            }
        }

        //[SecurityState((int)LibrarySecurity.View)]
        //[HttpGet]
        //[Route("{id}/{guidId}")]
        //public ActionResponse<LibraryItemDto> GetEntityById(long id, Guid guidId)
        //{
        //    try
        //    {
        //        var response = _libraryService.GetSingle(id, guidId);
        //        response.Token = _appSecurity.Token;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(new ActionResponse<LibraryItemDto>(), "GetEntityById Error :" + ex.Message);
        //    }
        //}



        //[SecurityState((int)LibrarySecurity.Save)]
        //[HttpPost]
        //public ActionResponse<LibraryItemDto> CreateLibraryItem([FromForm] CreateLibraryItemDto input)
        //{
        //    try
        //    {
        //        var response = _libraryService.Create(input);
        //        response.Token = _appSecurity.Token;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(new ActionResponse<LibraryItemDto>(), "CreateLibraryItem :" + ex.Message);
        //    }
        //}

        //[SecurityState((int)LibrarySecurity.Save)]
        //[HttpPut]
        //[Route("{id}")]
        //public ActionResponse<LibraryItemDto> UpdateLibraryItem([FromForm] UpdateLibraryItemDto input, long id)
        //{
        //    try
        //    {
        //        var response = _libraryService.Update(id, input);
        //        response.Token = _appSecurity.Token;

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(new ActionResponse<LibraryItemDto>(), "UpdateLibraryItem :" + ex.Message);
        //    }
        //}


        //[SecurityState((int)LibrarySecurity.Delete)]
        //[HttpDelete]
        //[Route("{id}/{guidId}")]
        //public ActionResponse<bool> DeleteEntity(long id, Guid guidId)
        //{
        //    try
        //    {
        //        var response = _libraryService.Delete(id, guidId);
        //        response.Token = _appSecurity.Token;
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return ErrorResponse(new ActionResponse<bool>(), "DeleteEntity :" + ex.Message);
        //    }
        //}
    }
}