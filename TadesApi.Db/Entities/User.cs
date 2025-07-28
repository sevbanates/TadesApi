using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TadesApi.Db.Entities;
using TadesApi.Core;
using TadesApi.Db.PartialEntites;


namespace TadesApi.Db.Entities
{
    public partial class User : BaseEntity, ISoftDeletable
    {
        public long CompanyId { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string Email { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public long? RefId { get; set; }
        public string LastIpAddress { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public string PasswordSalt2 { get; set; }
        public string ChangePassReq { get; set; }
        public DateTime? ChangePassDate { get; set; }
        public string ChangePassCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string UserLogoPath { get; set; }
        public bool? IsFirstLogin { get; set; }

        public int RoleId { get; set; }
        
        public string LoginCode { get; set; }
        public DateTime? LoginCodeExpireDate { get; set; }
        
        //public long? ClientId { get; set; }
        
        public bool IsPrimaryForClient { get; set; }
        
        public virtual SysRole Role { get; set; }

        //[InverseProperty("Consultant")] public virtual ICollection<Client> Clients { get; set; } = new List<Client>();
        //public virtual ICollection<ScheduleEvent> ScheduleEvents { get; set; } = new List<ScheduleEvent>();
        
        public long? AccounterId { get; set; }
        public virtual User? Accounter { get; set; }
        public virtual ICollection<User> Clients { get; set; } = new List<User>();
    }
}