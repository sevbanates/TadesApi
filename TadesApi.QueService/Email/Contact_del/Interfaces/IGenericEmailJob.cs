using VideoPortalApi.Core.Masstransit.Model;
using VideoPortalApi.Models.ViewModels.Inquiry;

namespace VideoPortalApi.QueService.Email.Contact.Interfaces;

public interface IGenericEmailJob
{
    public void SendGenericEmail(ReplyInquiryDto replyInquiryDto);
}