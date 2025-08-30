using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SendWithBrevo;
using System;
using System.Collections.Generic;
using System.Linq;
using TadesApi.BusinessService._base;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.BusinessService.CommonServices.interfaces;
using TadesApi.BusinessService.InquiryServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ConstantKeys;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.ViewModels.Inquiry;

namespace TadesApi.BusinessService.InquiryServices.Services;

public class InquiryService : BaseServiceNg<Inquiry, InquiryDto, CreateInquiryDto, UpdateInquiryDto>, IInquiryService
{
    private readonly IEmailHelper _emailHelper;
    
    public InquiryService(
        IRepository<Inquiry> entityRepository,
        ILocalizationService locManager,
        IMapper mapper,
        IEmailHelper emailHelper,
        ICurrentUser session, IQueueService queueService) : base(entityRepository,
        locManager, mapper, session, queueService)
    {
        _emailHelper = emailHelper;
    }

    public PagedAndSortedResponse<InquiryWithReplyMessagesDto> GetInquiriesWithReplyMessages(PagedAndSortedSearchInput input)
    {
        var query = _entityRepository.TableNoTracking
            .Include(x => x.ReplyMessages)
            .Where(x => x.ReplyMessageId == null)
            .WhereIf(!string.IsNullOrEmpty(input.Search),
                x => x.FirstName.Contains(input.Search) || x.Email.Contains(input.Search) || x.LastName.Contains(input.Search));

        var totalCount = query.Count();
        var toReturn = CommonFunctions.GetPagedAndSortedData(query, input.Limit, input.Page, input.SortDirection, input.SortBy);
        return new PagedAndSortedResponse<InquiryWithReplyMessagesDto>
        {
            EntityList = _mapper.Map<List<InquiryWithReplyMessagesDto>>(toReturn),
            TotalCount = totalCount
        };
    }
    

    public ActionResponse<InquiryDto> GetSingle(long id, Guid guidId)
    {
        var inquiry = _entityRepository.TableNoTracking.FirstOrDefault(x => x.Id == id);
        if (inquiry == null || inquiry.GuidId != guidId)
            return new ActionResponse<InquiryDto>().ReturnResponseError("Inquiry not found.");

        if (inquiry.IsRead) return new ActionResponse<InquiryDto> { Entity = _mapper.Map<InquiryDto>(inquiry) };

        // mark the inquiry as read
        inquiry.IsRead = true;
        _entityRepository.Update(inquiry);
        return new ActionResponse<InquiryDto> { Entity = _mapper.Map<InquiryDto>(inquiry) };
    }
    
    public ActionResponse<InquiryDto> CreateInquiry(CreateInquiryDto input)
    {
        var inquiry = _mapper.Map<Inquiry>(input);
        _entityRepository.Insert(inquiry);
        
        // send email to the support
        var recipients = new List<Recipient>
        {
            new(email: "admin@globalpsychsolutions.com", name: "Support"),
        };
        var parameters = new Dictionary<string, string>
        {
            { EmailParams.FullName, input.FirstName + " " + input.LastName },
            { EmailParams.Email, input.Email },
            { EmailParams.Phone, input.PhoneNumber },
            { EmailParams.Title, input.Subject },
            { EmailParams.Body, input.Message }
        };
        const string subject = "New Contact Form Submission";
        //_emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.CreateInquiry);
        
        return new ActionResponse<InquiryDto> { Entity = _mapper.Map<InquiryDto>(inquiry) };
    }

    public ActionResponse<bool> SendReplyMessage(long id, ReplyInquiryDto input)
    {
       SendInquiryReplyEmail(input);

        // create a new record in inquiry table
        var replyMessage = _mapper.Map<Inquiry>(input);
        replyMessage.ReplyMessageId = id;
        _entityRepository.Insert(replyMessage);
        return new ActionResponse<bool> { Entity = true };
    }

    private void SendInquiryReplyEmail(ReplyInquiryDto input)
    {
        var recipients = new List<Recipient>()
        {
            new(input.FirstName, input.Email)
        };
        var parameters = new Dictionary<string, string>
        {
            { EmailParams.FullName, input.FirstName + " " + input.LastName },
            { EmailParams.Body, input.Message }
        };
        const string subject = "Thank you for your interest";
        //_emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.InquiryReply);
    }
}