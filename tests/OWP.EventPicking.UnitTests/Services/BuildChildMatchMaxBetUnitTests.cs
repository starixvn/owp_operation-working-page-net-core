using NSubstitute;
using OWP.Models;
using OWP.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

using OWP.EventPicking.Api.Models;
using OWP.EventPicking.Api.Services;
using Xunit.Asserts.Compare;

public class BuildChildMatchMaxBetUnitTests
{
    private readonly BuildChildMatchMaxBetImp services;

    public BuildChildMatchMaxBetUnitTests()
    {
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        services = new BuildChildMatchMaxBetImp(maxbetSettingService);
    }

    [Fact]
    public void BuildChildMatchMaxbet()
    {
        var childMatches = MockChildMatches();
        var mainMatches = MockMainMatches();
        var maxbetSettings = new Dictionary<decimal, MaxBetSettingModel>
        {
            { 5000, new MaxBetSettingModel {
                FT_HDP = 5000,
                NoOfCorner_FT_HDP = 2000,
                NoOfCorner_FT_OU = 100,
                NextCorner_FT_HDP = 12000
            } },
            { 4000, new() {FT_HDP = 4000, NextCorner_FT_HDP = 400, NextBooking_FT_HDP = 100} },
            { 3000, new() { FT_HDP = 3000, TotalBooking_FT_HDP = 300, TotalBooking_FT_OU = 100} },
            { -1000, new() { FT_HDP = -1000, NextCorner_FT_HDP = 200, NextBooking_FT_HDP = 333 }}
        };

        services.BuildExport(mainMatches, childMatches, maxbetSettings);

        DeepAssert.Equal(MockExpectedMatches().First().MaxBetHDP, childMatches.First().MaxBetHDP);
    }

    public List<EventPickingModel> MockChildMatches()
        =>
        [
            new EventPickingModel
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -3
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -6
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 5555,
                MinorBettypeId = -7
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99
            }
        ];

    public List<EventPickingModel> MockMainMatches()
        =>
        [
            new EventPickingModel { MaxBetHDP = null },
            new EventPickingModel
            {
                MaxBetHDPNumber = 5000,
                InfoSiteMatchId = 3333
            },

            new EventPickingModel
            {
                MaxBetHDPNumber = 3000,
                InfoSiteMatchId = 4444
            },

            new EventPickingModel
            {
                MaxBetHDPNumber = 100,
                InfoSiteMatchId = 5555
            }
        ];

    public List<EventPickingModel> MockExpectedMatches()
        =>
        [
            new EventPickingModel
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -3,
                MaxBetHDP = "2K",
                MaxBetOU = "0.1K"
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4,
                MaxBetHDP = "12K"
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -6,
                MaxBetHDP = "0.3K",
                MaxBetOU = "0.1K"
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 5555,
                MinorBettypeId = -7,
                MaxBetHDP = "0.333K"
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99,
                MaxBetHDP = ""
            }
        ];

    [Fact]
    public async Task BuildMBForAddSpecialProduct_NoOfCorner_ShouldSetCorrectMaxBet()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 5000,
                    NoOfCorner_FT_HDP = 2000,
                    NoOfCorner_FT_OU = 1500
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 100,
                MinorBettypeId = -3 // NoOfCorner
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 100,
                MaxbetHdp = 5000
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert
        Assert.Equal(2000, childMatches[0].MaxbetHDP);
        Assert.Equal(1500, childMatches[0].MaxbetOU);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_NextCorner_ShouldSetCorrectMaxBet()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 10000,
                    NextCorner_FT_HDP = 3000
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 200,
                MinorBettypeId = -4 // NextCorner
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 200,
                MaxbetHdp = 10000
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert
        Assert.Equal(3000, childMatches[0].MaxbetHDP);
        Assert.Null(childMatches[0].MaxbetOU);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_TotalBooking_ShouldSetCorrectMaxBet()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 7500,
                    TotalBooking_FT_HDP = 2500,
                    TotalBooking_FT_OU = 2000
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 300,
                MinorBettypeId = -6 // TotalBooking
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 300,
                MaxbetHdp = 7500
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert
        Assert.Equal(2500, childMatches[0].MaxbetHDP);
        Assert.Equal(2000, childMatches[0].MaxbetOU);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_NextBooking_ShouldSetCorrectMaxBet()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 6000,
                    NextBooking_FT_HDP = 1800
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 400,
                MinorBettypeId = -7 // NextBooking
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 400,
                MaxbetHdp = 6000
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert
        Assert.Equal(1800, childMatches[0].MaxbetHDP);
        Assert.Null(childMatches[0].MaxbetOU);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_UnknownBettypeId_ShouldSetMaxBetToNull()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 5000,
                    NoOfCorner_FT_HDP = 2000
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 500,
                MinorBettypeId = -999 // Unknown bettype
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 500,
                MaxbetHdp = 5000
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert
        Assert.Null(childMatches[0].MaxbetHDP);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_ParentNotFound_ShouldNotSetMaxBet()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 5000,
                    NoOfCorner_FT_HDP = 2000
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 999, // Parent not in mainEvents
                MinorBettypeId = -3
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 100,
                MaxbetHdp = 5000
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert - Should remain null when parent not found
        Assert.Null(childMatches[0].MaxbetHDP);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_MultipleChildMatches_ShouldProcessAll()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = 8000,
                    NoOfCorner_FT_HDP = 2500,
                    NoOfCorner_FT_OU = 2000,
                    NextCorner_FT_HDP = 3500
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 100,
                MinorBettypeId = -3 // NoOfCorner
            },
            new()
            {
                ParentMatchId = 100,
                MinorBettypeId = -4 // NextCorner
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 100,
                MaxbetHdp = 8000
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert
        Assert.Equal(2500, childMatches[0].MaxbetHDP);
        Assert.Equal(2000, childMatches[0].MaxbetOU);
        Assert.Equal(3500, childMatches[1].MaxbetHDP);
        Assert.Null(childMatches[1].MaxbetOU);
    }

    [Fact]
    public async Task BuildMBForAddSpecialProduct_UseOtherKeyWhenMaxBetNotInDictionary()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new()
                {
                    FT_HDP = -1000, // Other key
                    NoOfCorner_FT_HDP = 999,
                    NoOfCorner_FT_OU = 888
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);

        var childMatches = new List<ChildMatchModel>
        {
            new()
            {
                ParentMatchId = 100,
                MinorBettypeId = -3 // NoOfCorner
            }
        };

        var mainEvents = new List<EventPickingModel>
        {
            new()
            {
                InfoSiteMatchId = 100,
                MaxbetHdp = 12345 // Not in settings dictionary
            }
        };

        // Act
        await service.BuildMBForAddSpecialProduct(childMatches, mainEvents);

        // Assert - Should use -1000 (other key) settings
        Assert.Equal(999, childMatches[0].MaxbetHDP);
        Assert.Equal(888, childMatches[0].MaxbetOU);
    }
}