using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.InvoiceServices.Interfaces;
using TadesApi.BusinessService.SettingsService.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.ActionsEnum;
using TadesApi.Models.AppMessages;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Settings.Accounter;

namespace TadesApi.BusinessService.SettingsService.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IRepository<TicketMessage> _messageRepo;
        private readonly IRepository<AccounterRequest> _entityRepository;
        private readonly IRepository<User> _userRepo;
        protected readonly BtcDbContext _dbContext;
        public SettingsService(IRepository<Ticket> entityRepository, ILocalizationService locManager, IMapper mapper, ICurrentUser session, IRepository<TicketMessage> messageRepo, IRepository<User> userRepo, BtcDbContext dbContext, IRepository<AccounterRequest> entityRepository1)
        {
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _dbContext = dbContext;
            _entityRepository = entityRepository1;
        }


        public ActionResponse<AccounterRequestDto> CreateAccounterRequest(CreateAccounterRequestDto dto)
        {
           var response = new ActionResponse<AccounterRequestDto>();
            try
            {
                var user = _userRepo.TableNoTracking.FirstOrDefault(x => x.Email == dto.TargetEmail);
                if (user == null)
                {
                    response.ReturnResponseError("Girdiðiniz mail e ait kullanýcý bulunamadý");
                    return response;
                }

                var draftRequest = _entityRepository.TableNoTracking
                    .FirstOrDefault(x => x.TargetId == user.Id && x.Status == AccounterRequestStatus.Draft);

                if (draftRequest != null)
                {
                    response.ReturnResponseError("Beklemede olan isteðiniz vardýr.");
                    return response;
                }

                var approvedRequest = _entityRepository.TableNoTracking
                    .FirstOrDefault(x => x.TargetId == user.Id && x.Status == AccounterRequestStatus.Approved);

                if (approvedRequest != null)
                {
                    response.ReturnResponseError("Zaten kaydýnýz vardýr.");
                    return response;
                }
                var accounterRequest = new AccounterRequest
                {
                    TargetId = user.Id,
                    Status = AccounterRequestStatus.Draft,
                    CreDate = DateTime.UtcNow
                };
                _entityRepository.Insert(accounterRequest);
                response.Entity = new AccounterRequestDto
                {
                    Id = accounterRequest.Id,
                    GuidId = accounterRequest.GuidId,
                    TargetFullName = user.FirstName + " " + user.LastName,
                    Status = accounterRequest.Status,
                    CreDate = accounterRequest.CreDate
                };
                return response;
            }
            catch (Exception ex)
            {
                response.ReturnResponseError("CreateAccounterRequest Error: " + ex.Message);
            }
            return response;
        }
    }
   
}