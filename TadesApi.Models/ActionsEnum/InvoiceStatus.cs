namespace TadesApi.Models.ActionsEnum;

public enum InvoiceStatus
{
    Draft = 0,         // Taslak (henüz gönderilmedi)
    Pending = 1,       // Gönderim için hazır
    Sent = 2,          // GİB'e gönderildi
    Approved = 3,      // GİB tarafından kabul edildi
    Rejected = 4,      // GİB tarafından reddedildi
    Cancelled = 5,     // İptal edildi
    Archived = 6
}