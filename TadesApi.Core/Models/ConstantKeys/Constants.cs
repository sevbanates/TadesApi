namespace TadesApi.Core.Models.ConstantKeys;

public class RoleConstant
{
    public const int Admin = 100;
    public const int User = 101;

    //public const int SuperUser = 101;
    public const int Consultant = 102;
    public const int RoomClient = 103;
    public const int Client = 104;
}

public class EmailParams
{
    public const string FirstName = "FIRST_NAME";
    public const string LastName = "LAST_NAME";
    public const string FullName = "FULLNAME";
    public const string Consultant = "CONSULTANT";
    public const string Location = "LOCATION";
    public const string Date = "DATE";
    public const string StartsAt = "STARTS_AT";
    public const string EndsAt = "ENDS_AT";
    public const string RoomUrl = "ROOM_URL";
    public const string FormUrl = "FORM_URL";
    public const string Title = "TITLE";
    public const string Description = "DESCRIPTION";
    public const string Status = "STATUS";
    public const string SendIntakeFormLink = "SEND_INTAKE_FORM_LINK";
    public const string SendRoomLink = "SEND_ROOM_LINK";
    public const string Body = "BODY";
    public const string Phone = "PHONE";
    public const string Email = "E_MAIL";
    public const string UserName = "USERNAME";
    public const string DateOfBirth = "DATE_OF_BIRTH";
    public const string Service = "SERVICE";
}

public class EmailTemplate
{
    public const int ScheduledEvent = 1;
    public const int ForgotPassword = 2;
    public const int LoginConfirm = 3;
    public const int NewMessageNotification = 4;
    public const int InquiryReply = 5;
    public const int CreateInquiry = 6;
    public const int AccountCreated = 7;
    public const int IntakeFormSubmitted = 8;
}