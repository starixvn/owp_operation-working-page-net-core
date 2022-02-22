namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Enum;
    using Models;
    using NSubstitute;
    using TicketCenter.Handlers;
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TicketSupportDetailServiceUnitTests
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketSupportDetailServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketSupportModel, TicketSupportDetailViewModel>();
                cfg.CreateMap<EventTicketSupportModel, MessageDetailViewModel>();
            });

            mapper = configuration.CreateMapper();
            ticketDetailService = new TicketDetailServiceImp(mapper);
        }

        [Fact]
        public async Task GetTicketSupportDetail_Always_ReturnTicketMatchDetailViewModel()
        {
            // Arrange
            var request = new TicketSupportDetailRequest() { ActorId = 1, TicketId = 10 };

            // Act
            repository.GetTicketSupportDetail(request.TicketId).Returns(MockTicketSupportSource());
            repository.GetRecipientByTicketId(request.TicketId).Returns(MockRecipientResponse());

            var service = new TicketSupportDetailServiceImp(repository, ticketDetailService, mapper);
            var expected = new TicketSupportDetailViewModel()
            {
                TicketId = 146,
                Subject = "champions league",
                TypeName = "Support",
                EventType = (int)EventType.Support,
                Recipients = new List<RecipientResponseModel>
                {
                    new()
                    {
                        RecipientId = 11,
                        UserName = "Liam"
                    },
                    new()
                    {
                        RecipientId = 12,
                        UserName = "Luca",
                    },
                },
                Messages = new List<MessageDetailViewModel>
                {
                    new() {MessageId = 7, Username = "Johnny", UserId = 8, Message = "hello",CreatedDate = new DateTime(2021,8,1)},
                    new() {MessageId = 8,Username = "Carlos",UserId = 9, Message = "ten ten",CreatedDate = new DateTime(2021,8,2)},
                }
            };

            var actual = await service.GetTicketSupportDetail(request);

            // Assert
            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketSupportModel> MockTicketSupportSource()
            => new()
            {
                new()
                {
                    TicketId = 146,
                    MessageId = 7,
                    TypeName = "Support",
                    Subject = "champions league",
                    Message = "hello",
                    UserId = 8,
                    Username = "Johnny",
                    CreatedDate = new DateTime(2021, 8, 1)
                },
                new()
                {
                    TicketId = 146,
                    MessageId = 8,
                    TypeName = "Support",
                    Subject = "champions league",
                    Message = "ten ten",
                    UserId = 9,
                    Username = "Carlos",
                    CreatedDate = new DateTime(2021, 8, 2)
                },
            };

        public IEnumerable<RecipientResponseModel> MockRecipientResponse()
            => new List<RecipientResponseModel>
            {
                new()
                {
                    RecipientId = 11,
                    UserName = "Liam",
                },
                new()
                {
                    RecipientId = 12,
                    UserName = "Luca",
                },
            };
    }
}