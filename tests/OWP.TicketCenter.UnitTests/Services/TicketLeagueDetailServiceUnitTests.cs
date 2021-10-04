namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Models;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TicketLeagueDetailServiceUnitTests
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketLeagueDetailServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketLeagueModel, TicketLeagueDetailViewModel>();
                cfg.CreateMap<EventTicketLeagueModel, MessageDetailViewModel>();
            });

            mapper = configuration.CreateMapper();
            ticketDetailService = new TicketDetailServiceImp(mapper);
        }

        [Fact]
        public async Task GetTicketLeagueDetail_Always_ReturnTicketLeagueDetailViewModel()
        {
            var ticketId = 144;
            repository.GetTicketLeagueDetail(ticketId).Returns(MockTicketLeagueSource());
            repository.GetRecipientByTicketId(ticketId).Returns(MockRecipientResponse());
            var service = new TicketLeagueDetailServiceImp(repository, ticketDetailService, mapper);
            var expected = new TicketLeagueDetailViewModel()
            {
                TicketId = 144,
                OwpLeagueId = 17,
                OwpLeagueName = "NHA",
                TypeName = "Other",
                Recipients = new List<RecipientResponseModel>
                {
                    new()
                    {
                        RecipientId = 10,
                        UserName = "sa",
                    },
                    new()
                    {
                        RecipientId = 11,
                        UserName = "Luca",
                    },
                },
                Messages = new List<MessageDetailViewModel>
                {
                    new() {MessageId = 12, Username = "sa", UserId = 8, Message = "hello",CreatedDate = new DateTime(2021,8,1)},
                    new() {MessageId = 14,Username = "Luca",UserId = 9, Message = "hi,Liam",CreatedDate = new DateTime(2021,8,2)},
                }
            };

            var actual = await service.GetTicketLeagueDetail(ticketId);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketLeagueModel> MockTicketLeagueSource()
            => new List<EventTicketLeagueModel>
            {
                new()
                {
                    TicketId = 144,
                    MessageId = 12,
                    TypeName="Other",
                    OwpLeagueId = 17,
                    OwpLeagueName ="NHA",
                    Message ="hello",
                    UserId = 8,
                    Username = "sa",
                    CreatedDate = new DateTime(2021,8,1)
                },
                new()
                {
                    TicketId = 144,
                    MessageId = 14,
                    TypeName="Other",
                    OwpLeagueId = 17,
                    OwpLeagueName ="NHA",
                    Message ="hi,Liam",
                    UserId = 9,
                    Username = "Luca",
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
                new()
                {
                    RecipientId = 11,
                    UserName = "Luca",
                },
            };
    }
}