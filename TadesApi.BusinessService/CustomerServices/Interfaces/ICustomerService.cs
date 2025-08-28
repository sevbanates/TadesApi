using System;
using TadesApi.BusinessService._base;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Db.Entities;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.BusinessService.CustomerServices.Interfaces
{
    public interface ICustomerService : IBaseServiceNg<CustomerCreateDto, CustomerUpdateDto, CustomerDto, PagedAndSortedInput>
    {
        PagedAndSortedResponse<CustomerDto> GetCustomers(PagedAndSortedSearchInput input);
        ActionResponse<bool> CreateCustomer(CustomerCreateDto model);
        //PagedAndSortedResponse<CustomerDto> GetCustomersByAccounter(CustomerCreateDto model);
        ActionResponse<bool> UpdateCustomer(int id, CustomerUpdateDto model);
    }
}