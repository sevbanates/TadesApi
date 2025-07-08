using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoPortalApi.QueService.Enum
{
    public static class RabbitMQFileSettings
    {
        public static string ResourceTemplateFolder => $"{Path.Combine(Directory.GetParent(Environment.CurrentDirectory).FullName, "Resource", "Template")}";
        public static string ApplicationUrl => $"http://localhost:5000/sign-in";
        public static string ResetPasswordUrl => "http://localhost:5000/reset-password/{0}";

    }
}
