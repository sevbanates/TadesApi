namespace TadesApi.Models.CustomModels;

public class CreatedTicketMailModel
{
    public long TicketId { get; set; }
    public string SenderName { get; set; }
    public List<string> Recievers { get; set; }
    public string TicketMessage { get; set; }
    public string TicketUrl { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public TicketPriority TicketPriority { get; set; }
    public TicketCategory TicketCategory { get; set; }
    public DateTime CreatedDate { get; set; }
}