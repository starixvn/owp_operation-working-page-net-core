namespace OWP.Hubs
{
    using Microsoft.AspNetCore.SignalR;
    

    public interface ChangedInfoNotifyHub
    {
        Task notifyChangedInformationData(object messageInfo);
    }

    public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub> { }
}