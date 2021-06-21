namespace OWP.PreMatchNotification.Services
{
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Fanex.NAP.Client.Helpers.Web;
    using OWP.PreMatchNotification.Constants;
    using OWP.PreMatchNotification.Models;

    public interface MessageFactory
    {
        List<MessageModel> Transform(IList<ChangedValue> changedValues);
    }

    public class MessageFactoryImp : MessageFactory
    {
        #region MessageTemplate

        private static readonly ImmutableList<MessageTemplate> messageTemplates = ImmutableList<MessageTemplate>.Empty.AddRange(new[]
        {
            new MessageTemplate
            {
                MessageType = (int)MsgTemplateCode.NewLeague,
                MessageContent = "<b>New Add</b> <League>",
                IsImportant = true,
                IsBlinking = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.DayShiftAssignmentInsert,
                 MessageContent="[DayShift] You have NEW ASSIGNMENT <bettype> in <League>",
                 IsImportant = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.DayShiftAssignmentDelete,
                 MessageContent="[DayShift] You have been REMOVED from <bettype> in <League>",
                 IsImportant = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.DayShiftOtherAssignmentUpdate,
                 MessageContent="[DayShift] <bettype>'s Assignee has been updated from <old value> to <newvalue> in your assignment <League>",
                 IsImportant = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.NightShiftAssignmentInsert,
                 MessageContent="[NightShift] You have NEW ASSIGNMENT <bettype> in <League>",
                 IsImportant = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.NightShiftAssignmentDelete,
                 MessageContent="[NightShift] You have been REMOVED from <bettype> in <League>",
                 IsImportant = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.NightShiftOtherAssignmentUpdate,
                 MessageContent="[NightShift] <bettype>'s Assignee has been updated from <old value> to <newvalue> in your assignment <League>",
                 IsImportant = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.LGAAddLiveMatch,
                 MessageContent="ADD LIVE for <K.O Time> <League> - <Home> -vs- <Away>",
                 IsImportant = true,
                 IsBlinking = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.LGACancelLiveMatch,
                 MessageContent="CANCEL LIVE for <K.O Time> <League> - <Home> -vs- <Away>",
                 IsImportant = true,
                 IsBlinking = true
            },
            new MessageTemplate
            {
                 MessageType=(int)MsgTemplateCode.LGAChangeMatchStatus,
                 MessageContent="Has been <newvalue> for match: <League> <Home> -vs- <Away>; Previous <old value>",
                 IsImportant = true,
                 IsBlinking = true
            },
        });

        #endregion MessageTemplate

        #region MessageGroup

        private class MessageGroup : IEqualityComparer<MessageGroup>
        {
            public MessageGroup(ChangedValue value)
            {
                BetTypeName = value.BetTypeName;
                MessageTypeId = value.MessageTypeId;
                NewValue = value.NewValue;
                OldValue = value.OldValue;
                League = value.League;
                InfoSiteMatchId = value.InfoSiteMatchId;
            }

            public int InfoSiteMatchId { get; set; }

            public string AssignmentNote { get; set; }

            public string BetTypeName { get; set; }

            public int MessageTypeId { get; set; }

            public string NewValue { get; set; }

            public string OldValue { get; set; }

            public string League { get; set; }

            public string EventName { get; set; }

            public bool IsBlinking { get; set; }

            public bool Equals(MessageGroup x, MessageGroup y)
            {
#pragma warning disable S1067 // Expressions should not be too complex - ignored by Jacob
                return x.AssignmentNote == y.AssignmentNote &&
                   x.BetTypeName == y.BetTypeName &&
                   x.MessageTypeId == y.MessageTypeId &&
                   x.NewValue == y.NewValue &&
                   x.OldValue == y.OldValue &&
                   x.League == y.League &&
                   x.EventName == y.EventName &&
                   x.InfoSiteMatchId == y.InfoSiteMatchId;
#pragma warning restore S1067 // Expressions should not be too complex
            }

            public int GetHashCode(MessageGroup obj)
            {
                var hash = 17;
                hash = hash * 31 + obj.MessageTypeId;
                hash = hash * 47 + obj.League.GetHashCode();
                hash = hash * 59 + obj.EventName.GetHashCode();

                return hash;
            }
        }

        #endregion MessageGroup

        #region MessageModelRaw

        private class MessageModelRaw
        {
            public string MessageContent { get; set; }

            public bool IsImportant { get; set; }

            public int MessageType { get; set; }

            public bool IsBlinking { get; set; }

            public List<int> Assignees { get; set; }
        }

        #endregion MessageModelRaw

        private static readonly Dictionary<int, MessageTemplate> messageTemplateMap = messageTemplates.ToDictionary(m => m.MessageType, m => m);

        private readonly INapSession currentSession;

        public MessageFactoryImp(INapSession currentSession)
        {
            this.currentSession = currentSession;
        }

        public List<MessageModel> Transform(IList<ChangedValue> changedValues)
        {
            var messageModels = TransformChangedValueToMessageModel(changedValues);
            return Reduce(messageModels);
        }

        private List<MessageModelRaw> TransformChangedValueToMessageModel(IList<ChangedValue> changedValues)
        {
            return changedValues
                .Select(g =>
                {
                    var messageTemplate = messageTemplateMap[g.MessageTypeId];
                    var content = CreateMessageContent(g);

                    return new MessageModelRaw
                    {
                        IsImportant = messageTemplate.IsImportant,
                        MessageContent = content,
                        MessageType = g.MessageTypeId,
                        Assignees = g.ReceivedUserIds,
                        IsBlinking = messageTemplate.IsBlinking
                    };
                }).ToList();
        }

        private string CreateMessageContent(ChangedValue value)
        {
            var messageTemplate = messageTemplateMap[value.MessageTypeId];
            var content = messageTemplate.MessageContent;

            content = content.Replace("<newvalue>", "<b><newvalue></b>");
            content = content.Replace("<bettype>", "<b><bettype></b>");

            content = content.Replace("<newvalue>", string.IsNullOrWhiteSpace(value.NewValue) ? "[]" : value.NewValue);
            content = content.Replace("<old value>", string.IsNullOrWhiteSpace(value.OldValue) ? "[]" : value.OldValue);
            content = content.Replace("<League>", value.League);
            content = content.Replace("<Home>", value.Home);
            content = content.Replace("<Away>", value.Away);
            content = content.Replace("<User>", currentSession.UserName);
            content = content.Replace("<bettype>", value.BetTypeName);
            content = content.Replace("<K.O Time>", value.KoTime);

            return content;
        }

        private static List<MessageModel> Reduce(List<MessageModelRaw> messageModels)
        {
            return messageModels.GroupBy(m => new { m.MessageContent, m.IsImportant, m.MessageType, m.IsBlinking })
                .Select(m =>
                {
                    var assignees = m.SelectMany(x => x.Assignees).Distinct().ToArray();
                    return new MessageModel
                    {
                        MessageType = m.Key.MessageType,
                        MessageContent = m.Key.MessageContent,
                        IsImportant = m.Key.IsImportant,
                        ReceivedUserIds = assignees.Select(a => a.ToString()).ToList(),
                        IsBlinking = m.Key.IsBlinking
                    };
                })
                .ToList();
        }
    }
}