namespace OWP.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public interface ChangedInfoNotifyHub
    {
        Task notifyChangedInformationData(object messageInfo);
    }

    public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub> { }
}