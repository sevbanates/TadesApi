using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Portal.ActionFilters;
using TadesApi.Portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.Core;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Portal.Controllers.Library
{
    [Route("api/library")]
    [ApiController]
    public class InvoiceController : BaseController
    {
        private readonly ILibraryService _libraryService;

        public InvoiceController(ILibraryService libraryService)
        {
            _libraryService = libraryService;
        }

        [SecurityState((int)LibrarySecurity.View)]
        [HttpGet]
        [Route("{id}/{guidId}")]
        public ActionResponse<LibraryItemDto> GetEntityById(long id, Guid guidId)
        {
            try
            {
                var response = _libraryService.GetSingle(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<LibraryItemDto>(), "GetEntityById Error :" + ex.Message);
            }
        }


        
        [SecurityState((int)LibrarySecurity.Save)]
        [HttpPost]
        public ActionResponse<LibraryItemDto> CreateLibraryItem([FromForm] CreateLibraryItemDto input)
        {
            try
            {
                var response = _libraryService.Create(input);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<LibraryItemDto>(), "CreateLibraryItem :" + ex.Message);
            }
        }

        [SecurityState((int)LibrarySecurity.Save)]
        [HttpPut]
        [Route("{id}")]
        public ActionResponse<LibraryItemDto> UpdateLibraryItem([FromForm] UpdateLibraryItemDto input, long id)
        {
            try
            {
                var response = _libraryService.Update(id, input);
                response.Token = _appSecurity.Token;

                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<LibraryItemDto>(), "UpdateLibraryItem :" + ex.Message);
            }
        }


        [SecurityState((int)LibrarySecurity.Delete)]
        [HttpDelete]
        [Route("{id}/{guidId}")]
        public ActionResponse<bool> DeleteEntity(long id, Guid guidId)
        {
            try
            {
                var response = _libraryService.Delete(id, guidId);
                response.Token = _appSecurity.Token;
                return response;
            }
            catch (Exception ex)
            {
                return ErrorResponse(new ActionResponse<bool>(), "DeleteEntity :" + ex.Message);
            }
        }
    }
}