namespace OWP.TicketCenter.Builder
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public static class TwoLastCommentBuilder
    {
        public static IList<MessageDetailViewModel> GetTwoLastComment(IList<MessageModel> message)
        {
            return message.OrderByDescending(v => v.CreatedDate)
                .Take(2)
                .Select(i => new MessageDetailViewModel
                {
                    MessageId = i.MessageId,
                    Message = i.Message,
                    CreatedDate = i.CreatedDate,
                    UserId = i.UserId,
                    Username = i.Username
                })

                .ToList();
        }
    }
}