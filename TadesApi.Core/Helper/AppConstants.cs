using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppConstants
{

    public static class DefActions
    {
        public const string _Save = "Save";
        public const string _Show = "Show";
        public const string _Delete = "Delete";
        public const string _List = "List";

        private static string[] Items()
        {
            List<string> vs = new List<string>();
            vs.Add("Save");
            vs.Add("Show");
            vs.Add("Delete");
            vs.Add("List");
            return vs.ToArray();
        }
    }
   
}
