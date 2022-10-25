using AutoMapper;
using NSubstitute;
using OWP.TicketCenter.Enum;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.Services;
using OWP.TicketCenter.ViewModels;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.TicketCenter.UnitTests.Services
{
    public class TicketLiveIncidentDetailServiceUnitTests
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketLiveIncidentDetailServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketMatchModel, TicketMatchDetailViewModel>();
                cfg.CreateMap<EventTicketMatchModel, MessageDetailViewModel>();
            });

            mapper = configuration.CreateMapper();
            ticketDetailService = new TicketDetailServiceImp(mapper);
        }

        [Fact]
        public async Task GetTicketMatchDetail_Always_ReturnTicketMatchDetailViewModel()
        {
            // Arrange
            var request = new TicketLiveIncidentDetailRequest { ActorId = 1, TicketId = 10 };

            // Act
            repository.GetTicketLiveIncidentDetail(request.TicketId).Returns(MockTicketMatchSource());
            repository.GetRecipientByTicketId(request.TicketId).Returns(MockRecipientResponse());

            var service = new TicketLiveIncidentDetailServiceImp(repository, ticketDetailService, mapper);
            var expected = new TicketMatchDetailViewModel()
            {
                TicketId = 105,
                HomeName = "MU",
                AwayName = "Liv",
                TypeName = "Other",
                EventType = (int)EventType.LiveIncident,
                Recipients = new List<RecipientResponseModel>
                {
                    new()
                    {
                        RecipientId = 10,
                        UserName = "sa",
                    },
                },
                Messages = new List<MessageDetailViewModel>
                {
                    new() {MessageId = 12, Username = "Liam", UserId = 8, Message = "glory glory mu mu",CreatedDate = new DateTime(2022,8,1)},
                    new() {MessageId = 14,Username = "Bruno",UserId = 9, Message = "ten ten",CreatedDate = new DateTime(2022,8,2)},
                }
            };

            var actual = await service.GetTicketMatchDetail(request);

            // Assert
            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketMatchModel> MockTicketMatchSource()
            => new()
            {
                new()
                {
                    TicketId = 105,
                    MessageId = 12,
                    TypeName = "Other",
                    HomeName = "MU",
                    AwayName = "Liv",
                    Message = "glory glory mu mu",
                    UserId = 8,
                    Username = "Liam",
                    CreatedDate = new DateTime(2022, 8, 1)
                },
                new()
                {
                    TicketId = 105,
                    MessageId = 14,
                    TypeName = "Other",
                    HomeName = "MU",
                    AwayName = "Liv",
                    Message = "ten ten",
                    UserId = 9,
                    Username = "Bruno",
                    CreatedDate = new DateTime(2022, 8, 2)
                },
            };

        public IEnumerable<RecipientResponseModel> MockRecipientResponse()
            => new List<RecipientResponseModel>
            {
                new()
                {
                    RecipientId = 10,
                    UserName = "sa",
                },
            };
    }
}
