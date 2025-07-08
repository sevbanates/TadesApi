using System.Collections.Generic;
using SendWithBrevo;
using TadesApi.BusinessService.Common.Interfaces;
using TadesApi.Core;

namespace TadesApi.BusinessService.Common.Services;

public class EmailHelper : IEmailHelper
{
    public ActionResponse<bool> SendEmail(string subject, List<Recipient> recipients, Dictionary<string, string> parameters, int templateId)
    {
        var client = new BrevoClient("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");
        var sender = new Sender("Global Psychoeducational Solutions", "admin@globalpsychsolutions.com");
        var response = client.SendAsync(sender, subject: subject, content: "GPS", to: recipients, parameters: parameters,
            templateId: templateId, isHtml: true).Result;
        return new ActionResponse<bool> { IsSuccess = response, Entity = response };
    }
}