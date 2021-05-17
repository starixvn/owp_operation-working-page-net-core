namespace OWP.OtherSport.UnitTests.Notification.Services
{
    using System.Collections.Generic;
    using OtherSport.Notification.Models;
    using OtherSport.Notification.Services;
    using Fanex.NAP.Core.Web.Session;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;
    using OWP.OtherSport.Notification.Constants;
    using Fanex.NAP.Client.Helpers.Web;

    public class MessageFactoryUnitTest
    {
        private readonly INapSession currentSession;
        private readonly MessageFactory messageFactory;

        public MessageFactoryUnitTest()
        {
            currentSession = Substitute.For<INapSession>();
            messageFactory = new MessageFactoryImp(currentSession);
        }

        [Fact]

        public void Transform_Always_ReturnMessageList()
        {
            var changedValueList = GetListChangedValue();
            currentSession.UserName = "sa";

            var expected = new List<Message>
            {
                new Message
                {
                   IsImportant = false,
                   MessageContent = "<b>Match No.</b> Updated from 1234 to <b>20000</b> for #20000 - A-Coners - LiverPool-Coners -vs- Arsenal-Coners updated by sa",
                   MessageType = (int)MsgTemplateCode.EditMatchNo,
                   ReceivedUserIdList = new List<string> { "6789" }
                },

                new Message
                {
                   IsImportant = false,
                   MessageContent = "<b>Max Bet</b> Updated from 400K to <b>300K</b> for #50000 - PremierLeague - LiverPool -vs- Arsenal updated by sa",
                   MessageType = (int)MsgTemplateCode.EditMaxbet,
                   ReceivedUserIdList = new List<string> { "6789" }
                }
            };

            var actual = messageFactory.Transform(changedValueList);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void Transform_OtherAssignmentChanged_ReturnMessageList()
        {
            var changedValueList = GetListAssignmentChanged();
            currentSession.UserName = "sa";

            var expected = new List<Message>
            {
                new Message
                {
                   IsImportant = true,
                   MessageContent = "You have been REMOVED from #1324 - Jo - Spur -vs- MC updated by sa",
                   MessageType = (int)MsgTemplateCode.OtherAssignmentDelete,
                   ReceivedUserIdList = new List<string> { "2542" }
                },

                new Message
                {
                   IsImportant = true,
                   MessageContent = "You have NEW ASSIGNEMNT <b>1H (HDP)</b> in #5000 - Jo - Spur -vs- MC updated by sa",
                   MessageType = (int)MsgTemplateCode.OtherAssignmentInsert,
                   ReceivedUserIdList = new List<string> { "2542" }
                }
            };

            var actual = messageFactory.Transform(changedValueList);
            DeepAssert.Equal(expected, actual);
        }

        private List<ChangedValue> GetListAssignmentChanged()
        {
            var valueChanged = new ChangedValue
            {
                MessageTypeId = (int)MsgTemplateCode.OtherAssignmentDelete,
                BetTypeName = "1H (HDP)",
                OldValue = "PK Same",
                NewValue = null,
                AssignmentNote = "PK Same",
                AssignmentType = 2,
                OSMatchId = 5567,
                MatchNo = "1324",
                League = "Jo",
                EventName = "Spur -vs- MC",
                UserId = 2542,
                Username = "esther"
            };

            var valueChanged2 = new ChangedValue
            {
                MessageTypeId = (int)MsgTemplateCode.OtherAssignmentInsert,
                OldValue = null,
                NewValue = "#esther",
                AssignmentNote = "#<user>",
                AssignmentType = 2,
                BetTypeName = "1H (HDP)",
                OSMatchId = 5567,
                MatchNo = "5000",
                League = "Jo",
                EventName = "Spur -vs- MC",
                UserId = 2542,
                Username = "esther"
            };

            var changedValueList = new List<ChangedValue>
            {
                valueChanged,
                valueChanged2
            };

            return changedValueList;
        }
        private List<ChangedValue> GetListChangedValue()
        {
            var valueChanged = new ChangedValue
            {
                MessageTypeId = (int)MsgTemplateCode.EditMatchNo,
                NewValue = "20000",
                OldValue = "1234",
                League = "A-Coners",
                MatchNo = "20000",
                EventName = "LiverPool-Coners -vs- Arsenal-Coners",
                UserId = 6789,
                Username = "esther"
            };

            var valueChanged2 = new ChangedValue
            {
                OldValue = "400K",
                NewValue = "300K",
                BetTypeName = "Next of Coners (HDP)",
                MatchNo = "50000",
                League = "PremierLeague",
                EventName = "LiverPool -vs- Arsenal",
                UserId = 6789,
                Username = "esther",
                MessageTypeId = (int)MsgTemplateCode.EditMaxbet
            };

            var changedValueList = new List<ChangedValue>
            {
                valueChanged,
                valueChanged2
            };

            return changedValueList;
        }
    }
}