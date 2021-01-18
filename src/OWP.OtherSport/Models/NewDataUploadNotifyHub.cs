namespace OWP.OtherSport.Models
{
    using Microsoft.AspNetCore.SignalR;
    using System.Threading.Tasks;

    public class NewDataUploadNotifyHub : Hub
    {
        public async Task Notify()
        {
            await Clients.All.SendAsync("UploadedNewData");
        }
    }
}