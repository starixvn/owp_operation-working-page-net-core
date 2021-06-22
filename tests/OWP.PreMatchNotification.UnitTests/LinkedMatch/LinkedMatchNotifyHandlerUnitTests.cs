namespace OWP.PreMatchNotification.UnitTests.LinkedMatch
{
    using NSubstitute;
    using OWP.PreMatchNotification.LinkedMatch;
    using OWP.PreMatchNotification.LinkedMatch.Models;
    using OWP.PreMatchNotification.LinkedMatch.Services;
    using OWP.PreMatchNotification.Models;
    using OWP.PreMatchNotification.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class LinkedMatchNotifyHandlerUnitTests
    {
        public LinkedMatchNotifyHandler service;
        private readonly CacheManipulator cacheManipulator;
        private readonly Repository queryRepository;
        private readonly ChangedLinkedMatchDetector changedValueBySnapshot;
        private readonly MessageFactory messageFactory;
        private readonly CommandRepository commandRepository;
        private readonly NotifyServices notify;

        public LinkedMatchNotifyHandlerUnitTests()
        {
            cacheManipulator= Substitute.For<CacheManipulator>();
            queryRepository = Substitute.For<Repository>();
            changedValueBySnapshot = Substitute.For<ChangedLinkedMatchDetector>();
            messageFactory = Substitute.For<MessageFactory>();
            commandRepository = Substitute.For<CommandRepository>();
            notify = Substitute.For<NotifyServices>();

            service = new LinkedMatchNotifyHandler(
                cacheManipulator, 
                queryRepository, 
                changedValueBySnapshot,
                messageFactory, 
                commandRepository, 
                notify);
        }

        [Fact]
        public void MapAssignmentWithInfositeId_MapAssigneeByInfosite()
        {
            var prematchLeague = new List<InfositePrematchLeague>
            {
                new(){InfoSiteMatchId=111, PreMatchLeagueId=100},
                new(){InfoSiteMatchId=222, PreMatchLeagueId=200},
                new(){InfoSiteMatchId=333, PreMatchLeagueId=300},
            };
            var prematchAssignees = new List<Assignee>
            {
                new(){PreMatchLeagueId=100,AssigneeId=1},
                new(){PreMatchLeagueId=200,AssigneeId=3},
                new(){PreMatchLeagueId=200,AssigneeId=4},
            };
            var expected= new List<InfositePrematchLeague>
            {
                new(){InfoSiteMatchId=111, PreMatchLeagueId=100, PrematchAssigneeIds=new List<int>{1} },
                new(){InfoSiteMatchId=222, PreMatchLeagueId=200, PrematchAssigneeIds=new List<int>{3,4} },
                new(){InfoSiteMatchId=333, PreMatchLeagueId=300, PrematchAssigneeIds=new List<int>{} },
            };

            service.MapAssignmentWithInfositeId(prematchLeague, prematchAssignees);

            DeepAssert.Equal(expected, prematchLeague);
        }

        [Fact]
        public async Task GenerateMessages_OldMatchesIsNull_ReturnEmpty()
        {
            var request = new LinkedMatchNotifyRequest { EventDate = new DateTime(2021, 6, 18), SessionId = "abc" };
            cacheManipulator.GetOldLGAMatchSnapshot(request.SessionId, request.EventDate).Returns(new List<GeneralMatch>());

            var actual = await service.GenerateMessages(request);
            Assert.Empty(actual);
        }

        [Fact]
        public async Task GenerateMessages_OldMatchesHasValue_ReturnListChangeValue()
        {
            var request = new LinkedMatchNotifyRequest { EventDate = new DateTime(2021, 6, 18), SessionId = "abc" };
            cacheManipulator.GetOldLGAMatchSnapshot(request.SessionId, request.EventDate)
                .Returns(new List<GeneralMatch> { new() { MatchId = 123 } });
            queryRepository.GetLGAMatchNewSnapshot(request.EventDate).Returns(new List<GeneralMatch> { new() { MatchId = 456 } });
            queryRepository.GetInfositePrematch(request.EventDate).Returns(new List<InfositePrematchLeague>());
            commandRepository.GetEmployeesBySection("").Returns(new List<Assignee> { new() { AssigneeId = 1 }, new() { AssigneeId = 2 } });
            commandRepository.GetPreMatchLeagueAssignees(new List<int>()).Returns(new List<Assignee>());

            var expected = new List<ChangedValue> { new ChangedValue { MessageTypeId = 200 } };
            changedValueBySnapshot.Create(Arg.Compat.Any<List<GeneralMatch>>(), Arg.Compat.Any<List<GeneralMatch>>(), Arg.Compat.Any<Employee>()).Returns(expected);

            var actual = await service.GenerateMessages(request);
            DeepAssert.Equal(expected, actual);
        }
    }
}
