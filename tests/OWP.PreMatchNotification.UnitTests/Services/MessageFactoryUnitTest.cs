using Fanex.NAP.Client.Helpers.Web;
using NSubstitute;
using OWP.PreMatchNotification.Constants;
using OWP.PreMatchNotification.Models;
using OWP.PreMatchNotification.Services;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.PreMatchNotification.UnitTests.Services
{
    public class MessageFactoryUnitTest
    {
        private MessageFactory MockService()
        {
            var currentSession = Substitute.For<INapSession>();
            currentSession.UserName = "starix1";

            return new MessageFactoryImp(currentSession);
        }

        [Fact]
        public void Transform_WithAddNewLeague_ReturnMessageList()
        {
            var service = MockService();
            var changedValueList = GetListNewLeague();
            var expected = new List<MessageModel>
            {
                new MessageModel
                {
                   IsBlinking=true,
                   IsImportant = true,
                   MessageContent = "<b>New Add</b> VLeague",
                   MessageType = (int)MsgTemplateCode.NewLeague,
                   ReceivedUserIds = new List<string> { "2","3","5"}
                }
            };

            var actual = service.Transform(changedValueList);

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void Transform_WithAssimentChanged_ReturnMessageList()
        {
            var service = MockService();
            var changedValueList = GetListAssignmentChanged();
            var expected = new List<MessageModel>
            {
                new MessageModel
                {
                   IsImportant = true,
                   MessageContent = "[DayShift] You have NEW ASSIGNMENT <b>O/U</b> in VLeague",
                   MessageType = (int)MsgTemplateCode.DayShiftAssignmentInsert,
                   ReceivedUserIds = new List<string> { "12","1"}
                },
                new MessageModel
                {
                   IsImportant = true,
                   MessageContent = "[DayShift] You have been REMOVED from <b>O/U</b> in VLeague",
                   MessageType = (int)MsgTemplateCode.DayShiftAssignmentDelete,
                   ReceivedUserIds = new List<string> { "2"}
                },
                new MessageModel
                {
                   IsImportant = true,
                   MessageContent = "[DayShift] <b>O/U</b>'s Assignee has been updated from Sa to <b>Starix15</b> in your assignment VLeague",
                   MessageType = (int)MsgTemplateCode.DayShiftOtherAssignmentUpdate,
                   ReceivedUserIds = new List<string> { "13"}
                },
            };

            var actual = service.Transform(changedValueList);

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void Transform_WithLinkedMatchChanged_ReturnMessageList()
        {
            var service = MockService();
            var changedValueList = GetListLinkedMatchChanged();
            var expected = new List<MessageModel>
            {
                new MessageModel
                {
                   IsBlinking=true,
                   IsImportant = true,
                   MessageContent = "Has been <b>Live</b> for match: LeagueA HomeA -vs- AwayA; Previous Postponed",
                   MessageType = (int)MsgTemplateCode.LGAChangeMatchStatus,
                   ReceivedUserIds = new List<string> { "1","3","8"}
                },
                new MessageModel
                {
                   IsBlinking=true,
                   IsImportant = true,
                   MessageContent = "ADD LIVE for 05:12 AM LeagueC - HomeA -vs- AwayA",
                   MessageType = (int)MsgTemplateCode.LGAAddLiveMatch,
                   ReceivedUserIds = new List<string> { "1","2","3"}
                },
                new MessageModel
                {
                   IsBlinking=true,
                   IsImportant = true,
                   MessageContent = "CANCEL LIVE for 04:02 AM VLeague - HomeA -vs- AwayA",
                   MessageType = (int)MsgTemplateCode.LGACancelLiveMatch,
                   ReceivedUserIds = new List<string> { "1","2"}
                },
            };

            var actual = service.Transform(changedValueList);

            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void Transform_WithMatchInfoChanged_ReturnMessageList()
        {
            var service = MockService();
            var changedValueList = GetListMatchInfoChanged();
            var expected = new List<MessageModel>
            {
                new MessageModel
                {
                   IsImportant = true,
                   MessageContent = "Home Team Update from Premier League - MC -vs- MU to Premier League - <b>MC changed</b> -vs- MU",
                   MessageType = (int)MsgTemplateCode.UpdateHome,
                   ReceivedUserIds = new List<string> { "2","3","4","5","6","7","8"}
                },
                new MessageModel
                {
                   IsImportant = true,
                   MessageContent = "Away Team Update from Premier League - Liv -vs- MC to Premier League - Liv -vs- <b>MC changed</b>",
                   MessageType = (int)MsgTemplateCode.UpdateAway,
                   ReceivedUserIds = new List<string> { "2","3","4","5","6"}
                },
                new MessageModel
                {
                   IsImportant = true,
                   MessageContent = "K.O time Update from 06/23/2021 03:00 PM to <b>09/03/2022 03:00 PM</b> for Premier League - Liv -vs- MC",
                   MessageType = (int)MsgTemplateCode.UpdateKOTime,
                   ReceivedUserIds = new List<string> { "2","3"}
                },
            };

            var actual = service.Transform(changedValueList);

            DeepAssert.Equal(expected, actual);
        }

        private IList<ChangedValue> GetListNewLeague()
        {
            return new List<ChangedValue>
            {
                new ChangedValue
                {
                    League = "VLeague",
                    ReceivedUserIds = new List<int> { 2, 3 },
                    MessageTypeId= (int) MsgTemplateCode.NewLeague
                },
                new ChangedValue
                {
                    League = "VLeague",
                    ReceivedUserIds = new List<int> { 3, 5 },
                    MessageTypeId= (int) MsgTemplateCode.NewLeague
                },
            };
        }

        private IList<ChangedValue> GetListAssignmentChanged()
        {
            return new List<ChangedValue>
            {
                new ChangedValue
                {
                    BetTypeName = "O/U",
                    League = "VLeague",
                    MessageTypeId = (int)MsgTemplateCode.DayShiftAssignmentInsert,
                    ReceivedUserIds = new List<int>{12,1}
                },
                new ChangedValue
                {
                    BetTypeName = "O/U",
                    League = "VLeague",
                    MessageTypeId = (int)MsgTemplateCode.DayShiftAssignmentDelete,
                    ReceivedUserIds = new List<int>{2}
                },
                new ChangedValue
                {
                    League = "VLeague",
                    BetTypeName = "O/U",
                    OldValue = "Sa",
                    NewValue = "Starix15",
                    ReceivedUserIds = new List<int>{13},
                    MessageTypeId = (int)MsgTemplateCode.DayShiftOtherAssignmentUpdate
                },
            };
        }

        private IList<ChangedValue> GetListLinkedMatchChanged()
        {
            return new List<ChangedValue>
            {
                new ChangedValue
                {
                    MessageTypeId=(int)MsgTemplateCode.LGAChangeMatchStatus,
                    League="LeagueA",
                    Home="HomeA",
                    Away="AwayA",
                    OldValue = "Postponed",
                    NewValue = "Live",
                    ReceivedUserIds=new List<int>{1,3,8}
                },
                new ChangedValue
                {
                    MessageTypeId=(int)MsgTemplateCode.LGAAddLiveMatch,
                    League="LeagueC",
                    Home="HomeA",
                    Away="AwayA",
                    KoTime = "05:12 AM",
                    ReceivedUserIds=new List<int>{1,2,3}
                },
                new ChangedValue
                {
                    MessageTypeId=(int)MsgTemplateCode.LGACancelLiveMatch,
                    League="VLeague",
                    Home="HomeA",
                    Away="AwayA",
                    KoTime = "04:02 AM",
                    ReceivedUserIds=new List<int>{1,2}
                }
            };
        }

        private IList<ChangedValue> GetListMatchInfoChanged()
        {
            return new List<ChangedValue>
            {
                new ChangedValue
                {
                    InfoSiteMatchId = 1,
                    League="Premier League",
                    Home = "MC",
                    Away = "MU",
                    OldValue = "MC" ,
                    NewValue =  "MC changed",
                    ReceivedUserIds = new List<int> { 2, 2, 3, 4, 5, 6 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 7,
                    League="Premier League",
                    Home = "MC",
                    Away = "MU",
                    OldValue = "MC" ,
                    NewValue =  "MC changed",
                    ReceivedUserIds = new List<int> { 5, 6 ,7, 8 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateHome
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League = "Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = "MC",
                    NewValue = "MC changed",
                    ReceivedUserIds = new List<int> { 2, 3, 4, 5, 6 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateAway
                },
                new ChangedValue
                {
                    InfoSiteMatchId = 2,
                    League = "Premier League",
                    Home = "Liv",
                    Away = "MC",
                    OldValue = new DateTime(2021, 06, 23, 15, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    NewValue = new DateTime(2022, 09, 03, 15, 00, 00).ToString("MM/dd/yyyy hh:mm tt"),
                    ReceivedUserIds = new List<int> { 2, 3 },
                    MessageTypeId = (int)MsgTemplateCode.UpdateKOTime
                }
            };
        }
    }
}