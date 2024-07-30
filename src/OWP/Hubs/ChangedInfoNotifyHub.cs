
using Microsoft.AspNetCore.SignalR;


namespace OWP.Hubs;

public interface ChangedInfoNotifyHub
{
    Task notifyChangedInformationData(object messageInfo); //implement from notification service 
}

public class ChangedInfoNotifyHubImp : Hub<ChangedInfoNotifyHub>;