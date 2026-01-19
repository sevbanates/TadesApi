using TadesApi.Core;
using TadesApi.Db.PartialEntites;

namespace TadesApi.Db.Entities;

public class UserPreferences : BaseEntity, ISoftDeletable
{
    public long UserId { get; set; } // Accounterin kendisi
    public long? SelectedUserId { get; set; } // Seçtiği kullanıcı (null olabilir)
    public string PreferenceKey { get; set; } // "SELECTED_USER" gibi
    public string PreferenceValue { get; set; } // JSON data veya basit değer
    public bool IsDeleted { get; set; }
    
    // Navigation properties
    public virtual User User { get; set; }
    public virtual User? SelectedUser { get; set; }
}









