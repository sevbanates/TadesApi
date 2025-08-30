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

        [SecurityState((int)CustomerSecurity.Save)]
        [HttpPost("create")]
        public ActionResponse<bool> Create([FromBody] CustomerCreateDto model)
        {
            try
            {
                var response = _customerService.CreateCustomer(model);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "CreateCustomer :" + ex.Message);
            }
    
        }

        [SecurityState((int)CustomerSecurity.Save)]
        [HttpPut("update/{id}")]
        public ActionResponse<bool> Update(int id, [FromBody] CustomerUpdateDto model)
        {


            try
            {
                var response = _customerService.UpdateCustomer(id, model);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "Update :" + ex.Message);
            }
    
        }



        [SecurityState((int)CustomerSecurity.List)]
        [HttpGet]
        public PagedAndSortedResponse<CustomerDto> GetMulti([FromQuery] PagedAndSortedSearchInput input)
        {
            try
            {
                var response = _customerService.GetCustomers(input);
                response.Token = _appSecurity.Token;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new PagedAndSortedResponse<CustomerDto>(), "GetMulti Error :" + ex.Message);
            }
        }


        [SecurityState((int)CustomerSecurity.View)]
        [HttpGet]
        [Route("{id}/{guidId}")]
        public ActionResponse<CustomerDto> GetSingle(long id, Guid guidId)
        {
            try
            {
                // Admin değilse, kullanıcının kendi verisine erişip erişmediğini kontrol et
                if (_appSecurity.RoleId != 100) // Admin role ID'si 100
                {
                    var customer = _customerService.GetSingle(id, guidId);
                    if (!customer.IsSuccess || customer.Entity == null)
                    {
                        return ErrorResponse(new ActionResponse<CustomerDto>(), "Müşteri bulunamadı veya erişim izniniz yok.");
                    }

                    // Kullanıcının kendi müşterisi mi kontrol et
                    if (customer.Entity.UserId != _appSecurity.UserId)
                    {
                        return ErrorResponse(new ActionResponse<CustomerDto>(), "Bu müşteriye erişim izniniz bulunmamaktadır.");
                    }
                }

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