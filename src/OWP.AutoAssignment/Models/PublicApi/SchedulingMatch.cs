namespace OWP.AutoAssignment.Models.PublicApi;

public class SchedulingMatch
{
    public List<RealMatchSender> Real { get; set; } = new List<RealMatchSender>();

    public List<SabaMatchSender> Saba { get; set; } = new List<SabaMatchSender>();
}