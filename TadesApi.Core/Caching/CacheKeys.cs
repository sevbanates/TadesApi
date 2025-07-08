using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace TadesApi.Core.Caching
{
    public class CacheKeys
    {
        public readonly string UserKey;
        public readonly string SecurityToken;
        public readonly string LoginConfirmCode;
        public readonly string ControllerSecurity;
        public readonly string ControllerActionTotal;

        public string TenantId { get; set; }

        public static object lockActionObject = new Object();
        public static object lockControllerObject = new Object();

        public readonly IOptions<AppConfigs> _vbtConfig;
        //public readonly ICurrentUser _currentUser;

        public CacheKeys(IOptions<AppConfigs> vbtConfig)
        {
            _vbtConfig = vbtConfig;
            //_currentUser = currentUser;

            UserKey = "User:{0}";
            SecurityToken = "User:{0}:SecurityToken";
            LoginConfirmCode = "User:{0}:ConfirmCode";
            ControllerSecurity = "ControllerSecurity";
            ControllerActionTotal = "ControllerActionTotal{0}{1}";
        }
    }  
}
