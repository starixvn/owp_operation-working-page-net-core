namespace OWP.TicketCenter.Builder
{
    using Models;
    using System.Collections.Generic;
    using System.Linq;

    public static class TwoLastCommentBuilder
    {
        public static void BuildTwoLastCommentOfEventTicket(IList<EventTicketBase> eventTickets, IList<EventTicketBaseRaw> eventInfo)
        {
            var twoLastCommentDic = GetTwoLastComment(eventInfo);
            foreach (var ticket in eventTickets)
            {
                IList<Comment> message;
                twoLastCommentDic.TryGetValue(ticket.TicketId, out message);
                if (message != null)
                {
                    ticket.Comments.AddRange(message);
                }
            }
        }

        public static Dictionary<int, IList<Comment>> GetTwoLastComment(IList<EventTicketBaseRaw> eventInfo)
        {
            var twoLastCommentDic = new Dictionary<int, IList<Comment>>();

            var groups = eventInfo
                .GroupBy(
                    m => m.TicketId,
                    (key, value) => new { key, value });
            foreach (var group in groups)
            {
                var twoLastComment = group.value
                    .OrderByDescending(v => v.CreatedDate)
                    .Take(2)
                    .Select(i => new Comment
                    {
                        Message = i.Message,
                        CreatedDate = i.CreatedDate,
                        UserId = i.UserId,
                        Username = i.Username
                    })

                    .ToList();

                twoLastCommentDic.Add(group.key, twoLastComment);
            }
            return twoLastCommentDic;
        }
    }
}