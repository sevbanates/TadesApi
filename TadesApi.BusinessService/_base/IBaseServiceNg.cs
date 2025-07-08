using System;
using TadesApi.Core;
using TadesApi.Core.Models.Global;

namespace TadesApi.BusinessService._base
{
    public interface IBaseServiceNg<in TCreateDto, in TUpdateDto, TViewDto, in TPageAndSortDto> where TPageAndSortDto : PagedAndSortedInput
    {
        PagedAndSortedResponse<TViewDto> GetMulti(TPageAndSortDto input, string includes = "");
        ActionResponse<TViewDto> GetSingle(string includes = "");
        ActionResponse<TViewDto> GetSingle(long id, Guid guidId, string includes = "");
        ActionResponse<TViewDto> Create(TCreateDto input);
        ActionResponse<TViewDto> Update(long id, TUpdateDto input);

        ActionResponse<bool> Delete(long id, Guid guidId);
    }
}