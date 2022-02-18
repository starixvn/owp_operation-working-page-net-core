namespace OWP.TicketCenter.UnitTests.Services
{
    using TicketCenter.Handlers;
    using Enum;
    using AutoMapper;
    using Models;
    using NSubstitute;
    
    
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;
    public class EventTicketSupportServiceUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketSupportServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketSupportModel, TicketSupportDetailViewModel>();
                cfg.CreateMap<EventTicketSupportModel, MessageResponseModel>();
            });

            mapper = configuration.CreateMapper();
        }
        [Fact]
        public async Task GetAllTicketSupport_Always_MapTicketLeagueDetail()
        {
            var request = new QueryEventTicketsRequest();
            repository.GetEventTicketSupport(request).Returns(MockListEventTicketSupport());
            var service = new EventTicketSupportServiceImp(repository, mapper);
            var expected = GetExpected();

            var actual = await service.GetAllTicketSupport(request);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketSupportModel> MockListEventTicketSupport()
            => new List<EventTicketSupportModel>
            {
                new()
                {
                    TicketId = 10,
                    MessageId = 12,
                    TypeName="Support",
                    Subject="Covid-19",
                    Message ="hello,test Match",
                    UserId = 8
                },
                new()
                {
                    TicketId = 10,
                    MessageId = 14,
                    TypeName="Support",
                    Message ="hi, Im Liam test",
                    UserId = 9
                },
                new()
                {
                    TicketId = 20,
                    MessageId = 18,
                    TypeName="Support",
                    Subject = "vaccine",
                    Message ="hehe",
                    UserId = 12
                },
            };

        public List<TicketSupportDetailViewModel> GetExpected()
            => new List<TicketSupportDetailViewModel>
            {
                new()
                {
                    TicketId = 10,
                    Subject = "Covid-19",
                    TypeName="Support",
                    EventType = (int)EventType.Support,
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 12, UserId = 8,Message = "hello,test Match"},
                        new() { MessageId = 14, UserId = 9,Message = "hi, Im Liam test"}
                    }
                },
                new()
                {
                    TicketId = 20,
                    Subject = "vaccine",
                    TypeName="Support",
                    EventType = (int)EventType.Support,
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 18, UserId = 12,Message ="hehe"}
                    }
                }
            };
    }
}
