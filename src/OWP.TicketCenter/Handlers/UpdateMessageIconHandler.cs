using MediatR;
using OWP.AccountSetting.Profile;
using OWP.TicketCenter.Criteria;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Services;
using OWP.TicketCenter.ViewModels;

namespace OWP.TicketCenter.Handlers;

public class UpdateMessageIconRequest : MessageIconModel, IRequest;

public class UpdateMessageIconHandler : IRequestHandler<UpdateMessageIconRequest>
{
    private readonly Repository repository;
    private readonly NotificationService notification;
    private readonly ProfileRepository profileRepository;

    public UpdateMessageIconHandler(Repository repository, NotificationService notification, ProfileRepository profileRepository)
    {
        this.repository = repository;
        this.notification = notification;
        this.profileRepository = profileRepository;
    }

    public async Task Handle(UpdateMessageIconRequest request, CancellationToken cancellationToken)
    {
        var criteria = new UpdateMessageIconCriteria
        {
            CreatorId = request.CreatorId,
            IconKey = request.IconKey,
            MessageId = request.MessageId
        };
        var action = await repository.UpdateMessageIcon(criteria);
        var (userName, avatarFileName) = await profileRepository.QueryUserProfileInfo();
        var userIds = await repository.GetUserIdsByTicketId(request.TicketId);
        var messageReactIcon = new MessageReactIcon
        {
            IconKey = request.IconKey,
            MessageId = request.MessageId,
            ReactUserId = request.CreatorId,
            ReactUserName = userName,
            ReactAvatarName = avatarFileName
        };

        await notification.NotifyUpdateReactIconMessage(userIds, messageReactIcon, action, request.TicketId);
    }
}