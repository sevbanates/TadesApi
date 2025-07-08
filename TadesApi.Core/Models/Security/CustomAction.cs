using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Core.Models.Security
{
    public class CustomAction
    {
        public string label { get; set; }
        public string btnClass { get; set; }
        public string actionName { get; set; }
        public string stateName { get; set; }
        public int orderNo { get; set; } = 0;
        public bool isOpenDialog { get; set; } = false;
        public bool isRequiredDescr { get; set; } = true;
        public string icon { get; set; }
        public bool isAvaliable { get; set; } = false;
        public bool isConfirm { get; set; } = false;
        public bool isFree { get; set; } = false;
        public bool isLocal { get; set; } = false;
        public bool isToolbar { get; set; } = false;
    }
    public class InquiryAction
    {
        public string label { get; set; }
        public string btnClass { get; set; }
        public string actionName { get; set; }
        public string icon { get; set; }
        public int orderNo { get; set; } = 0;

    }
}
