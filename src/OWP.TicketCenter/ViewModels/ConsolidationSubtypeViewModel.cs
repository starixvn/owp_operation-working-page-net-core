namespace OWP.TicketCenter.ViewModels;
public record ConsolidationSubtypeViewModel
{
    public int Id { get; set; }

    public  string Label { get; set; }

    public IList<ConsolidationSubtypeViewModel> ChildNode { get; set; }
}
