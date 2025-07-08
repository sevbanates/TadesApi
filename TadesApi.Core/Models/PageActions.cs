using System.Collections.Generic;
using TadesApi.Core.Models.Security;

namespace TadesApi.Core.Models
{


    public static class CityActions
    {
        public const string _ControllerName = "City";

        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {
            };
            return list;
        }
    }

    public static class ControllerMenuActions
    {
        public const string _ControllerName = "ControllerMenu";

        public const string _GenerateUILocalAction = "GenerateUILocalAction";
        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {

                new CustomAction { label = "GcUILocalAction", actionName = "GenerateUILocalAction",btnClass = "p-button-info p-button-text p-mr-2", orderNo = 5, isRequiredDescr = false, isOpenDialog = false, isConfirm = false, icon = "pi-file-o", isLocal=false, isFree=false}
            };
            return list;
        }
    }
    public static class SysControllerActions
    {
        public const string _ControllerName = "SysController";

        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {
            };
            return list;
        }
    }
    public static class SysMenuActions
    {
        public const string _ControllerName = "SysMenu";

        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {
            };
            return list;
        }
    }
    public static class TaxOfficeActions
    {
        public const string _ControllerName = "TaxOffice";

        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {
            };
            return list;
        }
    }
    public static class UsersActions
    {
        public const string _ControllerName = "Users";

        public const string _ChangePassword = "ChangePassword";
        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {

                new CustomAction { label = "Şifre Değiştir", actionName = "ChangePassword",btnClass = "p-button-info p-button-text p-mr-2", orderNo = 5, isRequiredDescr = false, isOpenDialog = false, isConfirm = false, icon = "pi pi-user-edit", isLocal=false, isFree=false}
            };
            return list;
        }
    }
    public static class SysControllerMenu2Actions
    {
        public const string _ControllerName = "SysControllerMenu2";

        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {

            };
            return list;
        }
    }
    public static class ContractActions
    {
        public const string _ControllerName = "Contract";

        public static List<CustomAction> Items()
        {
            var list = new List<CustomAction>
            {
            };
            return list;
        }
    }


}
