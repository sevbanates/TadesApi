using System.Collections.Generic;
using SendWithBrevo;
using TadesApi.Core;

namespace TadesApi.BusinessService.Common.Interfaces;

public interface IEmailHelper
{
    ActionResponse<bool> SendEmail(string subject, List<Recipient> recipients, Dictionary<string, string> parameters, int templateId);
}