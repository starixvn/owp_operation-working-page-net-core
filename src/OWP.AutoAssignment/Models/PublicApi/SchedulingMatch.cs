namespace OWP.AutoAssignment.Models.PublicApi;

public class SchedulingMatch
{
    public List<RealMatchSender> Real { get; set; } = new();

    public List<SabaMatchSender> Saba { get; set; } = new();
}