namespace OWP.Hubs;

using Microsoft.AspNetCore.SignalR;

public interface ChangedInfoNotifyHub // Redis backplane
{
    Task notifyChangedInformationData(object messageInfo);
}

public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub> { }