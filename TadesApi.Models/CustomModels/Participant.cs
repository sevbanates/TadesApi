using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TadesApi.Core.Models.ViewModels.AuthManagement;

namespace TadesApi.Models.CustomModels
{
    public record Participant(string RoomId, string UserId, string Name, ParticipantType ParticipantType, string ConnectionId)
    {
        public string RoomId { get; set; } = RoomId;
        public string Name { get; set; } = Name;
        public string ConnectionId { get; set; } = ConnectionId;
        public bool IsAdmited { get; set; }
    };

    public enum ParticipantType
    {
        Consultant,
        Student,
        SFU
    }
}
