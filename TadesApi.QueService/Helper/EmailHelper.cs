using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core.Email;
using VideoPortalApi.Db.Entities;

namespace VideoPortalApi.QueService.Helper
{
    public static class EmailHelper
    {
        public static EmailMessage SetMailReceiver(EmailMessage mail, User userRec)
        {
            mail.ToAdd(userRec.Email, userRec.FirstName + " " + userRec.LastName);
            return mail;
        }
    }
}
