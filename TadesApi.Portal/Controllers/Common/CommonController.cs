using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using SendWithBrevo;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.BusinessService.InquiryServices.Interfaces;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ConstantKeys;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Models.ViewModels.Inquiry;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Models.ViewModels.PotentialClient;

namespace TadesApi.Portal.Controllers.Common;

//[Route("api/[Controller]")]
[Route("api/common")]
[ApiController]
public class CommonController : ControllerBase
{
    private readonly ICommonService _commonService;
    private readonly IInquiryService _inquiryService;
    private readonly ILibraryService _libraryService;
    private readonly IEmailHelper _emailHelper;

    public CommonController(ICommonService commonService, ILibraryService libraryService, IInquiryService inquiryService,
        IEmailHelper emailHelper)
    {
        _commonService = commonService;
        _libraryService = libraryService;
        _inquiryService = inquiryService;
        _emailHelper = emailHelper;
    }


    [HttpGet]
    [Route("roles")]
    public ActionResponse<TextIntValueDto> GetRoles()
    {
        try
        {
            var response = _commonService.GetRoles();
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<TextIntValueDto>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }


    //***************************** General Function  ***********************************

    [HttpGet]
    [Route("user/{userId}")]
    public ActionResponse<TextIntValueDto> GetUserById(long userId)
    {
        try
        {
            var response = _commonService.GetUserById(userId);
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<TextIntValueDto>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }

    [HttpGet]
    [Route("users")]
    public ActionResponse<TextIntValueDto> GetUsers()
    {
        try
        {
            var response = _commonService.GetUsers();
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<TextIntValueDto>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }

    [HttpGet]
    [Route("consultants")]
    public ActionResponse<TextIntValueDto> GetConsultants()
    {
        try
        {
            var response = _commonService.GetConsultants();
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<TextIntValueDto>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }


    [HttpPut]
    [Route("language/{culture}/{returnUrl}")]
    public IActionResult ChangeLanguage(string culture, string returnUrl)
    {
        Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            }
        );

        return LocalRedirect(returnUrl);
    }


    [HttpGet]
    [Route("library-items")]
    public IActionResult GetLibraryContent([FromQuery] LibrarySearchInput input)
    {
        try
        {
            var response = _libraryService.GetMulti(input);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound();
        }
    }
    

    [HttpGet]
    [Route("document/{id}/{guidId}")]
    public ActionResponse<LibraryItemDto> GetDocumentById(long id, Guid guidId)
    {
        try
        {
            var response = _libraryService.GetSingle(id, guidId);
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<LibraryItemDto>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }

    [HttpGet]
    [Route("countries-cities")]
    public ActionResponse<CountryAndCityModel> GetCountryAndCity()
    {
        try
        {
            var response = _commonService.GetCountryAndCity();
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<CountryAndCityModel>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }

    //[HttpPost]
    //[Route("potential-client")]
    //public ActionResponse<ClientBasicDto> CreatePotentialClient(CreatePotentialClientDto input)
    //{
    //    try
    //    {
    //        var response = _clientService.Create(input);
    //        // Send email to the admin
    //        var recipients = new List<Recipient>
    //        {
    //            new(email: "admin@globalpsychsolutions.com", name: "Admin"),
    //        };
    //        var parameters = new Dictionary<string, string>
    //        {
    //            { EmailParams.FirstName, input.FirstName },
    //            { EmailParams.LastName, input.LastName },
    //            { EmailParams.DateOfBirth, input.BirthDate }
    //        };
    //        const string subject = "New Intake Form Submitted";
    //        _emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.IntakeFormSubmitted);
            
    //        return response;
    //    }

    //    catch (Exception ex)
    //    {
    //        return new ActionResponse<ClientBasicDto>
    //        {
    //            IsSuccess = false,
    //            ReturnMessage = new List<string> { ex.Message }
    //        };
    //    }
    //}

    //[HttpPut]
    //[Route("potential-client/{id}/family")]
    //public ActionResponse<ClientBasicDto> UpdateInterpersonalRelationshipsAndCreateFamily(
    //    [FromBody] UpdateInterpersonalRelationshipsAndCreateFamilyDto input, long id)
    //{
    //    try
    //    {
    //        var response = _clientService.UpdateInterpersonalRelationshipsAndCreateFamily(id, input);
    //        return response;
    //    }
    //    catch (Exception ex)
    //    {
    //        return new ActionResponse<ClientBasicDto>
    //        {
    //            IsSuccess = false,
    //            ReturnMessage = new List<string> { ex.Message }
    //        };
    //    }
    //}

    
    
   
    [HttpPost]
    [Route("inquiry")]
    public ActionResponse<InquiryDto> CreateInquiry([FromBody] CreateInquiryDto input)
    {
        try
        {
            var response = _inquiryService.CreateInquiry(input);
            return response;
        }
        catch (Exception ex)
        {
            return new ActionResponse<InquiryDto>
            {
                IsSuccess = false,
                ReturnMessage = new List<string> { ex.Message }
            };
        }
    }
}