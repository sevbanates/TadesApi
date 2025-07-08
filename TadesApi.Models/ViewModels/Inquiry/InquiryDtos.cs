using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Models.Global;

namespace TadesApi.Models.ViewModels.Inquiry
{
    public class CreateInquiryDto
    {
        [Required] [StringLength(50)] public string FirstName { get; set; }

        [Required] [StringLength(50)] public string LastName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)] public string PhoneNumber { get; set; }

        [StringLength(500)] public string Subject { get; set; }

        [Required] public string Message { get; set; }
    }

    public class UpdateInquiryDto : IBaseUpdateModel
    {
        [Required] public Guid GuidId { get; set; }
    }

    public class InquiryDto : BaseModel
    {
        public Guid GuidId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime? CreDate { get; set; }
        public bool IsRead { get; set; }
    }

    public class InquiryWithReplyMessagesDto : InquiryDto
    {
        public List<InquiryDto> ReplyMessages { get; set; }
    }

    public class ReplyInquiryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }
}