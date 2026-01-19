using AutoMapper;
using Lsts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.BusinessService.LibraryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Helper;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.AppMessages;
using TadesApi.Models.CustomModels;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using static Amazon.S3.Util.S3EventNotification;

namespace TadesApi.BusinessService.InvoiceServices.Services
{
    public class InvoiceService : BaseServiceNg<Invoice, InvoiceDto, InvoiceCreateDto, InvoiceUpdateDto>, IInvoiceService
    {
        private readonly IQueueService _queueService;
        private readonly IRepository<Customer> _customeRepository;

        public InvoiceService(
            IRepository<Invoice> entityRepository,
            ILocalizationService locManager,
            IMapper mapper,
            ICurrentUser session, IQueueService queueService, IRepository<Customer> customeRepository) : base(entityRepository, locManager, mapper, session, queueService)
        {
            _queueService = queueService;
            _customeRepository = customeRepository;
        }
        public ActionResponse<bool> CreateInvoice(InvoiceCreateDto model)
        {
            var response = new ActionResponse<bool>();

            var entity = _mapper.Map<Invoice>(model);
             _entityRepository.Insert(entity);
             _queueService.AddLog<Invoice>(entity, "Fatura oluşturuldu.", _session.SecurityModel);
            return response;
        }

        public ActionResponse<List<CustomerSelectModel>> GetCustomers()
        {
            List<CustomerSelectModel> ss;
            if (_session.IsAccounter)
            {
                 ss = _customeRepository.TableNoTracking.Where(x=> x.UserId == _session.SelectedUserId).Select(x => new CustomerSelectModel
                    { Id = x.Id, FullName = x.Name + " " + x.Surname, Name = x.Name, Surname = x.Surname, VknTckn = x.VknTckn, IsCompany = x.IsCompany, Title = x.Title }).ToList();
            }
            else if (_session.IsAdmin)
            {
                 ss = _customeRepository.TableNoTracking.Select(x => new CustomerSelectModel
                    { Id = x.Id, FullName = x.Name + " " + x.Surname, Name = x.Name, Surname = x.Surname, VknTckn = x.VknTckn, IsCompany = x.IsCompany, Title = x.Title }).ToList();
            }
            else
            {
               ss = _customeRepository.TableNoTracking.Where(x=> x.UserId == _session.UserId).Select(x => new CustomerSelectModel
                    { Id = x.Id, FullName = x.Name + " " + x.Surname, Name = x.Name, Surname = x.Surname, VknTckn = x.VknTckn, IsCompany = x.IsCompany, Title = x.Title }).ToList();
            }
          

            _queueService.AddLog<List<CustomerSelectModel>>(ss, "Cari verileri çekildi.", _session.SecurityModel);

            return new ActionResponse<List<CustomerSelectModel>>
            {
                Entity = ss,
                IsSuccess = true,
                //Message = "Müşteriler başarıyla alındı."
            };
        }

        public ActionResponse<List<InvoiceDto>> GetInvoices()
        {
            var response = new ActionResponse<List<InvoiceDto>>();
            try
            {
                var invoices = _entityRepository.TableNoTracking
                    .Include(x => x.Items)
                    .OrderByDescending(x => x.CreDate)
                    .ToList();

                response.Entity = _mapper.Map<List<InvoiceDto>>(invoices);
                response.IsSuccess = true;
                _queueService.AddLog(invoices, "Faturalar listelendi.", _session.SecurityModel);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = new List<string> { ex.Message };
            }
            return response;
        }

        public ActionResponse<InvoiceDto> GetInvoiceById(long id)
        {
            var response = new ActionResponse<InvoiceDto>();
            try
            {
                var invoice = _entityRepository.TableNoTracking
                    .Include(x => x.Items)
                    .FirstOrDefault(x => x.Id == id);

                if (invoice == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = new List<string> { "Fatura bulunamadı." };
                    return response;
                }

                response.Entity = _mapper.Map<InvoiceDto>(invoice);
                response.IsSuccess = true;
                _queueService.AddLog(invoice, "Fatura detayı görüntülendi.", _session.SecurityModel);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = new List<string> { ex.Message };
            }
            return response;
        }

        public ActionResponse<bool> UpdateInvoice(long id, InvoiceUpdateDto model)
        {
            var response = new ActionResponse<bool>();
            try
            {
                var invoice = _entityRepository.Table.FirstOrDefault(x => x.Id == id);
                if (invoice == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = new List<string> { "Fatura bulunamadı." };
                    return response;
                }

                _mapper.Map(model, invoice);
                _entityRepository.Update(invoice);
                _queueService.AddLog(invoice, "Fatura güncellendi.", _session.SecurityModel);

                response.Entity = true;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = new List<string> { ex.Message };
            }
            return response;
        }

        public ActionResponse<bool> DeleteInvoice(long id)
        {
            var response = new ActionResponse<bool>();
            try
            {
                var invoice = _entityRepository.Table.FirstOrDefault(x => x.Id == id);
                if (invoice == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = new List<string> { "Fatura bulunamadı." };
                    return response;
                }

                _entityRepository.Delete(invoice);
                _queueService.AddLog(invoice, "Fatura silindi.", _session.SecurityModel);

                response.Entity = true;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = new List<string> { ex.Message };
            }
            return response;
        }

        public ActionResponse<bool> SendToGib(long id)
        {
            var response = new ActionResponse<bool>();
            try
            {
                var invoice = _entityRepository.Table
                    .Include(x => x.Items)
                    .FirstOrDefault(x => x.Id == id);

                if (invoice == null)
                {
                    response.IsSuccess = false;
                    response.ReturnMessage = new List<string> { "Fatura bulunamadı." };
                    return response;
                }

                // TODO: GIB Entegratör API'sine gönderim yapılacak
                // Şimdilik sadece status güncelliyoruz
                invoice.Status = InvoiceStatus.Sent;
                invoice.GibStatus = "PENDING";
                invoice.GibMessage = "Fatura GIB'e gönderildi, onay bekleniyor.";
                
                _entityRepository.Update(invoice);
                _queueService.AddLog(invoice, "Fatura GIB'e gönderildi.", _session.SecurityModel);

                response.Entity = true;
                response.IsSuccess = true;
                response.ReturnMessage = new List<string> { "Fatura GIB'e gönderildi. (Entegratör bağlantısı henüz yapılmadı)" };
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ReturnMessage = new List<string> { ex.Message };
            }
            return response;
        }
    }
}