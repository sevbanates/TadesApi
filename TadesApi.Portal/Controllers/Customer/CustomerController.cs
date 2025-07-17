using Microsoft.AspNetCore.Mvc;
using TadesApi.BusinessService.CustomerServices.Interfaces;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;

namespace TadesApi.Portal.Controllers.Customer
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        //[SecurityState((int)LibrarySecurity.Save)]
        [HttpPost]
        [Route("create-invoice")]
        public ActionResponse<bool> CreateCustomer([FromBody] CustomerCreateDto input)
        {
            try
            {
                var response = _customerService.CreateCustomer(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "CreateCustomer :" + ex.Message);
            }
        }


        [SecurityState((int)ClientSecurity.List)]
        [HttpGet]
        public PagedAndSortedResponse<CustomerDto> GetMulti([FromQuery] PagedAndSortedSearchInput input)
        {
            try
            {
                var response = _customerService.GetMulti(input);
                response.Token = _appSecurity.Token;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<CustomerDto>(), "GetMulti Error :" + ex.Message);
            }
        }


        [SecurityState((int)ClientSecurity.View)]
        [HttpGet]
        [Route("{id}/{guidId}")]
        public ActionResponse<CustomerDto> GetSingle(long id, Guid guidId)
        {
            try
            {
                var response = _customerService.GetSingle(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<CustomerDto>(), "GetSingle :" + ex.Message);
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