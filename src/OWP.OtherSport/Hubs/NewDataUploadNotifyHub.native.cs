namespace OWP.OtherSport.Hubs;

using Microsoft.AspNetCore.SignalR;

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