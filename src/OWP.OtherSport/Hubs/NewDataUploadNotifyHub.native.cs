using Microsoft.AspNetCore.SignalR;

namespace OWP.OtherSport.Hubs;

public interface NewDataUploadNotifyHub
{
    Task NotifyUploadedNewData();
}

public class NewDataUploadNotifyHubImp : Hub<NewDataUploadNotifyHub>
{
    public async Task Notify()
    {
        await Clients.All.NotifyUploadedNewData();
    }
}