using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Security;

namespace TadesApi.Core.Session
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public HttpContext Current => _httpContextAccessor.HttpContext;
        public SecurityModel SecurityModel
        {
            get
            {
                if (Current == null)
                    return new SecurityModel();

                if (_httpContextAccessor.HttpContext.Items["SecurityModel"] == null)
                    return new SecurityModel();

                return (SecurityModel)(_httpContextAccessor.HttpContext.Items["SecurityModel"]);
            }
        }

        public long UserId { get => SecurityModel.UserId; set { } }
        public string UserName { get => SecurityModel.UserName; set { } }
        public string Email { get => SecurityModel.EmailAddress; set { } }
        public bool IsAdmin { get => SecurityModel.RoleId == 100; set { } }
        public bool User { get => SecurityModel.RoleId == 101; set { } }
        public bool IsAccounter { get => SecurityModel.RoleId == 102; set { } }
        //public bool IsConsultant { get => SecurityModel.RoleId == 102; set { } }
        //public bool IsRoomClient { get => SecurityModel.RoleId == 103; set { } }
        //public bool IsClient { get => SecurityModel.RoleId == 104; set { } }
        public int RoleId { get => SecurityModel.RoleId; set { } }
        public int LanguageId { get => SecurityModel.LanguageId; set { } }
        
        public long? SelectedUserId 
        { 
            get => Current?.Items["SelectedUserId"] as long?; 
            set => Current?.Items.TryAdd("SelectedUserId", value);
        }

        public DateTime Created { get; set; }
        //public bool IsCustomer { get => SecurityModel.RoleId == 104; set { } }
        //public bool IsManager { get => SecurityModel.RoleId == 105; set { } }
        //public bool IsSupplier { get => SecurityModel.RoleId == 106; set { } }
        //public long? ClientId { get => SecurityModel.ClientId; set { } }
    }
}
