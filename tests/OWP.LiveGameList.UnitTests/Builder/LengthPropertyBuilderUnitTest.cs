namespace OWP.LiveGameList.UnitTests.Builder;

using LiveGameList.Builder;
using Models;
using Xunit;

public class LengthPropertyBuilderUnitTest
{
    [Fact]
    public void Build_MainMatch_ReturnExpect()
    {
        var matchInfo = new MatchInfo
        {
            League = "V-league",
            MatchOffers = new List<MatchOffer> { new() { MatchOfferValue = "6s" } },
            Remark = "ok",
            Round = "final",
            Division = "job",
            Assignment = new AssignmentModel { Handicap = new List<Assignment> { new() { Username = "Johnny" } } },
            Televisions = new List<Television> { new() { TVCode = "ok ok " } },
            Provider = "provider",
            WatchBet = "ok",
            Phone = "ok none "
        };
        LengthPropertyBuilder.BuildLengthProperty(matchInfo);

        Assert.Equal(10, matchInfo.LengthLeague);
        Assert.Equal(0, matchInfo.LengthOtherProductTenMinutes);
        Assert.Equal(10, matchInfo.LengthRemark);
    }
}
