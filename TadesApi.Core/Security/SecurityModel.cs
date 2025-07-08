using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Security
{
    public class SecurityModel
    {
        public string Token { get; set; }
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public int TenantId { get; set; }
        public long CompanyId { get; set; }
        public int[] selectedRoles { get; set; }
        public bool IsRefreshToken { get; set; } = false;
        public int LanguageId { get; set; }
        
        public long? ClientId { get; set; }
    }
}
