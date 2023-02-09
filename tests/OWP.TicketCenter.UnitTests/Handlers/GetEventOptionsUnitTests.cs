using NSubstitute;
using OWP.TicketCenter.Handlers;
using OWP.TicketCenter.Models;
using OWP.TicketCenter.ViewModels;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.TicketCenter.UnitTests.Handlers;

public class GetEventOptionsUnitTests
{
    private readonly GetEventOptionHandler eventOptionHandler;

    private readonly Repository repository;

    public GetEventOptionsUnitTests()
    {
        repository = Substitute.For<Repository>();
        eventOptionHandler = new GetEventOptionHandler(repository);
    }

    [Fact]
    public async void GetEventOption_Keyword_ReturnListOption()
    {
        // Arrange
        var request = new GetTicketEventOptionRequest
        {
            Keyword = "league"
        };

        var eventOptionInfoModel = new List<EventCheckingOption>()
        {
            new ()
            {
                InfoSiteMatchId = 123,

                OwpLeagueId = 123,

                LeagueName = "V-league",

                HomeId = 111,

                AwayId = 112,

                HomeName = "VN",

                AwayName = "TL",

                EventDateTimeGmt8 = DateTime.Parse("2021/10/27"),

                RecipientId = 19,

                UserName = "sa",

                NickName = "sa_nickname",

                HasAssignee = true
            }
        };

        repository
            .GetEventOptionInfo("League")
            .ReturnsForAnyArgs(x => eventOptionInfoModel);

        // Act
        var actual = await eventOptionHandler.Handle(request, CancellationToken.None);

        // Assert
        var expected = new List<EventCheckingOptionInfoSender>
        {
            new ()
            {
                EventInfoViewModel = new List<EventCheckingOptionInfoViewModel>
                {
                    new ()
                    {
                        InfoSiteMatchId = 123,

                        OwpLeagueId = 123,

                        LeagueName = "V-league",

                        HomeId = 111,

                        AwayId = 112,

                        HomeName = "VN",

                        AwayName = "TL",

                        EventDateTimeGmt8 = DateTime.Parse("2021/10/27"),
                    }
                },
                UserViewModel = new List<UserViewModel>
                {
                    new ()
                    {
                        RecipientId = 19,

                        UserName = "sa",

                        NickName = "sa_nickname",
                    }
                }
            }
        };
        DeepAssert.Equal(expected, actual);
    }
}