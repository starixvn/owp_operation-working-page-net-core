namespace OWP.TicketCenter.ViewModels;
public record ConsolidationSubtypeViewModel
{

    public int TypeId { get; set; }

    public int SubTypeId { get; set; }

    public  string Label { get; set; }
    public bool IsRead { get; set; }
}
