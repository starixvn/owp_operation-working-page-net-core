namespace OWP.TicketCenter.Models;
public record ConsolidationSubtypeModel
{
    public int TypeId { get; set; }

    public string TypeName { get; set; }

    public int SubTypeId { get; set; }

    public string SubTypeName { get; set; }
}
