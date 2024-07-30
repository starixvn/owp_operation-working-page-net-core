namespace OWP.AutoAssignment.Models.PublicApi;

public class SchedulingMatch
{
    public List<RealMatchSender> Real { get; set; } = [];

    public List<SabaMatchSender> Saba { get; set; } = [];
}