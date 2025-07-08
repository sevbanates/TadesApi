using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoPortalApi.Core.Masstransit.Model;

namespace VideoPortalApi.QueService.Email.RoomInviteManagement.Interfaces
{
    public interface IRoomInviteJob
    {
        public bool SendRoomInvite(RoomInviteMailModel roomInviteMailModel);
    }
}
