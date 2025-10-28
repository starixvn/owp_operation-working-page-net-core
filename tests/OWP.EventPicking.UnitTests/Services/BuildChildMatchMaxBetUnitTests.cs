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

        DeepAssert.Equal(MockExpectedMatches().First().MaxbetHdpExcel, childMatches.First().MaxbetHdpExcel);
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
            new EventPickingModel { MaxbetHdp = null },
            new EventPickingModel
            {
                MaxbetHdp = 5000,
                InfoSiteMatchId = 3333
            },

            new EventPickingModel
            {
                MaxbetHdp = 3000,
                InfoSiteMatchId = 4444
            },

            new EventPickingModel
            {
                MaxbetHdp = 100,
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
                MaxbetHdp = 2000,
                MaxbetOu = 100
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 3333,
                MinorBettypeId = -4,
                MaxbetHdp = 12000
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -6,
                MaxbetHdp = 300,
                MaxbetOu = 100
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 5555,
                MinorBettypeId = -7,
                MaxbetHdp = 333
            },

            new EventPickingModel
            {
                InfoSiteMatchId = 4444,
                MinorBettypeId = -99,
                MaxbetHdp = null
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 100,
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
        Assert.Equal(2000, childMatches[0].MaxbetHdp);
        Assert.Equal(1500, childMatches[0].MaxbetOu);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 200,
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
        Assert.Equal(3000, childMatches[0].MaxbetHdp);
        Assert.Null(childMatches[0].MaxbetOu);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 300,
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
        Assert.Equal(2500, childMatches[0].MaxbetHdp);
        Assert.Equal(2000, childMatches[0].MaxbetOu);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 400,
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
        Assert.Equal(1800, childMatches[0].MaxbetHdp);
        Assert.Null(childMatches[0].MaxbetOu);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 500,
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
        Assert.Null(childMatches[0].MaxbetHdp);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 999, // Parent not in mainEvents
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
        Assert.Null(childMatches[0].MaxbetHdp);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 100,
                MinorBettypeId = -3 // NoOfCorner
            },
            new()
            {
                ParentId = 100,
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
        Assert.Equal(2500, childMatches[0].MaxbetHdp);
        Assert.Equal(2000, childMatches[0].MaxbetOu);
        Assert.Equal(3500, childMatches[1].MaxbetHdp);
        Assert.Null(childMatches[1].MaxbetOu);
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

        var childMatches = new List<SpecialProductMaxbetSpread>
        {
            new()
            {
                ParentId = 100,
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
        Assert.Equal(999, childMatches[0].MaxbetHdp);
        Assert.Equal(888, childMatches[0].MaxbetOu);
    }

    #region Build Method Tests

    [Fact]
    public async Task Build_WithNoChildMatches_ShouldNotProcessAnyMatches()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new() { FT_HDP = 5000, NoOfCorner_FT_HDP = 2000 }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 } // No MinorBettypeId = not a child match
        };

        // Act
        await service.Build(events);

        // Assert - No child matches to process, so no changes expected
        Assert.False(events[0].IsChildMatch); // IsChildMatch is computed from MinorBettypeId.HasValue
    }

    [Fact]
    public async Task Build_WithChildMatches_ShouldApplyCorrectSettings()
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
                    NoOfCorner_FT_OU = 1000,
                    NoOfCorner_FT_Spread_HDP = 0.05m,
                    NoOfCorner_FT_Spread_OU = 0.03m,
                    NextCorner_FT_HDP = 3000,
                    NextCorner_FT_Spread_HDP = 0.04m,
                    TotalBooking_FT_HDP = 1500,
                    TotalBooking_FT_OU = 800,
                    TotalBooking_FT_Spread_HDP = 0.06m,
                    TotalBooking_FT_Spread_OU = 0.02m,
                    NextBooking_FT_HDP = 2500,
                    NextBooking_FT_Spread_HDP = 0.07m
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }, // No MinorBettypeId = not a child match
            // Child matches
            new() { InfoSiteMatchId = 1, MinorBettypeId = -3 }, // NoOfCorner
            new() { InfoSiteMatchId = 1, MinorBettypeId = -4 }, // NextCorner
            new() { InfoSiteMatchId = 1, MinorBettypeId = -6 }, // TotalBooking
            new() { InfoSiteMatchId = 1, MinorBettypeId = -7 }  // NextBooking
        };

        // Act
        await service.Build(events);

        // Assert - Check NoOfCorner settings
        var noOfCornerChild = events[1];
        Assert.Equal(2000, noOfCornerChild.MaxbetHdp);
        Assert.Equal(1000, noOfCornerChild.MaxbetOu);
        Assert.Equal(0.05m, noOfCornerChild.SpreadHdp);
        Assert.Equal(0.03m, noOfCornerChild.SpreadOu);

        // Assert - Check NextCorner settings
        var nextCornerChild = events[2];
        Assert.Equal(3000, nextCornerChild.MaxbetHdp);
        Assert.Equal(0.04m, nextCornerChild.SpreadHdp);

        // Assert - Check TotalBooking settings
        var totalBookingChild = events[3];
        Assert.Equal(1500, totalBookingChild.MaxbetHdp);
        Assert.Equal(800, totalBookingChild.MaxbetOu);
        Assert.Equal(0.06m, totalBookingChild.SpreadHdp);
        Assert.Equal(0.02m, totalBookingChild.SpreadOu);

        // Assert - Check NextBooking settings
        var nextBookingChild = events[4];
        Assert.Equal(2500, nextBookingChild.MaxbetHdp);
        Assert.Equal(0.07m, nextBookingChild.SpreadHdp);
    }

    [Fact]
    public async Task Build_WithNullCoalescing_ShouldNotOverwriteExistingValues()
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
                    NoOfCorner_FT_OU = 1000,
                    NoOfCorner_FT_Spread_HDP = 0.05m,
                    NoOfCorner_FT_Spread_OU = 0.03m
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }, // No MinorBettypeId = not a child match
            // Child match with existing values
            new()
            {
                InfoSiteMatchId = 1,
                MinorBettypeId = -3,
                MaxbetHdp = 9999, // Existing value
                MaxbetOu = 8888,  // Existing value
                SpreadHdp = 0.99m, // Existing value
                SpreadOu = 0.88m   // Existing value
            }
        };

        // Act
        await service.Build(events);

        // Assert - Existing values should not be overwritten due to null-coalescing
        var child = events[1];
        Assert.Equal(9999, child.MaxbetHdp); // Should remain unchanged
        Assert.Equal(8888, child.MaxbetOu);  // Should remain unchanged
        Assert.Equal(0.99m, child.SpreadHdp); // Should remain unchanged
        Assert.Equal(0.88m, child.SpreadOu);  // Should remain unchanged
    }

    [Fact]
    public async Task Build_WithNullValues_ShouldApplyDefaultSettings()
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
                    NoOfCorner_FT_OU = 1000,
                    NoOfCorner_FT_Spread_HDP = 0.05m,
                    NoOfCorner_FT_Spread_OU = 0.03m
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }, // No MinorBettypeId = not a child match
            // Child match with null values
            new()
            {
                InfoSiteMatchId = 1,
                MinorBettypeId = -3,
                MaxbetHdp = null, // Null value
                MaxbetOu = null,  // Null value
                SpreadHdp = null, // Null value
                SpreadOu = null   // Null value
            }
        };

        // Act
        await service.Build(events);

        // Assert - Null values should be replaced with default settings
        var child = events[1];
        Assert.Equal(2000, child.MaxbetHdp);
        Assert.Equal(1000, child.MaxbetOu);
        Assert.Equal(0.05m, child.SpreadHdp);
        Assert.Equal(0.03m, child.SpreadOu);
    }

    [Fact]
    public async Task Build_WithUnknownBetType_ShouldSetMaxbetHdpToNull()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new() { FT_HDP = 5000 }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }, // No MinorBettypeId = not a child match
            // Child match with unknown bet type
            new()
            {
                InfoSiteMatchId = 1,
                MinorBettypeId = -999 // Unknown bet type
            }
        };

        // Act
        await service.Build(events);

        // Assert - Unknown bet type should set MaxbetHdp to null
        var child = events[1];
        Assert.Null(child.MaxbetHdp);
    }

    [Fact]
    public async Task Build_WithNoMatchingMainMatch_ShouldNotApplySettings()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new() { FT_HDP = 5000, NoOfCorner_FT_HDP = 2000 }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match with different InfoSiteMatchId
            new() { InfoSiteMatchId = 999, MaxbetHdp = 5000 }, // No MinorBettypeId = not a child match
            // Child match looking for non-existent parent
            new()
            {
                InfoSiteMatchId = 1, // No matching main match
                MinorBettypeId = -3
            }
        };

        // Act
        await service.Build(events);

        // Assert - No settings should be applied since no matching main match
        var child = events[1];
        Assert.Null(child.MaxbetHdp);
        Assert.Null(child.MaxbetOu);
        Assert.Null(child.SpreadHdp);
        Assert.Null(child.SpreadOu);
    }

    [Fact]
    public async Task Build_WithMainMatchHavingNullMaxbetHdp_ShouldNotApplySettings()
    {
        // Arrange
        var maxbetSettingService = Substitute.For<MaxbetSettingService>();
        maxbetSettingService.QueryMaxbetSetting().Returns(Task.FromResult<IList<MaxBetSettingModel>>(
            new List<MaxBetSettingModel>
            {
                new() { FT_HDP = 5000, NoOfCorner_FT_HDP = 2000 }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match with null MaxbetHdp
            new() { InfoSiteMatchId = 1, MaxbetHdp = null }, // No MinorBettypeId = not a child match
            // Child match
            new()
            {
                InfoSiteMatchId = 1,
                MinorBettypeId = -3
            }
        };

        // Act
        await service.Build(events);

        // Assert - No settings should be applied since main match has null MaxbetHdp
        var child = events[1];
        Assert.Null(child.MaxbetHdp);
        Assert.Null(child.MaxbetOu);
        Assert.Null(child.SpreadHdp);
        Assert.Null(child.SpreadOu);
    }

    [Fact]
    public async Task Build_WithMultipleChildMatches_ShouldProcessAllCorrectly()
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
                    NoOfCorner_FT_OU = 1000,
                    NextCorner_FT_HDP = 3000,
                    TotalBooking_FT_HDP = 1500,
                    TotalBooking_FT_OU = 800,
                    NextBooking_FT_HDP = 2500
                }
            }));

        var service = new BuildChildMatchMaxBetImp(maxbetSettingService);
        var events = new List<EventPickingModel>
        {
            // Main match
            new() { InfoSiteMatchId = 1, MaxbetHdp = 5000 }, // No MinorBettypeId = not a child match
            // Multiple child matches
            new() { InfoSiteMatchId = 1, MinorBettypeId = -3 }, // NoOfCorner
            new() { InfoSiteMatchId = 1, MinorBettypeId = -4 }, // NextCorner
            new() { InfoSiteMatchId = 1, MinorBettypeId = -6 }, // TotalBooking
            new() { InfoSiteMatchId = 1, MinorBettypeId = -7 }, // NextBooking
            new() { InfoSiteMatchId = 1, MinorBettypeId = -3 }  // Another NoOfCorner
        };

        // Act
        await service.Build(events);

        // Assert - All child matches should be processed
        Assert.Equal(2000, events[1].MaxbetHdp); // First NoOfCorner
        Assert.Equal(3000, events[2].MaxbetHdp); // NextCorner
        Assert.Equal(1500, events[3].MaxbetHdp); // TotalBooking
        Assert.Equal(2500, events[4].MaxbetHdp); // NextBooking
        Assert.Equal(2000, events[5].MaxbetHdp); // Second NoOfCorner
    }

    #endregion
}