namespace OWP.OtherSport.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public interface ChangedInfoNotifyHub
    {
        Task notifyChangedInfomationData(object messageInfo);
    }

    public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub> { }
}