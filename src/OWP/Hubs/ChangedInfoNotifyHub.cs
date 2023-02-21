using Microsoft.AspNetCore.SignalR;

namespace OWP.Hubs;

public interface ChangedInfoNotifyHub
{
    Task notifyChangedInformationData(object messageInfo);
}

public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub> { }