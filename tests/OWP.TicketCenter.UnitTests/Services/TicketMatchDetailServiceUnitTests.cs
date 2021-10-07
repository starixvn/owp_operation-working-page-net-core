namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Handlers;
    using Models;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TicketMatchDetailServiceUnitTests
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketMatchDetailServiceUnitTests()
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
            var request = new TicketMatchDetailRequest() { ActorId = 1, TicketId = 10 };
            repository.GetTicketMatchDetail(request.TicketId).Returns(MockTicketMatchSource());
            repository.GetRecipientByTicketId(request.TicketId).Returns(MockRecipientResponse());
            var service = new TicketMatchDetailServiceImp(repository, ticketDetailService, mapper);
            var expected = new TicketMatchDetailViewModel()
            {
                TicketId = 145,
                HomeName = "MU",
                AwayName = "Liv",
                TypeName = "Other",
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
                    new() {MessageId = 12, Username = "Liam", UserId = 8, Message = "hello",CreatedDate = new DateTime(2021,8,1)},
                    new() {MessageId = 14,Username = "Bruno",UserId = 9, Message = "ten ten",CreatedDate = new DateTime(2021,8,2)},
                }
            };

            var actual = await service.GetTicketMatchDetail(request);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketMatchModel> MockTicketMatchSource()
            => new ()
            {
                new()
                {
                    TicketId = 145,
                    MessageId = 12,
                    TypeName="Other",
                    HomeName = "MU",
                    AwayName = "Liv",
                    Message ="hello",
                    UserId = 8,
                    Username = "Liam",
                    CreatedDate = new DateTime(2021,8,1)
                },
                new()
                {
                    TicketId = 145,
                    MessageId = 14,
                    TypeName="Other",
                    HomeName = "MU",
                    AwayName = "Liv",
                    Message ="ten ten",
                    UserId = 9,
                    Username = "Bruno",
                    CreatedDate = new DateTime(2021,8,2)
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