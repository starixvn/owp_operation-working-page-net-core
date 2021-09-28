namespace OWP.TicketCenter.Builder
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using ViewModels;

    public static class TwoLastCommentBuilder
    {
        public static IList<MessageDetail> GetTwoLastComment(IList<MessageModel> message)
        {
            return message.OrderByDescending(v => v.CreatedDate)
                .Take(2)
                .Select(i => new MessageDetail
                {
                    Message = i.Message,
                    CreatedDate = i.CreatedDate,
                    UserId = i.UserId,
                    Username = i.Username
                })

                .ToList();
        }
    }
}