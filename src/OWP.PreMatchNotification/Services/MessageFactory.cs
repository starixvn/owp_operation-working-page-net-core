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
        List<MessageModel> Transform(List<ChangedValue> changedValues);
    }

    public class MessageFactoryImp : MessageFactory
    {
        #region MessageTemplate

        private static readonly ImmutableList<MessageTemplate> messageTemplates = ImmutableList<MessageTemplate>.Empty.AddRange(new[]
        {
            new MessageTemplate
            {
                MessageType = (int)MsgTemplateCode.NewLeague,
                MessageContent = "<b>New Add</b> <League>"
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
            }
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
                EventName = value.EventName;
                InfoSiteMatchId = value.InfoSiteMatchId;
                IsBlinking = value.IsBlinking;
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

            public List<Assignee> Assignees { get; set; }
        }

        #endregion MessageModelRaw

        private static readonly Dictionary<int, MessageTemplate> messageTemplateMap = messageTemplates.ToDictionary(m => m.MessageType, m => m);

        private readonly INapSession currentSession;

        public MessageFactoryImp(INapSession currentSession)
        {
            this.currentSession = currentSession;
        }

        public List<MessageModel> Transform(List<ChangedValue> changedValues)
        {
            var messageModels = TransformChangedValueToMessageModel(changedValues);
            return Reduce(messageModels);
        }

        private List<MessageModelRaw> TransformChangedValueToMessageModel(List<ChangedValue> changedValues)
        {
            return changedValues.GroupBy(v => new MessageGroup(v))
                .Select(g =>
                {
                    var messageTemplate = messageTemplateMap[g.Key.MessageTypeId];
                    var content = CreateMessageContent(g.Key);

                    return new MessageModelRaw
                    {
                        IsImportant = messageTemplate.IsImportant,
                        MessageContent = content,
                        MessageType = g.Key.MessageTypeId,
                        Assignees = g.Select(i => new Assignee { AssigneeId = i.UserId, AssigneeName = i.Username }).ToList(),
                        IsBlinking = g.Key.IsBlinking
                    };
                }).ToList();
        }

        private string CreateMessageContent(MessageGroup messageGroup)
        {
            var messageTemplate = messageTemplateMap[messageGroup.MessageTypeId];
            var content = messageTemplate.MessageContent;

            content = content.Replace("<newvalue>", "<b><newvalue></b>");
            content = content.Replace("<bettype>", "<b><bettype></b>");

            content = content.Replace("<newvalue>", string.IsNullOrWhiteSpace(messageGroup.NewValue)?"[]": messageGroup.NewValue);
            content = content.Replace("<old value>", string.IsNullOrWhiteSpace(messageGroup.OldValue) ? "[]" : messageGroup.OldValue);
            content = content.Replace("<League>", messageGroup.League);
            content = content.Replace("<Event>", messageGroup.EventName);
            content = content.Replace("<User>", currentSession.UserName);
            content = content.Replace("<bettype>", messageGroup.BetTypeName);

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
                        ReceivedUserIds = assignees.Select(a => a.AssigneeId.ToString()).ToList(),
                        IsBlinking = m.Key.IsBlinking
                    };
                })
                .ToList();
        }
    }
}