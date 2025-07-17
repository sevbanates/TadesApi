using System;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.CustomModels;

namespace TadesApi.BusinessService.Common.Interfaces;

public interface ICommonService
{
    ActionResponse<TextIntValueDto> GetRoles();
    ActionResponse<TextIntValueDto> GetUserById(long userId);
    ActionResponse<TextIntValueDto> GetUsers();
    ActionResponse<TextIntValueDto> GetConsultants();
    ActionResponse<CountryAndCityModel> GetCountryAndCity();
    
}