namespace OWP.PreMatchAssignment.Handlers
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Services;
    //using GlobalNotificationService = OWP.Services.NotificationService;

    public class CreateNewIssueRequest : IRequest<QueryMessagesViewModel>
    {
        public string Message { get; set; }

        public int? OWPLeagueId { get; set; }

        public int? InfoSiteMatchId { get; set; }

        public int? FileId { get; set; }
    }

    public class CreateNewIssueHandler : IRequestHandler<CreateNewIssueRequest, QueryMessagesViewModel>
    {
        private readonly PreMatchAssignmentService preMatchAssignmentService;

        private readonly NotificationService notificationService;

        //private readonly GlobalNotificationService globalNotificationService;

        public CreateNewIssueHandler(PreMatchAssignmentService preMatchAssignmentService, NotificationService notificationService)
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
            this.notificationService = notificationService;
            //this.globalNotificationService = globalNotificationService;
        }

        public async Task<QueryMessagesViewModel> Handle(CreateNewIssueRequest request, CancellationToken cancellationToken)
        {
            await preMatchAssignmentService.CreateNewIssue(request);
            var messages = await preMatchAssignmentService.QueryIssueMessagesAfterInsert(request.OWPLeagueId, request.InfoSiteMatchId);
            var preMatchIssueId = messages.FirstOrDefault().PreMatchIssueId;
            var notificationMessage = new Message
            {
                MessageContent = request.Message,
                PreMatchIssueId = preMatchIssueId
            };

            var sessionId = await notificationService.Notify(notificationMessage, request.FileId);
            var notifyUsers = await preMatchAssignmentService.QueryNotificationReceivers(preMatchIssueId);
            //await globalNotificationService.Notify(sessionId, preMatchIssueId);
            PreMatchIssueNotifyHub.NewIssuesIncome(notifyUsers);

            var result = new QueryMessagesViewModel
            {
                Messages = messages,
                PreMatchIssueId = preMatchIssueId
            };

            return result;
        }
    }
}