using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Models.ViewModels.Library;

namespace TadesApi.BusinessService.LibraryServices.Interfaces
{
    public interface ILibraryService : IBaseServiceNg<CreateLibraryItemDto, UpdateLibraryItemDto, LibraryItemDto, PagedAndSortedInput>
    {
    }
}