namespace OWP.TicketCenter.ViewModels;

public class EventOptionInfoSender
{
    public IList<UserViewModel> UserViewModel { get; set; }
}

public class EventCheckingOptionInfoSender : EventOptionInfoSender
{
    public IList<EventCheckingOptionInfoViewModel> EventInfoViewModel { get; set; }
}

public class EventLGAOptionInfoSender : EventOptionInfoSender
{
    public IList<EventLGAOptionInfoViewModel> EventInfoViewModel { get; set; }
}