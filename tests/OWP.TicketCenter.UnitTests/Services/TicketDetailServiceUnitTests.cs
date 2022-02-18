namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Models;
    
    
        using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TicketDetailServiceUnitTests
    {
        private readonly IMapper mapper;

        public TicketDetailServiceUnitTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketOtherModel, MessageDetailViewModel>();
                cfg.CreateMap<EventTicketLeagueModel, MessageDetailViewModel>();
                cfg.CreateMap<EventTicketMatchModel, MessageDetailViewModel>();
            });

            mapper = configuration.CreateMapper();
        }

        [Fact]
        public void BuildMessage_Always_ListMessageDetailViewModel()
        {
            var service = new TicketDetailServiceImp(mapper);
            var expected = GetExpected();

            var actual = service.BuildMessage(MockTicketSource()).ToList();

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketModelBase> MockTicketSource()
        {
            return new List<EventTicketModelBase>
            {
                new EventTicketOtherModel()
                {
                    TicketId = 20,
                    MessageId = 18,
                    TypeName="Other",
                    Subject = "vaccine",
                    Message ="hehe",
                    UserId = 12,
                    Username = "starix1",
                    CreatedDate = new DateTime(2021,9,29,17,29,31)
                },
                new EventTicketLeagueModel()
                {
                    TicketId = 10,
                    MessageId = 12,
                    TypeName="Other",
                    OwpLeagueName ="NHA",
                    Message ="hi",
                    UserId = 8,
                    Username = "sa",
                    CreatedDate = new DateTime(2021,9,29,16,29,31)
                },
                new EventTicketLeagueModel()
                {
                    TicketId = 10,
                    MessageId = 12,
                    TypeName="Other",
                    OwpLeagueName ="NHA",
                    Message ="hello,test Match",
                    UserId = 8,
                    Username = "sa",
                    CreatedDate = new DateTime(2021,9,29,15,30,31)
                },
                new EventTicketMatchModel()
                {
                    TicketId = 10,
                    MessageId = 14,
                    TypeName="Other",
                    HomeName = "MU",
                    AwayName = "MC",
                    Message ="hi, Im Liam test",
                    UserId = 9,
                    Username = "starix2",
                    CreatedDate = new DateTime(2021,9,29,16,29,31)
                },
            };
        }

        public List<MessageDetailViewModel> GetExpected()
            => new List<MessageDetailViewModel>
            {
                new()
                {
                    MessageId = 12,
                    UserId = 8,
                    Message = "hello,test Match",
                    CreatedDate =new DateTime(2021,9,29,15,30,31),
                    Username = "sa",
                },
                new()
                {
                    MessageId = 12,
                    UserId = 8,
                    Message = "hi",
                    CreatedDate = new DateTime(2021,9,29,16,29,31),
                    Username = "sa",
                },
                new()
                {
                    MessageId = 14,
                    UserId = 9,
                    Message = "hi, Im Liam test",
                    CreatedDate = new DateTime(2021,9,29,16,29,31),
                    Username = "starix2",
                },
                new()
                {
                    MessageId = 18,
                    UserId = 12,
                    Message = "hehe",
                    CreatedDate = new DateTime(2021,9,29,17,29,31),
                    Username = "starix1",
                }
            };
    }
}