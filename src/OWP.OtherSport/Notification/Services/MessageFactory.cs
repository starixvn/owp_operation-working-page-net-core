using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Fanex.NAP.Client.Helpers.Web;
using Fanex.NAP.Core.Web.Session;
using OWP.OtherSport.Notification.Constants;
using OWP.OtherSport.Notification.Models;

namespace OWP.OtherSport.Notification.Services
{
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
                MessageType = (int)MsgTemplateCode.EditMatchNo,
                MessageContent = "<b>Match No.</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
             new ()
            {
                MessageType = (int)MsgTemplateCode.EditMatchStatus,
                MessageContent = "<b>Match Status</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>",
                IsImportant= true
             },
             new()
             {
                MessageType = (int)MsgTemplateCode.EditCategory,
                MessageContent = "<b>Category</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>",
             },
             new()
             {
                 MessageType=(int)MsgTemplateCode.EditNoOfHDP,
                 MessageContent="<b>No. HDP</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
             },
             new()
             {
                 MessageType=(int)MsgTemplateCode.EditMatchOffer,
                 MessageContent="<b>Match Offer</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
             },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditAutoLink,
                 MessageContent="<b>Auto Link</b> Updated from <old value> to  <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditProvider,
                 MessageContent="<b>Provider</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditOtherProvider,
                 MessageContent="<b>Other Provider</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
             new()
            {
                 MessageType=(int)MsgTemplateCode.EditIPTV,
                 MessageContent="<b>IPTV</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditWB,
                 MessageContent="<b>Streaming</b> Info (WB) Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditRemark,
                 MessageContent="<b>Remark</b> Updated from <old value>> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditMaxbet,
                 MessageContent="<b>Max Bet</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.EditReporter,
                 MessageContent="<b>Reporter</b> Updated from <old value> to <newvalue> for <Match.No> - <League> - <Event> updated by <User>"
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.AssignmentInsert,
                 MessageContent="You have NEW ASSIGNMENT <bettype> in <Match.No> - <League> - <Event> updated by <User>",
                 IsImportant = true
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.OtherAssignmentInsert,
                 MessageContent="You have NEW ASSIGNMENT <bettype> in <Match.No> - <League> - <Event> updated by <User>",
                 IsImportant = true
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.AssignmentUpdate,
                 MessageContent="UPDATED ASSIGNEE of <bettype> from <old value> change to <newvalue> for <Match No.> - <League> - <Event> Updated by <User> ",
                 IsImportant = true
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.OtherAssignmentUpdate,
                 MessageContent="UPDATED ASSIGNEE of <bettype> from <old value> change to <newvalue> for <Match No.> - <League> - <Event> Updated by <User> ",
                 IsImportant = true
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.AssignmentDelete,
                 MessageContent="You have been REMOVED from <Match.No> - <League> - <Event> updated by <User>",
                 IsImportant = true
            },
            new()
            {
                 MessageType=(int)MsgTemplateCode.OtherAssignmentDelete,
                 MessageContent="You have been REMOVED from <Match.No> - <League> - <Event> updated by <User>",
                 IsImportant = true
            }
        });

        #endregion MessageTemplate

        #region MessageGroup

        private class MessageGroup : IEqualityComparer<MessageGroup>
        {
            public MessageGroup(ChangedValue value)
            {
                AssignmentNote = value.AssignmentNote;
                BetTypeName = value.BetTypeName;
                MessageTypeId = value.MessageTypeId;
                NewValue = value.NewValue;
                OldValue = value.OldValue;
                League = value.League;
                EventName = value.EventName;
                MatchNo = value.MatchNo;
            }

            public string AssignmentNote { get; set; }

            public string BetTypeName { get; set; }

            public int MessageTypeId { get; set; }

            public string NewValue { get; set; }

            public string OldValue { get; set; }

            public string League { get; set; }

            public string EventName { get; set; }

            public string MatchNo { get; set; }

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
                   x.MatchNo == y.MatchNo;
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
                        Assignees = g.Select(i => new Assignee { AssigneeId = i.UserId, AssigneeName = i.Username }).ToList()
                    };
                }).ToList();
        }

        private string CreateMessageContent(MessageGroup messageGroup)
        {
            var messageTemplate = messageTemplateMap[messageGroup.MessageTypeId];
            var content = messageTemplate.MessageContent;

            var assignmentNote = Regex.Replace(messageGroup.AssignmentNote, "#<user>", "", RegexOptions.IgnoreCase);
            assignmentNote = Regex.Replace(assignmentNote, "same", "", RegexOptions.IgnoreCase);

            content = content.Replace("<newvalue>", "<b><newvalue></b>");
            content = content.Replace("<bettype>", "<b><bettype></b>");

            content = content.Replace("<newvalue>", messageGroup.NewValue);
            content = content.Replace("<old value>", messageGroup.OldValue);
            content = content.Replace("<Match.No>", !string.IsNullOrEmpty(messageGroup.MatchNo) ? $"#{messageGroup.MatchNo}" : string.Empty);
            content = content.Replace("<League>", messageGroup.League);
            content = content.Replace("<Event>", messageGroup.EventName);
            content = content.Replace("<User>", currentSession.UserName);

            content = content.Replace("<bettype>", messageGroup.BetTypeName);
            content = content.Replace("<Note>", assignmentNote);

            return content;
        }

        private static List<MessageModel> Reduce(List<MessageModelRaw> messageModels)
        {
            return messageModels.GroupBy(m => new { m.MessageContent, m.IsImportant, m.MessageType })
                .Select(m =>
                {
                    var assignees = m.SelectMany(x => x.Assignees).Distinct().ToArray();
                    return new MessageModel
                    {
                        MessageType = m.Key.MessageType,
                        MessageContent = m.Key.MessageContent,
                        IsImportant = m.Key.IsImportant,
                        ReceivedUserIds = assignees.Select(a => a.AssigneeId.ToString()).ToList()
                    };
                })
                .ToList();
        }
    }
}