using System.Linq;
using TadesApi.Core;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Customer;

namespace TadesApi.BusinessService.CustomerServices.Services
{
    public class CustomerValidationService
    {
        private readonly IRepository<Customer> _entityRepository;

        public CustomerValidationService(IRepository<Customer> entityRepository)
        {
            _entityRepository = entityRepository;
        }

        public ActionResponse<bool> ValidateCreate(CustomerCreateDto model)
        {
            var response = new ActionResponse<bool>();

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Surname) || string.IsNullOrWhiteSpace(model.VknTckn))
            {
                response.IsSuccess = false;
                response.ReturnMessage.Add("Name, Surname ve VknTckn alanlarý zorunludur.");
                return response;
            }

            var exists = _entityRepository.TableNoTracking.Any(x => x.VknTckn == model.VknTckn);
            if (exists)
            {
                response.IsSuccess = false;
                response.ReturnMessage.Add("Bu VknTckn ile kayýtlý bir müþteri zaten mevcut.");
                return response;
            }

            response.IsSuccess = true;
            return response;
        }

        public ActionResponse<bool> ValidateUpdate(int id, CustomerUpdateDto model)
        {
            var response = new ActionResponse<bool>();

            var entity = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == id);
            if (entity == null)
            {
                response.IsSuccess = false;
                response.ReturnMessage.Add("Müþteri bulunamadý.");
                return response;
            }

            var exists = _entityRepository.TableNoTracking.Any(x => x.VknTckn == model.VknTckn && x.Id != id);
            if (exists)
            {
                response.IsSuccess = false;
                response.ReturnMessage.Add("Bu VknTckn baþka bir müþteri tarafýndan kullanýlýyor.");
                return response;
            }

            response.IsSuccess = true;
            return response;
        }
    }
}