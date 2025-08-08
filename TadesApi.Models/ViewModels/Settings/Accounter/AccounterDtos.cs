using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Models.ActionsEnum;

namespace TadesApi.Models.ViewModels.Settings.Accounter
{
    public class AccounterRequestDto
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public string TargetFullName { get; set; }
        public AccounterRequestStatus Status { get; set; }
        public DateTime CreDate { get; set; }
        public DateTime ModDate { get; set; }
    }      
    
    public class ChangeAccounterRequestStatusDto
    {
        public long Id { get; set; }
        public Guid GuidId { get; set; }
        public AccounterRequestStatus Status { get; set; }
    }   
    public class CreateAccounterRequestDto
    {
        public string TargetEmail { get; set; }
    }

}
