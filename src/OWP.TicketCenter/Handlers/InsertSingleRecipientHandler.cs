using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using OWP.OtherSport.Notification.Models;
using OWP.OtherSport.Notification.Services;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Notification.Const;
using OWP.TicketCenter.Services;
using MessageTemplate = OWP.TicketCenter.Notification.Const.MessageTemplate;

namespace OWP.TicketCenter.Handlers
{
    public class InsertSingleRecipientRequest : InsertOrRemoveSingleRecipientModel, IRequest<int>
    {
    }

    public class InsertRecipientHandler : IRequestHandler<InsertSingleRecipientRequest, int>
    {
        private readonly Repository repository;
        private readonly RecipientService recipientService;
        private readonly NotifyServices notifyServices;

        public InsertRecipientHandler(Repository repository, RecipientService recipientService, NotifyServices notifyServices)
        {
            this.repository = repository;
            this.recipientService = recipientService;
            this.notifyServices = notifyServices;
        }

        public async Task<int> Handle(InsertSingleRecipientRequest request, CancellationToken cancellationToken)
        {
            var recipients = new List<InsertOrRemoveManyRecipientModel>()
            {
                new ()
                {
                   RecipientIds = new List<int>(){request.RecipientId},
                   RecipientType = request.RecipientType
                }
            };

            await recipientService.InsertManyRecipientReader(recipients, request.TicketId);
            var result = await repository.AddSingleRecipient(request);
            await notifyServices.Notify(CreateNotifyContent(request));
            return result;
        }

        private List<MessageModel> CreateNotifyContent(InsertSingleRecipientRequest request)
        {
            var notifyTemplate = MessageTemplate.NotificationTemplateMap[(int)NotificationTemplateCode.AddRecipient];

            return new List<MessageModel>
            {
                new MessageModel
                {
                    MessageContent = notifyTemplate.NotificationContent
                        .Replace("<ticketId>",$"{request.TicketId}")
                        .Replace("<Event>",request.TicketTitle)
                        .Replace("<username>",request.ActorUsername),
                    IsImportant = notifyTemplate.IsImportant,
                    ReceivedUserIds = new List<string>(){$"{request.RecipientId}"}
                }
            };
        }
    }
}