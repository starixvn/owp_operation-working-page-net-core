namespace OWP.TicketCenter.ViewModels;

public class EventOptionInfoSender
{
    public IList<UserViewModel> UserViewModel { get; set; }
}

public class EventCheckingOptionInfoSender : EventOptionInfoSender
{
    public IList<EventCheckingOptionInfoViewModel> EventInfoViewModel { get; set; }
}

public class EventLgaOptionInfoSender : EventOptionInfoSender
{
    public IList<EventLgaOptionInfoViewModel> EventInfoViewModel { get; set; }
}