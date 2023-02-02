using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.UnitTests.Enum;

namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Models;
    using NSubstitute;
    using OWP.TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class GetEventTicketsServiceUnitTests
    {
        private readonly Repository repository;

        public GetEventTicketsServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
        }

        [Fact]
        public async Task GetAllLeague_Always_MapTicketLeagueDetail()
        {
            var request = new QueryEventTicketsRequest();
            repository.GetEventTicketLeague(request).Returns(MockListEventTicketLeague());
            repository.GetEventTicketMatch(request).Returns(MockListEventTicketMatch());
            repository.GetEventTicketOther(request).Returns(MockListEventTicketOther());
            repository.GetTicketIdHaveSavedMessage().Returns(MockListTicketIsSaved());
            repository.GetEventTicketSupport(request).Returns(MockListEventTicketSupport());
            repository.GetTwoLastComment(Arg.Compat.Any<IList<int>>()).Returns(MockLastMessageInfo());
            var service = new GetEventTicketsServiceImp(repository);
            var expected = GetExpected();

            var actual = await service.GetAllTickets(request);

            DeepAssert.Equal(expected, actual);
        }
        public List<int> MockListTicketIsSaved()
        => new List<int>{
          1,2,3
        };
        public List<TicketLeagueInfo> MockListEventTicketLeague()
                => new List<TicketLeagueInfo>
                {
                new()
                {
                    TicketId = 1,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    LastCommentDate=new DateTime(2022,3,16)
                },
                };

        public List<TicketMatchInfo> MockListEventTicketMatch()
           => new List<TicketMatchInfo>
           {
                new()
                {
                    TicketId = 2,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    LastCommentDate=new DateTime(2022,3,17)
                },
           };

        public List<TicketOtherInfo> MockListEventTicketOther()
           => new List<TicketOtherInfo>
           {
                new()
                {
                    TicketId = 3,
                    Subject="hi",
                    LastCommentDate=new DateTime(2022,3,18)
                },
           };

        public List<TicketSupportInfo> MockListEventTicketSupport()
           => new List<TicketSupportInfo>
           {
                new()
                {
                    TicketId = 4,
                    Subject ="hi",
                    LastCommentDate=new DateTime(2022,3,15)
                },
           };

        public List<MessageInfoModel> MockLastMessageInfo()
           => new List<MessageInfoModel>
           {
                  new()
                  {
                      MessageId = 1,
                      TicketId=1,
                  },
                  new()
                  {
                      MessageId = 2,
                      TicketId=2,
                  },
                  new()
                  {
                      MessageId = 3,
                      TicketId=3,
                  },
                  new()
                  {
                      MessageId = 4,
                      TicketId=4,
                  }
           };

        public IList<TicketBaseInfo> GetExpected()
            => new List<TicketBaseInfo>
            {
                new TicketOtherInfo()
                {
                    TicketId = 3,
                    Subject="hi",
                    LastCommentDate=new DateTime(2022,3,18),
                    Messages=new List<MessageInfoModel>()
                     {
                         new()
                          {
                              MessageId = 3,
                              TicketId=3,
                          }
                     }
                },
                new TicketMatchInfo()
                {
                    TicketId = 2,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    LastCommentDate=new DateTime(2022,3,17),
                    Messages=new List<MessageInfoModel>()
                     {
                         new()
                          {
                              MessageId = 2,
                              TicketId=2,
                          }
                     }
                },
                new TicketLeagueInfo()
                {
                     TicketId = 1,
                     OwpLeagueId=17,
                     OwpLeagueName="NHA",
                     LastCommentDate=new DateTime(2022,3,16),
                     Messages=new List<MessageInfoModel>()
                     {
                         new()
                          {
                              MessageId = 1,
                              TicketId=1,
                          }
                     }
                }
            };
    }
}