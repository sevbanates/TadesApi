using AutoMapper;
using TadesApi.BusinessService.AppServices;
using TadesApi.Core;
using TadesApi.Db.Infrastructure;
using TadesApi.Core.Models.Global;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace TadesApi.BusinessService
{
    public interface IExtendBusinessService<T1, T2> where T1 : BaseModel where T2 : PagedAndSortedInput
    {
        ActionResponse<T1> OpenDefaultEntity();
        PagedAndSortedResponse<T1> GetEntityPage(T2 entityViewModel);
        ActionResponse<T1> GetEntityById(long id);
        ActionResponse<T1> GetEntityByViewModel(T1 entityViewMdel);
        ActionResponse<T1> InsertEntity(T1 entityViewModel);
        ActionResponse<T1> UpdateEntity(T1 entityViewModel);
        ActionResponse<bool> DeleteEntity(long id);
        ActionResponse<dynamic> GetInitialData();
    }
}