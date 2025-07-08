using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadesApi.Models.CustomModels
{
    public class RoomInfo
    {
        public string RoomCode { get; set; } = null!;
        public bool ClientsCanChat { get; set; } = true;
        public bool ClientsMouseEnabled { get; set; } = false;
        public bool SharedCursorEnabled { get; set; } = false;
        public string CanvasData { get; set; } 
    }

    public class ChatMessage
    {
        public string Id { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public bool IsMine { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
    }

    public class MousePosition
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public int TimeOutHandle { get; set; }
    }
}
