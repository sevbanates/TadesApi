using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models
{

    public static class RolesConstants
    {
        public const string _Admin = "Admin";
        public const string _User = "User";
        public const string _Accounter = "Accounter";
    }

    public static class RolesHelper
    {
        public static class RolesConstantsInt
        {
            public const int _Admin = 100;
            public const int _User = 101;
            public const int _Accounter = 102;
        }
        public static int GetRoleId(string role)
        {
            switch (role)
            {
                case RolesConstants._Admin:
                    return 100;
                case RolesConstants._User:
                    return 101; 
                case RolesConstants._Accounter:
                    return 102;
     
                default:
                    throw new Exception("Role Not Found");
            }
        }
    }


}
