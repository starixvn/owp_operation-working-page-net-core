using Microsoft.AspNetCore.SignalR;


namespace OWP.Hubs;

public interface ChangedInfoNotifyHub // Redis backplane
{
    Task notifyChangedInformationData(object messageInfo);
}

public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub> { }