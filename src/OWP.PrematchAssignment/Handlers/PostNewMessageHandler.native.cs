namespace OWP.PreMatchAssignment.Handlers
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Services;
    //using GlobalNotificationService = OWP.Services.NotificationService;

    public class PostNewMessageRequest : IRequest<IEnumerable<Message>>
    {
        public int PreMatchIssueId { get; set; }

        public string Message { get; set; }

        public bool IsPreMatch { get; set; }

        public int? FileId { get; set; }

        public bool IsShowAllMessage { get; set; }
    }

    public class PostNewMessageHandler : IRequestHandler<PostNewMessageRequest, IEnumerable<Message>>
    {
        private readonly PreMatchAssignmentService preMatchAssignmentService;

        //private readonly NotificationService notificationService;

        //private readonly GlobalNotificationService globalNotificationService;

        public PostNewMessageHandler(PreMatchAssignmentService preMatchAssignmentService)
        {
            this.preMatchAssignmentService = preMatchAssignmentService;
            //this.notificationService = notificationService;
            //this.globalNotificationService = globalNotificationService;
        }

        public async Task<IEnumerable<Message>> Handle(PostNewMessageRequest request, CancellationToken cancellationToken)
        {
            await preMatchAssignmentService.PostNewMessage(request);
            var notificationMessage = new Message
            {
                MessageContent = request.Message,
                PreMatchIssueId = request.PreMatchIssueId
            };

            //var sessionId = await notificationService.Notify(notificationMessage, request.FileId, request.IsPreMatch);
            var notifyUsers = await preMatchAssignmentService.QueryNotificationReceivers(request.PreMatchIssueId);
            //await globalNotificationService.Notify(sessionId, request.PreMatchIssueId);
            //PreMatchIssueNotifyHub.NewIssuesIncome(notifyUsers);

            return await preMatchAssignmentService.QueryIssueMessages(request.PreMatchIssueId, request.IsShowAllMessage);
        }
    }
}