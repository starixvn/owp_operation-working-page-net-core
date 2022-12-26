namespace OWP.TicketCenter.UnitTests.Services;

using AutoMapper;
using Models;
using NSubstitute;
using OWP.TicketCenter.Handlers;
using TicketCenter.Services;
using ViewModels;
using Xunit;
using Xunit.Asserts.Compare;

public class TicketDetailServiceUnitTests
{
    private readonly IMapper mapper;
    private readonly Repository repository;
    private readonly MessageService messageService;
    private readonly TicketDetailServiceImp service;

    public TicketDetailServiceUnitTests()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EventTicketOtherModel, TicketOtherDetailViewModel>();
            cfg.CreateMap<EventTicketLeagueModel, TicketLeagueDetailViewModel>();
            cfg.CreateMap<EventTicketMatchModel, TicketMatchDetailViewModel>();
            cfg.CreateMap<EventTicketSupportModel, TicketSupportDetailViewModel>();
            cfg.CreateMap<EventTicketMatchModel, TicketLiveIncidientDetailViewModel>();
        });
        repository = Substitute.For<Repository>();
        messageService = Substitute.For<MessageService>();
        mapper = configuration.CreateMapper();

        service = new TicketDetailServiceImp(mapper, repository, messageService);
    }

    [Fact]
    public async Task GetTicketMatchDetail_Always_ReturnTicketMatchDetailViewModel()
    {
        var actorId = 8;
        var request = new TicketMatchDetailRequest { ActorId = actorId, TicketId = 145 };
        var ticketMatchDetail = new EventTicketMatchModel
        {
            TicketId = 145,
            HomeName = "MU",
            AwayName = "Liv"
        };
        var recipients = new List<RecipientResponseModel> {
            new() {  RecipientId = 10, UserName = "sa", }
        };
        var messages = GetMockMessage();
        repository.GetTicketMatchDetail(request.TicketId).Returns(ticketMatchDetail);
        repository.GetRecipientByTicketId(request.TicketId).Returns(recipients);
        messageService.GetMessageListInLast2Days(request.TicketId).Returns(messages);

        var expected = new TicketMatchDetailViewModel
        {
            TicketId = 145,
            HomeName = "MU",
            AwayName = "Liv",
            Recipients = recipients,
            Messages = messages
        };

        var actual = await service.GetTicketMatchDetail(request);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetTicketLeagueDetail_Always_ReturnTicketLeagueDetailViewModel()
    {
        var actorId = 8;
        var request = new TicketLeagueDetailRequest { ActorId = actorId, TicketId = 145 };
        var ticketLeagueDetail = new EventTicketLeagueModel
        {
            TicketId = 145,
            OwpLeagueName = "League test",
        };
        var recipients = new List<RecipientResponseModel> {
            new() {  RecipientId = 10, UserName = "sa", }
        };
        var messages = GetMockMessage();
        repository.GetTicketLeagueDetail(request.TicketId).Returns(ticketLeagueDetail);
        repository.GetRecipientByTicketId(request.TicketId).Returns(recipients);
        messageService.GetMessageListInLast2Days(request.TicketId).Returns(messages);

        var expected = new TicketLeagueDetailViewModel
        {
            TicketId = 145,
            OwpLeagueName = "League test",
            Recipients = recipients,
            Messages = messages
        };

        var actual = await service.GetTicketLeagueDetail(request);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetTicketOtherDetail_Always_ReturnTicketOtherDetailViewModel()
    {
        var actorId = 8;
        var request = new TicketOtherDetailRequest { ActorId = actorId, TicketId = 145 };
        var ticketOtherDetail = new EventTicketOtherModel
        {
            TicketId = 145,
            TypeName = "Other",
        };
        var recipients = new List<RecipientResponseModel> {
            new() {  RecipientId = 10, UserName = "sa", }
        };
        var messages = GetMockMessage();
        repository.GetTicketOtherDetail(request.TicketId).Returns(ticketOtherDetail);
        repository.GetRecipientByTicketId(request.TicketId).Returns(recipients);
        messageService.GetMessageListInLast2Days(request.TicketId).Returns(messages);

        var expected = new TicketOtherDetailViewModel
        {
            TicketId = 145,
            TypeName = "Other",
            Recipients = recipients,
            Messages = messages
        };

        var actual = await service.GetTicketOtherDetail(request);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetTicketLiveIncidientDetail_Always_ReturnTicketLiveIncidientDetailViewModel()
    {
        var actorId = 8;
        var request = new TicketLiveIncidentDetailRequest { ActorId = actorId, TicketId = 145 };
        var ticketLiveIncidientDetail = new EventTicketMatchModel
        {
            TicketId = 145,
            TypeName = "Other",
            HomeName = "MU",
            AwayName = "Liv"
        };
        var recipients = new List<RecipientResponseModel> {
            new() {  RecipientId = 10, UserName = "sa", }
        };
        var messages = GetMockMessage();
        repository.GetTicketLiveIncidentDetail(request.TicketId).Returns(ticketLiveIncidientDetail);
        repository.GetRecipientByTicketId(request.TicketId).Returns(recipients);
        messageService.GetMessageListInLast2Days(request.TicketId).Returns(messages);

        var expected = new TicketLiveIncidientDetailViewModel
        {
            TicketId = 145,
            TypeName = "Other",
            HomeName = "MU",
            AwayName = "Liv",
            Recipients = recipients,
            Messages = messages
        };

        var actual = await service.GetTicketLiveIncidentDetail(request);

        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public async Task GetTicketSupportDetail_Always_ReturnTicketSupportDetailViewModel()
    {
        var actorId = 8;
        var request = new TicketSupportDetailRequest { ActorId = actorId, TicketId = 145 };
        var ticketSupportDetail = new EventTicketSupportModel
        {
            TicketId = 145
        };
        var recipients = new List<RecipientResponseModel> {
            new() {  RecipientId = 10, UserName = "sa", }
        };
        var messages = GetMockMessage();
        repository.GetTicketSupportDetail(request.TicketId).Returns(ticketSupportDetail);
        repository.GetRecipientByTicketId(request.TicketId).Returns(recipients);
        messageService.GetMessageListInLast2Days(request.TicketId).Returns(messages);

        var expected = new TicketSupportDetailViewModel
        {
            TicketId = 145,
            Recipients = recipients,
            Messages = messages
        };

        var actual = await service.GetTicketSupportDetail(request);

        DeepAssert.Equal(expected, actual);
    }

    public List<MessageDetailViewModel> GetMockMessage()
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
            }
        };
}