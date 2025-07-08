using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core.Masstransit.Model;

namespace VideoPortalApi.QueService.Email.AuthManagement.Interfaces
{
    public interface IAuthMailJob
    {
        public bool SendLoginConfirmationMail(LoginConfirmMailModel loginConfirmMailModel);
        public bool SendForgotPasswordMail(ForgotPasswordMailModel forgotPasswordMailModel);
        public bool SendGeneratedUserMail(NewCustomerMailModel customerMailmodel);

    }
}
