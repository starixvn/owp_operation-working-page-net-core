namespace OWP.PrematchAssignment.Models;

public class QueryLogViewModel
{
    public IEnumerable<Log> Logs { get; set; }

    public bool IsLastPage { get; set; }
}