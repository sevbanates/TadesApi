using System.ComponentModel.DataAnnotations;
using TadesApi.Core.Extensions;

namespace TadesApi.Models.ActionsEnum
{
    public enum AppController
    {
        [Display(Name = "User")]
        User = 1,

        [Display(Name = "SysControllerActionRole")]
        ControllerActionRole = 2,

        [Display(Name = "Invoice")]
        Invoice = 3,
        
        [Display(Name = "Customer")]
        Customer = 5,

        //[Display(Name = "ScheduleEvent")]
        //ScheduleEvent = 6, 
        
        //[Display(Name = "Inquiry")]
        //Inquiry = 10,
        
        //[Display(Name = "Message")]
        //Message = 11,
    }

    public static class AppControllerItems
    {
        public static List<EnumModel> GetListOfEnum()
        {
            return (Enum.GetValues(typeof(AppController)) as AppController[]).Select(c => new EnumModel() { Value = (int)c, Name = c.GetDisplayName() }).ToList();
        }
        public static List<KeyValuePair<string, byte>> GetEnumList<T>()
        {
            var list = new List<KeyValuePair<string, byte>>();
            foreach (var e in Enum.GetValues(typeof(T)))
            {
                list.Add(new KeyValuePair<string, byte>(((Enum)e).GetDisplayName(), (byte)e));
            }
            return list;
        }
    }
}
