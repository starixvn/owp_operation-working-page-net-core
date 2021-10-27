using OWP.TicketCenter.Handlers;

namespace OWP.TicketCenter.UnitTests.Services
{
    using AutoMapper;
    using Models;
    using NSubstitute;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Handlers;
    using TicketCenter.Services;
    using ViewModels;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TicketOtherDetailServiceUnitTests
    {
        private readonly Repository repository;

        private readonly TicketDetailService ticketDetailService;

        private readonly IMapper mapper;

        public TicketOtherDetailServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketOtherModel, TicketOtherDetailViewModel>();
                cfg.CreateMap<EventTicketOtherModel, MessageDetailViewModel>();
            });

            mapper = configuration.CreateMapper();
            ticketDetailService = new TicketDetailServiceImp(mapper);
        }

        [Fact]
        public async Task GetTicketOtherDetail_Always_ReturnTicketMatchDetailViewModel()
        {
            var request = new TicketOtherDetailRequest() { ActorId = 1, TicketId = 10 };
            repository.GetTicketOtherDetail(request.TicketId).Returns(MockTicketOtherSource());
            repository.GetRecipientByTicketId(request.TicketId).Returns(MockRecipientResponse());
            var service = new TicketOtherDetailServiceImp(repository, ticketDetailService, mapper);
            var expected = new TicketOtherDetailViewModel()
            {
                TicketId = 146,
                Subject = "champions league",
                TypeName = "Other",
                Recipients = new List<RecipientResponseModel>
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
                },
                Messages = new List<MessageDetailViewModel>
                {
                    new() {MessageId = 7, Username = "Johnny", UserId = 8, Message = "hello",CreatedDate = new DateTime(2021,8,1)},
                    new() {MessageId = 8,Username = "Carlor",UserId = 9, Message = "ten ten",CreatedDate = new DateTime(2021,8,2)},
                }
            };

            var actual = await service.GetTicketOtherDetail(request);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketOtherModel> MockTicketOtherSource()
            => new ()
            {
                new()
                {
                    TicketId = 146,
                    MessageId = 7,
                    TypeName="Other",
                    Subject = "champions league",
                    Message ="hello",
                    UserId = 8,
                    Username = "Johnny",
                    CreatedDate = new DateTime(2021,8,1)
                },
                new()
                {
                    TicketId = 146,
                    MessageId = 8,
                    TypeName="Other",
                    Subject = "champions league",
                    Message ="ten ten",
                    UserId = 9,
                    Username = "Carlor",
                    CreatedDate = new DateTime(2021,8,2)
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