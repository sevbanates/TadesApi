using TadesApi.Core.Security;

namespace TadesApi.Core.Session;

public interface ICurrentUser
{
    public SecurityModel SecurityModel { get; }
    public long UserId { get; set; }
    public string UserName { get; set; }
    public bool IsAdmin { get; set; }
    public bool User { get; set; }
    public bool IsConsultant { get; set; }
    public bool IsRoomClient { get; set; }
    public bool IsClient { get; set; }        
    public int RoleId { get; set; }
    public int LanguageId { get; set; }
    public bool IsCustomer { get; }
    public bool IsManager { get; }
    public bool IsSupplier { get; }
    public long? ClientId { get; set; }
}
