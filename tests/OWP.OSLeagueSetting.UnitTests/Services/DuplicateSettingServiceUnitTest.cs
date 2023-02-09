namespace OWP.OSLeagueSetting.UnitTests.Services;

using Factory;
using Models;
using NSubstitute;
using OSLeagueSetting.Services;
using Xunit;
using Xunit.Asserts.Compare;

public class DuplicateSettingServiceUnitTest
{
    private readonly Repository repository;

    public DuplicateSettingServiceUnitTest()
    {
        repository = Substitute.For<Repository>();
    }

    [Fact]
    public async void GetLeagueSetting_Always_ListLeagueSettingModel()
    {
        var service = new DuplicateSettingServiceImp(repository, new SettingRuleCreatorImp());
        var model = new SettingDetail
        {
            OfferLines = "1,2,3",
            Rounds = "1,2",
            BetDelay = "1,2",
            IsIptvOnly = true,
            Remark = "abc",
            Providers = "1,2",
            KOToTime = null,
            KOFromTime = new DateTime(2022, 8, 12, 2, 32, 23)
        };

        repository.GetLeagueSettingByLeagueName(Arg.Any<int?>(), Arg.Any<string>())
            .Returns(GetDupSettingInfosRaw());

        var actual = await service.IsDuplicate(model);

        DeepAssert.Equal(true, actual);
    }

    private IEnumerable<DuplicateSettingInfo> GetDupSettingInfosRaw()
        => new List<DuplicateSettingInfo>
        {
            new()
            {
                LeagueSettingId = 1,
                BetDeLayId = 1,
                IsIptvOnly = true,
                OfferLineId = 1,
                ProviderId = 1,
                Remark = "abc",
                RoundId = 1,
                KOFromTime = new TimeSpan(1,3,20),
                KOToTime = new TimeSpan(10,4,20)
            },
            new()
            {
                LeagueSettingId = 1,
                BetDeLayId = 2,
                OfferLineId = 2,
                ProviderId = 2,
                IsIptvOnly = true,
                Remark = "abc",
                RoundId = 2,
                KOFromTime = new TimeSpan(1,3,20),
                KOToTime = new TimeSpan(10,4,20)
            }
        };
}