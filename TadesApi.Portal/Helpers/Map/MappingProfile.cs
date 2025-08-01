using AutoMapper;
using TadesApi.Db.Entities;
using TadesApi.Models.ViewModels.WebPage;
using TadesApi.Models.ViewModels.WebPageContent;
using TadesApi.Models.ViewModels.WebSite;
using TadesApi.Core.Models.ViewModels.AuthManagement;
using TadesApi.Db.Entities;
using TadesApi.Models.ViewModels.AuthManagement;
using TadesApi.Models.ViewModels.Client;
using TadesApi.Models.ViewModels.Customer;
using TadesApi.Models.ViewModels.Inquiry;
using TadesApi.Models.ViewModels.Invoice;
using TadesApi.Models.ViewModels.Library;
using TadesApi.Models.ViewModels.Message;
using TadesApi.Models.ViewModels.PotentialClient;
using TadesApi.Models.ViewModels.ScheduleEvent;
using ClientConsultantLogDto = TadesApi.Models.ViewModels.Client.ClientConsultantLogDto;

namespace TadesApi.Portal.Helpers.Map;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //****************************************  CONST  ****************************************

        CreateMap<User, UserViewModel>();
        CreateMap<UserViewModel, User>();

        CreateMap<User, CreateUserDto>().ReverseMap();
        CreateMap<User, UpdateUserDto>().ReverseMap();
        CreateMap<User, UpdateProfileDto>().ReverseMap();


        CreateMap<SysControllerAction, SysControllerMenu2ViewModel>();
        CreateMap<SysControllerMenu2ViewModel, SysControllerAction>();

        CreateMap<SysControllerActionRole, SysControllerActionRoleViewModel>();
        CreateMap<SysControllerActionRoleViewModel, SysControllerActionRole>();

        CreateMap<SysRole, RoleBasicDto>().ReverseMap();

        //****************************************  BUSINESS  **************************************
        
    

       
    
        

       



        CreateMap<Library, LibraryItemDto>(); //.ForMember(dest => dest.ThumbImg, opt => opt.MapFrom(src => Convert.ToBase64String(src.ThumbImg)));
        CreateMap<LibraryItemDto, Library>();
        CreateMap<Library, CreateLibraryItemDto>().ReverseMap();
        CreateMap<Library, UpdateLibraryItemDto>().ReverseMap();
        CreateMap<Library, LibraryItemWithOwnerDto>().ReverseMap();

      

        CreateMap<SysControllerActionTotalViewModel, SysControllerActionTotal>();
        CreateMap<SysControllerActionTotal, SysControllerActionTotalViewModel>();

        CreateMap<Inquiry, InquiryDto>();
        CreateMap<InquiryDto, Inquiry>();
        CreateMap<InquiryWithReplyMessagesDto, Inquiry>().ReverseMap();
        CreateMap<ReplyInquiryDto, Inquiry>().ReverseMap();
        CreateMap<Inquiry, CreateInquiryDto>().ReverseMap();


        CreateMap<Message, MessageDto>().ReverseMap();
        CreateMap<Message, MessageWithSenderDto>().ReverseMap();
        CreateMap<Message, MessageWithReceiverDto>().ReverseMap();
        CreateMap<Message, MessageWithSenderAndRepliesDto>().ReverseMap();
        CreateMap<Message, CreateMessageDto>().ReverseMap();

        CreateMap<User, UserBasicDto>().ReverseMap();



        CreateMap<Invoice, InvoiceCreateDto>().ReverseMap();
        CreateMap<Invoice, InvoiceUpdateDto>().ReverseMap();
        CreateMap<Invoice, InvoiceDto>().ReverseMap();       
        
        CreateMap<Customer, CustomerCreateDto>().ReverseMap();
        CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
        CreateMap<Customer, CustomerDto>().ReverseMap();

        CreateMap<Ticket, TicketDto>().ReverseMap();
        CreateMap<Ticket, CreateTicketDto>().ReverseMap();
        CreateMap<Ticket, UpdateTicketDto>().ReverseMap();

        CreateMap<TicketMessage, TicketMessageDto>();
        //.ForMember(dest => dest.Attachments, opt => opt.MapFrom(src =>
        //    string.IsNullOrEmpty(src.Attachments)
        //        ? null
        //        : src.Attachments.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
        //));


    }
}