using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TadesApi.Db.Entities;
using SendWithBrevo;
using TadesApi.BusinessService.AppServices;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.BusinessService.MessageServices.Interfaces;
using TadesApi.Core;
using TadesApi.Core.Models.ConstantKeys;
using TadesApi.Core.Models.Global;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Core.Session;
using TadesApi.CoreHelper;
using TadesApi.Db.Entities;
using TadesApi.Db.Infrastructure;
using TadesApi.Models.AppMessages;
using TadesApi.Models.Global;
using TadesApi.Models.ViewModels.Inquiry;
using TadesApi.Models.ViewModels.Message;

namespace TadesApi.BusinessService.MessageServices.Services;

public class MessageService : IMessageService
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<User> _userRepository;
    private readonly ILocalizationService _locManager;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _session;
    private readonly IEmailHelper _emailHelper;

    public MessageService(
        IRepository<Message> messageRepository,
        ILocalizationService locManager,
        IMapper mapper,
        ICurrentUser session,
        IRepository<User> userRepository,
        IEmailHelper emailHelper
       )
    {
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _locManager = locManager;
        _mapper = mapper;
        _session = session;
        _emailHelper = emailHelper;
    }


    public ActionResponse<MessageDto> GetMessageById(long messageId, Guid guidId)
    {
        var message = _messageRepository.TableNoTracking.FirstOrDefault(x => x.Id == messageId);
        if (message == null || message.GuidId != guidId)
            return new ActionResponse<MessageDto>().ReturnResponseError("Message not found");

        UpdateIsReadStatus(message);
        return new ActionResponse<MessageDto> { Entity = _mapper.Map<MessageDto>(message) };
    }


    public ActionResponse<MessageWithSenderAndRepliesDto> GetMessageWithReplies(long messageId, Guid guidId)
    {
        var message = _messageRepository.TableNoTracking
            .Include(x => x.ReplyMessages)
            .Include(x => x.Sender).FirstOrDefault(x => x.Id == messageId);
        if (message == null || message.GuidId != guidId || message.ReceiverId != _session.UserId)
            return new ActionResponse<MessageWithSenderAndRepliesDto>().ReturnResponseError("Message not found");

        UpdateIsReadStatus(message);
        return new ActionResponse<MessageWithSenderAndRepliesDto> { Entity = _mapper.Map<MessageWithSenderAndRepliesDto>(message) };
    }

    public PagedAndSortedResponse<MessageWithSenderDto> GetReceivedMessages(PagedAndSortedInput input)
    {
        var query = _messageRepository.TableNoTracking.Include(x => x.Sender)
            .Where(x => x.ReceiverId == _session.UserId && !x.IsDeletedByReceiver);

        var totalCount = query.Count();
        var pagedAndSortedData = CommonFunctions.GetPagedAndSortedData(query, input.Limit, input.Page, input.SortDirection, input.SortBy);
        var receivedMessages = _mapper.Map<List<MessageWithSenderDto>>(pagedAndSortedData);
        var toReturn = new PagedAndSortedResponse<MessageWithSenderDto>
        {
            EntityList = receivedMessages,
            TotalCount = totalCount
        };

        return toReturn;
    }

    public PagedAndSortedResponse<MessageWithReceiverDto> GetSentMessages(PagedAndSortedInput input)
    {
        var query = _messageRepository.TableNoTracking.Include(x => x.Receiver)
            .Where(x => x.SenderId == _session.UserId && !x.IsDeletedBySender);

        var totalCount = query.Count();
        var pagedAndSortedData = CommonFunctions.GetPagedAndSortedData(query, input.Limit, input.Page, input.SortDirection, input.SortBy);
        var sentMessages = _mapper.Map<List<MessageWithReceiverDto>>(pagedAndSortedData);
        var toReturn = new PagedAndSortedResponse<MessageWithReceiverDto>
        {
            EntityList = sentMessages,
            TotalCount = totalCount
        };

        return toReturn;
    }

    public ActionResponse<int> GetUnreadMessagesCount()
    {
        var unreadMessagesCount = _messageRepository.TableNoTracking.Count(x => x.ReceiverId == _session.UserId && !x.IsRead
            && !x.IsDeletedByReceiver);
        return new ActionResponse<int> { Entity = unreadMessagesCount };
    }

    public ActionResponse<CreateMessageDto> CreateMessage(CreateMessageDto message)
    {
        var toCreate = _mapper.Map<Message>(message);
        toCreate.SenderId = _session.UserId;
        _messageRepository.Insert(toCreate);

        var receiver = _userRepository.GetById(toCreate.ReceiverId);
        if (receiver == null || string.IsNullOrEmpty(receiver.Email)) return new ActionResponse<CreateMessageDto> { Entity = message };
        // Send notification email to receiver
        SendNewMessageNotificationEmail(receiver, toCreate.SenderId);

        return new ActionResponse<CreateMessageDto> { Entity = message };
    }

    private void SendNewMessageNotificationEmail(User receiver, long senderId)
    {
        var recipients = new List<Recipient>()
        {
            new (receiver.FirstName, receiver.Email)
        };
        var sender = _userRepository.GetById(senderId);
        var parameters = new Dictionary<string, string>
        {
            { EmailParams.FullName, sender.FirstName + " " + sender.LastName }
        };
        const string subject = "New Message Notification";
        //_emailHelper.SendEmail(subject, recipients, parameters, EmailTemplate.NewMessageNotification);
    }

    public ActionResponse<MessageDto> DeleteMessageForSender(long messageId, Guid guidId)
    {
        var toDelete = _messageRepository.TableNoTracking.FirstOrDefault(x => x.Id == messageId);
        if (toDelete == null || toDelete.GuidId != guidId || toDelete.SenderId != _session.UserId)
            return new ActionResponse<MessageDto>().ReturnResponseError("Message not found");

        toDelete.IsDeletedBySender = true;
        _messageRepository.Update(toDelete);
        return new ActionResponse<MessageDto> { Entity = _mapper.Map<MessageDto>(toDelete) };
    }

    public ActionResponse<MessageDto> DeleteMessageForReceiver(long messageId, Guid guidId)
    {
        var toDelete = _messageRepository.TableNoTracking.FirstOrDefault(x => x.Id == messageId);
        if (toDelete == null || toDelete.GuidId != guidId || toDelete.ReceiverId != _session.UserId)
            return new ActionResponse<MessageDto>().ReturnResponseError("Message not found");

        toDelete.IsDeletedByReceiver = true;
        _messageRepository.Update(toDelete);
        return new ActionResponse<MessageDto> { Entity = _mapper.Map<MessageDto>(toDelete) };
    }


    private void UpdateIsReadStatus(Message message)
    {
        if (message.IsRead || message.ReceiverId != _session.UserId) return;
        message.IsRead = true;
        _messageRepository.Update(message);
    }
}