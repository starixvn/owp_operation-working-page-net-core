namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Handlers;
    using Models;
    using NSubstitute;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class EventTicketMatchServiceUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketMatchServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketMatchModel, TicketMatchDetailViewModel>();
                cfg.CreateMap<EventTicketMatchModel, MessageModel>();
            });

            mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task GetAllMatch_Always_MapTicketLeagueDetail()
        {
            var request = new QueryEventTicketsRequest();
            repository.GetEventTicketMatch(request).Returns(MockListEventTicketMatch());
            var service = new EventTicketMatchServiceImp(repository, mapper);
            var expected = GetExpected();

            var actual = await service.GetAllMatch(request);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketMatchModel> MockListEventTicketMatch()
            => new List<EventTicketMatchModel>
            {
                new()
                {
                    TicketId = 10,
                    MessageId = 12,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    HomeName = "MU",
                    AwayName = "MC",
                    Message ="hello,test Match",
                    UserId = 8
                },
                new()
                {
                    TicketId = 10,
                    MessageId = 14,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    HomeName = "MU",
                    AwayName = "MC",
                    Message ="hi, Im Liam test",
                    UserId = 9
                },
                new()
                {
                    TicketId = 20,
                    MessageId = 18,
                    OwpLeagueId=19,
                    HomeName = "HAGL",
                    AwayName = "SHB Da Nang",
                    OwpLeagueName="V-league",
                    Message ="hehe",
                    UserId = 12
                },
            };

        public List<TicketMatchDetailViewModel> GetExpected()
            => new List<TicketMatchDetailViewModel>
            {
                new()
                {
                    TicketId = 10,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    HomeName = "MU",
                    AwayName = "MC",
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 12, UserId = 8,Message = "hello,test Match"},
                        new() { MessageId = 14, UserId = 9,Message = "hi, Im Liam test"}
                    }
                },
                new()
                {
                    TicketId = 20,
                    OwpLeagueId=19,
                    OwpLeagueName="V-league",
                    HomeName = "HAGL",
                    AwayName = "SHB Da Nang",
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 18, UserId = 12,Message ="hehe"}
                    }
                }
            };
    }
}