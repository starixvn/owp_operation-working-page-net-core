namespace OWP.PreMatchNotification.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public interface PreMatchInfoNotifyHub
    {
        Task notifyChangedInfomationData(object messageInfo);
    }

    public class PreMatchInfoNotifyHubImp : Hub<PreMatchInfoNotifyHub> { }
}