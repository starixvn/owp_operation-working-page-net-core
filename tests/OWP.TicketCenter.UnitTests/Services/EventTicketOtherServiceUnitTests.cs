using OWP.TicketCenter.Handlers;

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

    public class EventTicketOtherServiceUnitTests
    {
        private readonly Repository repository;

        private readonly IMapper mapper;

        public EventTicketOtherServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventTicketOtherModel, TicketOtherDetailViewModel>();
                cfg.CreateMap<EventTicketOtherModel, MessageResponseModel>();
            });

            mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task GetAllTicketOther_Always_MapTicketLeagueDetail()
        {
            var request = new QueryEventTicketsRequest();
            repository.GetEventTicketOther(request).Returns(MockListEventTicketOther());
            var service = new EventTicketOtherServiceImp(repository, mapper);
            var expected = GetExpected();

            var actual = await service.GetAllTicketOther(request);

            DeepAssert.Equal(expected, actual);
        }

        public List<EventTicketOtherModel> MockListEventTicketOther()
            => new List<EventTicketOtherModel>
            {
                new()
                {
                    TicketId = 10,
                    MessageId = 12,
                    TypeName="Other",
                    Subject="Covid-19",
                    Message ="hello,test Match",
                    UserId = 8
                },
                new()
                {
                    TicketId = 10,
                    MessageId = 14,
                    TypeName="Other",
                    Message ="hi, Im Liam test",
                    UserId = 9
                },
                new()
                {
                    TicketId = 20,
                    MessageId = 18,
                    TypeName="Other",
                    Subject = "vaccine",
                    Message ="hehe",
                    UserId = 12
                },
            };

        public List<TicketOtherDetailViewModel> GetExpected()
            => new List<TicketOtherDetailViewModel>
            {
                new()
                {
                    TicketId = 10,
                    Subject = "Covid-19",
                    TypeName="Other",
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
                    TypeName="Other",
                    Messages = new List<MessageDetailViewModel>
                    {
                        new() { MessageId = 18, UserId = 12,Message ="hehe"}
                    }
                }
            };
    }
}