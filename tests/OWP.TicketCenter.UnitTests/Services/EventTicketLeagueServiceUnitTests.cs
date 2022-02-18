using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.UnitTests.Enum;

namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Handlers;
    using Models;
    using NSubstitute;
    
    
    
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class EventTicketLeagueServiceUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketLeagueServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketLeagueModel, TicketLeagueDetailViewModel>();
                cfg.CreateMap<EventTicketLeagueModel, MessageResponseModel>();
            });

            mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task GetAllLeague_Always_MapTicketLeagueDetail()
        {
            var request = new QueryEventTicketsRequest();
            repository.GetEventTicketLeague(request).Returns(MockListEventTicket());
            var service = new EventTicketLeagueServiceImp(repository, mapper);
            var expected = GetExpected();

            var actual = await service.GetAllLeague(request);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketLeagueModel> MockListEventTicket()
            => new List<EventTicketLeagueModel>
            {
                new()
                {
                    TicketId = 1,
                    MessageId = 12,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    Message ="hello",
                    UserId = 8,
                    CreatedDate = new DateTime(2021,10,8),
                },
                new()
                {
                    TicketId = 1,
                    MessageId = 14,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    Message ="hi, Im Liam",
                    UserId = 9,
                    CreatedDate = new DateTime(2021,10,9)
                },
                new()
                {
                    TicketId = 2,
                    MessageId = 18,
                    OwpLeagueId=19,
                    OwpLeagueName="V-league",
                    Message ="hehe",
                    UserId = 12,
                    CreatedDate = new DateTime(2021,10,10)
                },
            };

        public List<TicketLeagueDetailViewModel> GetExpected()
            => new List<TicketLeagueDetailViewModel>
            {
                new()
                {
                    TicketId = 1,
                    OwpLeagueId=17,
                    OwpLeagueName="NHA",
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 14, UserId = 9,Message = "hi, Im Liam",CreatedDate = new DateTime(2021,10,9)},
                        new() { MessageId = 12, UserId = 8,Message = "hello",  CreatedDate = new DateTime(2021,10,8)},
                    },
                    EventType = (int)EventType.League
                },
                new()
                {
                    TicketId = 2,
                    OwpLeagueId=19,
                    OwpLeagueName="V-league",
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 18, UserId = 12,Message ="hehe",CreatedDate = new DateTime(2021,10,10) }
                    },
                    EventType = (int)EventType.League
                }
            };
    }
}