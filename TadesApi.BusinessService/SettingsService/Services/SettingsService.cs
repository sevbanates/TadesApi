using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SendWithBrevo;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.CommonServices.interfaces;
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
        private readonly IRepository<AccounterUsers> _accUsersRepository;
        private readonly IRepository<User> _userRepo;
        private readonly IQueueService _queueService;
        private readonly IOptions<FileSettings> _fileSetting;
        protected readonly BtcDbContext _dbContext;
        protected readonly ICurrentUser _session;
        public SettingsService(IRepository<Ticket> entityRepository, ILocalizationService locManager, IMapper mapper, ICurrentUser session, IRepository<TicketMessage> messageRepo, IRepository<User> userRepo, BtcDbContext dbContext, IRepository<AccounterRequest> entityRepository1, IQueueService queueService, IOptions<FileSettings> fileSetting, IRepository<AccounterUsers> accUsersRepository)
        {
            _session = session;
            _messageRepo = messageRepo;
            _userRepo = userRepo;
            _dbContext = dbContext;
            _entityRepository = entityRepository1;
            _queueService = queueService;
            _fileSetting = fileSetting;
            _accUsersRepository = accUsersRepository;
        }


        public ActionResponse<AccounterRequestDto> CreateAccounterRequest(CreateAccounterRequestDto dto)
        {
           var response = new ActionResponse<AccounterRequestDto>();
            try
            {
                var targetUser = _userRepo.TableNoTracking.FirstOrDefault(x => x.Email == dto.TargetEmail);
                var senderUser = _userRepo.TableNoTracking.FirstOrDefault(x => x.Id == _session.UserId);
                if (targetUser == null)
                {
                    response.ReturnResponseError("Girdiğiniz mail e ait kullanıcı bulunamadı");
                    return response;
                }

                //var draftRequest = _entityRepository.TableNoTracking
                //    .FirstOrDefault(x => x.TargetId == user.Id && x.Status == AccounterRequestStatus.Draft);

                //if (draftRequest != null)
                //{
                //    response.ReturnResponseError("Beklemede olan isteğiniz vardır.");
                //    return response;
                //}

                //var approvedRequest = _entityRepository.TableNoTracking
                //    .FirstOrDefault(x => x.TargetId == user.Id && x.Status == AccounterRequestStatus.Approved);

                //if (approvedRequest != null)
                //{
                //    response.ReturnResponseError("Zaten kaydınız vardır.");
                //    return response;
                //}
                var accounterRequest = new AccounterRequest
                {
                    TargetId = targetUser.Id,
                    Status = AccounterRequestStatus.Draft,
                    CreDate = DateTime.UtcNow,
                    SenderId = _session.UserId
                };
                _entityRepository.Insert(accounterRequest);
                response.Entity = new AccounterRequestDto
                {
                    Id = accounterRequest.Id,
                    GuidId = accounterRequest.GuidId,
                    TargetFullName = targetUser.FirstName + " " + targetUser.LastName,
                    Status = accounterRequest.Status,
                    CreDate = accounterRequest.CreDate
                };
                
                var subject = "Muhasebeci Daveti";
                var senderFullName = senderUser.FirstName + " " + senderUser.LastName;
                var approveLink = $"{_fileSetting.Value.BaseUrl ?? "http://localhost:44559"}/api/settings/accounter-request/approve?id={accounterRequest.Id}&guid={accounterRequest.GuidId}";
                var messageText = "Muhasebeci daveti aldınız. Onaylamak için butona tıklayın.";
                _queueService.SendAccounterRequestMail(accounterRequest, subject, messageText, targetUser.Email, targetUser.FirstName + " " + targetUser.LastName, approveLink, senderFullName);
                
                return response;
            }
            catch (Exception ex)
            {
                response.ReturnResponseError("CreateAccounterRequest Error: " + ex.Message);
            }
            return response;
        }

        public ActionResponse<AccounterRequestDto> GetRequests()
        {
            var response = new ActionResponse<AccounterRequestDto>();

            if (_session.IsAccounter)
            {
                var requests = _entityRepository.TableNoTracking.Where(x => x.TargetId == _session.UserId).ToList();

                foreach (var item in requests)
                {
                    var user = _userRepo.TableNoTracking.FirstOrDefault(x => x.Id == item.SenderId);
                    var entity = new AccounterRequestDto
                    {
                        CreDate = item.CreDate,
                        GuidId = item.GuidId,
                        Id = item.Id,
                        Status = item.Status,
                        TargetFullName = user.FirstName + " " + user.LastName
                    };
                    response.EntityList.Add(entity);
                }

                return response;
            }
            else if (_session.User)
            {
                var requests = _entityRepository.TableNoTracking.Where(x => x.SenderId == _session.UserId).ToList();
                foreach (var item in requests)
                {
                    var user = _userRepo.TableNoTracking.FirstOrDefault(x => x.Id == item.TargetId);
                    var entity = new AccounterRequestDto
                    {
                        CreDate = item.CreDate,
                        GuidId = item.GuidId,
                        Id = item.Id,
                        Status = item.Status,
                        TargetFullName = user.FirstName + " " + user.LastName,
                        TargetEmail = user.Email
                    };
                    response.EntityList.Add(entity);
                }
            }
            else
            {
                var requests = _entityRepository.TableNoTracking.ToList();
                foreach (var item in requests)
                {
                    var targetUser = _userRepo.TableNoTracking.FirstOrDefault(x => x.Id == item.TargetId);
                    var senderUser = _userRepo.TableNoTracking.FirstOrDefault(x => x.Id == item.SenderId);
                    var entity = new AccounterRequestDto
                    {
                        CreDate = item.CreDate,
                        GuidId = item.GuidId,
                        Id = item.Id,
                        Status = item.Status,
                        TargetFullName = targetUser.FirstName + " " + targetUser.LastName,
                        TargetEmail = targetUser.Email,
                        SenderFullName = senderUser.FirstName + " " + senderUser.LastName,
                        SenderEmail = senderUser.Email

                    };
                    response.EntityList.Add(entity);
                }
            }

            return response;

        }

        public ActionResponse<AccounterRequestDto> ChangeStatus(AccounterRequestDto dto)
        {
            var response = new ActionResponse<AccounterRequestDto>();
           var request =  _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == dto.Id && x.GuidId == dto.GuidId);
           if (request == null)
           {
               return response.ReturnResponseError("İşleme dair bi istek bulunamadı.");
           }
           else
           {
               request.Status = dto.Status;
               request.ModDate = DateTime.UtcNow;
               _entityRepository.Update(request);

               AccounterUsers acUser = new AccounterUsers
               {
                   AccounterUserId = request.TargetId,
                   TargetUserUserId = request.SenderId,
               };
               _accUsersRepository.Insert(acUser);
                dto.ModDate = request.ModDate;
               response.Entity = dto;
               return response;
           }
        }
    }
   
}