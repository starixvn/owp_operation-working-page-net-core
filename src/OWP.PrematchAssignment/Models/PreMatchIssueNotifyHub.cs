namespace OWP.PreMatchAssignment.Models
{
    
    using Microsoft.AspNet.SignalR;

    public class PreMatchIssueNotifyHub : Hub
    {
        public static void NewIssuesIncome(IList<string> userIds)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<PreMatchIssueNotifyHub>();
            var userClient = context.Clients.Users(userIds);
            userClient.notifyNewIssue();
        }

        public static void OpenIssueChatBoard(string userId, PreMatchIssueInfo preMatchIssueInfo)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<PreMatchIssueNotifyHub>();
            var userClient = context.Clients.User(userId);
            userClient.notifyIssueNotificationOnClick(preMatchIssueInfo);
        }
    }
}