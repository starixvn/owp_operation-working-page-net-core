using OWP.OtherSport.Submit.Constants;
using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Submit.Services;
using OWP.OtherSport.Validation.Models;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.OtherSport.UnitTests.Submit.Services;

public class AssignmentMaxbetMapperUnitTests
{
    private readonly AssignmentMaxbetMapperImp services;

    public AssignmentMaxbetMapperUnitTests()
    {
        services = new AssignmentMaxbetMapperImp();
    }

    //MapBySports
    [Fact]
    public void MapBySports()
    {
        //Arrange
        var assignmentModels = new List<AssignmentModel>();
        var item = AssignmentModelMock(Sport.OtherSports);
        assignmentModels.Add(item);

        var assignmentMaxbets = new List<AssignmentMaxbet>
        {
            new()
            {
                Bettype = "Match Winner",
                Trader = "TraderOtherSportsMatchWinner",
                OtherTrader = "OtherTraderOtherSportsMatchWinner",
                MaxBet = 1200,
                Spread = 10
            },
            new()
            {
                Bettype = "FT (HDP)",
                Trader = "TraderOtherSportsFTHDP",
                OtherTrader = "OtherTraderOtherSportsFTHDP",
                MaxBet = 1200,
                Spread = 10
            },
            new()
            {
                Bettype = "1H (HDP)",
                Trader = "TraderOtherSports1HHDP",
                OtherTrader = "OtherTraderOtherSports1HHDP",
                MaxBet = 1200,
                Spread = 10
            },
            new()
            {
                Bettype = "FT (O/U)",
                Trader = "TraderOtherSportsFTOU",
                OtherTrader = "OtherTraderOtherSportsFTOU",
                MaxBet = 1200,
                Spread = 10
            },
            new()
            {
                Bettype = "1H (O/U)",
                Trader = "TraderOtherSports1HOU",
                OtherTrader = "OtherTraderOtherSports1HOU",
                MaxBet = 1200,
                Spread = 10
            }
        };

        var expect = new Dictionary<int, List<AssignmentMaxbet>> { { 0, assignmentMaxbets } };

        //Act
        var actual = services.MapBySports(assignmentModels);

        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapBySport
    [Fact]
    public void MapBySport_Get_AssignmentMaxbet_in_AssignmentModel()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Football);

        var expect = new List<AssignmentMaxbet>
        {
            new() {Bettype = "FT (HDP)", MaxBet = 1200,
                Spread = 10, OtherTrader = "OtherTraderFootballFTHDP", Trader = "TraderFootballFTHDP"},
            new() {Bettype = "1H (HDP)", MaxBet = 1200,
                Spread = 10, OtherTrader = "OtherTraderFootball1HHDP", Trader = "TraderFootball1HHDP" },
            new() {Bettype = "1Q & 3Q (HDP)", MaxBet = 1200,
                Spread = 10, OtherTrader = "OtherTraderFootball1Q3QHDP", Trader = "TraderFootball1Q3QHDP"},
            new() {Bettype = "FT (OU)", MaxBet = 1200,
                Spread = 10, OtherTrader = "OtherTraderFootballFTOU", Trader = "TraderFootballFTOU"},
            new() {Bettype = "1H (OU)", MaxBet = 1200,
                Spread = 10, OtherTrader = "OtherTraderFootball1HOU", Trader = "TraderFootball1HOU"},
            new() {Bettype = "1Q & 3Q (OU)", MaxBet = 1200,
                Spread = 10, OtherTrader = "OtherTraderFootball1Q3QOU", Trader = "TraderFootball1Q3QOU"}
        };

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBySport(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapBasketball
    [Fact]
    public void Get_Basketball_in_AssignmentModel_Contain_BasketBall()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Basketball);

        var expect = new List<AssignmentMaxbet>
        {
            new() {Bettype = "FT (HDP)", MaxBet = 1200, Spread =10, OtherTrader = "OtherTraderBasketballFTHDP", Trader = "TraderBasketballFTHDP"},
            new() {Bettype = "1H (HDP)", MaxBet = null, OtherTrader = "OtherTraderBasketball1HHDP", Trader = "TraderBasketball1HHDP"},
            new() {Bettype = "1Q & 3Q (HDP)", MaxBet = null, OtherTrader = "OtherTraderBasketball1Q3QHDP", Trader = "TraderBasketball1Q3QHDP"},
            new() {Bettype = "FT (OU/OE)", MaxBet = 1200, Spread=10, OtherTrader = "OtherTraderBasketballFTOUOE", Trader = "TraderBasketballFTOUOE"},
            new() {Bettype = "FT (OE)", MaxBet = 13000, Spread=10, OtherTrader = null, Trader = null},
            new() {Bettype = "1H (OU/OE)", MaxBet = null, OtherTrader = "OtherTraderBasketball1HOUOE", Trader = "TraderBasketball1HOUOE"},
            new() {Bettype = "1Q & 3Q (OU/OE)", MaxBet = null, OtherTrader = "OtherTraderBasketball1Q3QOUOE", Trader = "TraderBasketball1Q3QOUOE"}
        };

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBasketball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Basketball_in_AssignmentModel_not_Contain_BasketBall()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Football);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBasketball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapFootball
    [Fact]
    public void Get_Football_in_AssignmentModel_Contain_Football()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.FieldHockey);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapFootball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Football_in_AssignmentModel_not_Contain_Football()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Football);

        var expect = new List<AssignmentMaxbet>{ new()
            {
                Bettype = Bettype.FTHDP,
                Trader = assignment.TraderFootballFTHDP,
                OtherTrader = assignment.OtherTraderFootballFTHDP,
                MaxBet = assignment.MaxBetFootballFTHDP,
                Spread = assignment.SpreadFootballFTHDP
            },
            new()
            {
                Bettype = Bettype.FirstHHDP,
                Trader = assignment.TraderFootball1HHDP,
                OtherTrader = assignment.OtherTraderFootball1HHDP,
                MaxBet = assignment.MaxBetFootball1HHDP,
                Spread = assignment.SpreadFootball1HHDP
            },
            new()
            {
                Bettype = Bettype.FirstQ3QHDP,
                Trader = assignment.TraderFootball1Q3QHDP,
                OtherTrader = assignment.OtherTraderFootball1Q3QHDP,
                MaxBet = assignment.MaxBetFootball1Q3QHDP,
                Spread = assignment.SpreadFootball1Q3QHDP
            },
            new()
            {
                Bettype = Bettype.FTOU,
                Trader = assignment.TraderFootballFTOU,
                OtherTrader = assignment.OtherTraderFootballFTOU,
                MaxBet = assignment.MaxBetFootballFTOU,
                Spread = assignment.SpreadFootballFTOU
            },
            new()
            {
                Bettype = Bettype.FirstHOU,
                Trader = assignment.TraderFootball1HOU,
                OtherTrader = assignment.OtherTraderFootball1HOU,
                MaxBet = assignment.MaxBetFootball1HOU,
                Spread = assignment.SpreadFootball1HOU
            },
            new()
            {
                Bettype = Bettype.FirstQ3QOU,
                Trader = assignment.TraderFootball1Q3QOU,
                OtherTrader = assignment.OtherTraderFootball1Q3QOU,
                MaxBet = assignment.MaxBetFootball1Q3QOU,
                Spread = assignment.SpreadFootball1Q3QOU
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapFootball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapFieldHockey
    [Fact]
    public void Get_FieldHockey_in_AssignmentModel_Contain_FieldHockey()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.FieldHockey);

        var expect = new List<AssignmentMaxbet> {new()
            {
                Bettype = Bettype.FTHDP,
                Trader = assignment.TraderFieldHockeyFTHDP,
                OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                MaxBet = assignment.MaxBetFieldHockeyFTHDP,
                Spread = assignment.SpreadFieldHockeyFTHDP
            },
            new()
            {
                Bettype = Bettype.FirstHHDP,
                Trader = assignment.TraderFieldHockey1HHDP,
                OtherTrader = assignment.OtherTraderFieldHockey1HHDP,
                MaxBet = assignment.MaxBetFieldHockey1HHDP,
                Spread = assignment.SpreadFieldHockey1HHDP
            },
            new()
            {
                Bettype = Bettype.FirstQHDP,
                Trader = assignment.TraderFieldHockey1QHDP,
                OtherTrader = assignment.OtherTraderFieldHockey1QHDP,
                MaxBet = assignment.MaxBetFieldHockey1QHDP,
                Spread = assignment.SpreadFieldHockey1QHDP
            },
            new()
            {
                Bettype = Bettype.FTOsU,
                Trader = assignment.TraderFieldHockeyFTOU,
                OtherTrader = assignment.OtherTraderFieldHockeyFTOU,
                MaxBet = assignment.MaxBetFieldHockeyFTOU,
                Spread = assignment.SpreadFieldHockeyFTOU
            },
            new()
            {
                Bettype = Bettype.FirstHOsU,
                Trader = assignment.TraderFieldHockey1HOU,
                OtherTrader = assignment.OtherTraderFieldHockey1HOU,
                MaxBet = assignment.MaxBetFieldHockey1HOU,
                Spread = assignment.SpreadFieldHockey1HOU
            },
            new()
            {
                Bettype = Bettype.FirstQOU,
                Trader = assignment.TraderFieldHockey1QOU,
                OtherTrader = assignment.OtherTraderFieldHockey1QOU,
                MaxBet = assignment.MaxBetFieldHockey1QOU,
                Spread = assignment.SpreadFieldHockey1QOU
            } };

        //Act
        var actual = AssignmentMaxbetMapperImp.MapFieldHockey(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_FieldHockey_in_AssignmentModel_not_Contain_FieldHockey()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Darts);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapFieldHockey(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapIceHockey
    [Fact]
    public void Get_IceHockey_in_AssignmentModel_Contain_IceHockey()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.IceHockey);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.FTHDP,
                Trader = assignment.TraderIceHockeyFTHDP,
                OtherTrader = assignment.OtherTraderIceHockeyFTHDP,
                MaxBet = assignment.MaxBetIceHockeyFTHDP,
                Spread = assignment.SpreadIceHockeyFTHDP
            },
            new()
            {
                Bettype = Bettype.First2ndPeriodsHDP,
                Trader = assignment.TraderIceHockey1st2ndPeriodsHDP,
                OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsHDP,
                MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsHDP,
                Spread = assignment.SpreadIceHockey1st2ndPeriodsHDP
            },
            new()
            {
                Bettype = Bettype.FTOsU,
                Trader = assignment.TraderIceHockeyFTOU,
                OtherTrader = assignment.OtherTraderIceHockeyFTOU,
                MaxBet = assignment.MaxBetIceHockeyFTOU,
                Spread = assignment.SpreadIceHockeyFTOU
            },
            new()
            {
                Bettype = Bettype.First2ndPeriodsOU,
                Trader = assignment.TraderIceHockey1st2ndPeriodsOU,
                OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsOU,
                MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsOU,
                Spread = assignment.SpreadIceHockey1st2ndPeriodsOU
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapIceHockey(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_IceHockey_in_AssignmentModel_not_Contain_IceHockey()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Darts);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapIceHockey(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapOtherSports
    [Fact]
    public void Get_OtherSports_in_AssignmentModel_Contain_OtherSports()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.OtherSports);

        var expect = new List<AssignmentMaxbet> {
            new ()
            {
                Bettype = Bettype.MatchWinner,
                Trader = assignment.TraderOtherSportsMatchWinner,
                OtherTrader = assignment.OtherTraderOtherSportsMatchWinner,
                MaxBet = assignment.MaxBetOtherSportsMatchWinner,
                Spread = assignment.SpreadOtherSportsMatchWinner
            },
            new ()
            {
                Bettype = Bettype.FTHDP,
                Trader = assignment.TraderOtherSportsFTHDP,
                OtherTrader = assignment.OtherTraderOtherSportsFTHDP,
                MaxBet = assignment.MaxBetOtherSportsFTHDP,
                Spread = assignment.SpreadOtherSportsFTHDP
            },
            new ()
            {
                Bettype = Bettype.FirstHHDP,
                Trader = assignment.TraderOtherSports1HHDP,
                OtherTrader = assignment.OtherTraderOtherSports1HHDP,
                MaxBet = assignment.MaxBetOtherSports1HHDP,
                Spread = assignment.SpreadOtherSports1HHDP
            },
            new ()
            {
                Bettype = Bettype.FTOsU,
                Trader = assignment.TraderOtherSportsFTOU,
                OtherTrader = assignment.OtherTraderOtherSportsFTOU,
                MaxBet = assignment.MaxBetOtherSportsFTOU,
                Spread = assignment.SpreadOtherSportsFTOU
            },
            new ()
            {
                Bettype = Bettype.FirstHOsU,
                Trader = assignment.TraderOtherSports1HOU,
                OtherTrader = assignment.OtherTraderOtherSports1HOU,
                MaxBet = assignment.MaxBetOtherSports1HOU,
                Spread = assignment.SpreadOtherSports1HOU
            }
        };

        //Act
        var actual = AssignmentMaxbetMapperImp.MapOtherSports(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_OtherSports_in_AssignmentModel_not_Contain_OtherSports()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Darts);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapOtherSports(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapDarts
    [Fact]
    public void Get_Darts_in_AssignmentModel_Contain_Darts()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Darts);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.LegsHDP,
                Trader = assignment.TraderDartsLegsHDP,
                OtherTrader = assignment.OtherTraderDartsLegsHDP,
                MaxBet = assignment.MaxBetDartsLegsHDP,
                Spread = assignment.SpreadDartsLegsHDP
            },
            new()
            {
                Bettype = Bettype.Most180sHDP,
                Trader = assignment.TraderDartsMost180sHDP,
                OtherTrader = assignment.OtherTraderDartsMost180sHDP,
                MaxBet = assignment.MaxBetDartsMost180sHDP,
                Spread = assignment.SpreadDartsMost180sHDP
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapDarts(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Darts_in_AssignmentModel_not_Contain_Darts()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapDarts(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapPool
    [Fact]
    public void Get_Pool_in_AssignmentModel_Contain_Pool()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Pool);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.MatchWinnerHDP,
                Trader = assignment.TraderPoolMatchWinnerHDP,
                OtherTrader = assignment.OtherTraderPoolMatchWinnerHDP,
                MaxBet = assignment.MaxBetPoolMatchWinnerHDP,
                Spread = assignment.SpreadPoolMatchWinnerHDP
            },
            new()
            {
                Bettype = Bettype.RacksWinnerHDP,
                Trader = assignment.TraderPoolRacksWinnerHDP,
                OtherTrader = assignment.OtherTraderPoolRacksWinnerHDP,
                MaxBet = assignment.MaxBetPoolRacksWinnerHDP,
                Spread = assignment.SpreadPoolRacksWinnerHDP
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapPool(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Pool_in_AssignmentModel_not_Contain_Pool()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapPool(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapTableTennis
    [Fact]
    public void Get_TableTennis_in_AssignmentModel_Contain_TableTennis()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.TableTennis);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.SetWinnerHDP,
                Trader = assignment.TraderTableTennisSetWinnerHDP,
                OtherTrader = assignment.OtherTraderTableTennisSetWinnerHDP,
                MaxBet = assignment.MaxBetTableTennisSetWinnerHDP,
                Spread = assignment.SpreadTableTennisSetWinnerHDP
            },
            new()
            {
                Bettype = Bettype.SetXWinnerHDP,
                Trader = assignment.TraderTableTennisSetXWinnerHDP,
                OtherTrader = assignment.OtherTraderTableTennisSetXWinnerHDP,
                MaxBet = assignment.MaxBetTableTennisSetXWinnerHDP,
                Spread = assignment.SpreadTableTennisSetXWinnerHDP
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapTableTennis(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_TableTennis_in_AssignmentModel_not_Contain_TableTennis()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapTableTennis(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapVolleyball
    [Fact]
    public void Get_Volleyball_in_AssignmentModel_Contain_Volleyball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Volleyball);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.SetWinnerHDPOE,
                Trader = assignment.TraderVolleyballSetWinnerHDPOE,
                OtherTrader = assignment.OtherTraderVolleyballSetWinnerHDPOE,
                MaxBet = assignment.MaxBetVolleyballSetWinnerHDPOE,
                Spread = assignment.SpreadVolleyballSetWinnerHDPOE
            },
            new()
            {
                Bettype = Bettype.SetWinnerOE,
                MaxBet = assignment.MaxBetVolleyballSetWinnerOE,
                Spread = assignment.SpreadVolleyballSetWinnerOE
            },
            new()
            {
                Bettype = Bettype.SetsXPointWinnerHDP,
                Trader = assignment.TraderVolleyballSetsXPointWinnerHDP,
                OtherTrader = assignment.OtherTraderVolleyballSetsXPointWinnerHDP,
                MaxBet = assignment.MaxBetVolleyballSetsXPointWinnerHDP,
                Spread = assignment.SpreadVolleyballSetsXPointWinnerHDP
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapVolleyball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Volleyball_in_AssignmentModel_not_Contain_Volleyball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapVolleyball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapHandball
    [Fact]
    public void Get_Handball_in_AssignmentModel_Contain_Handball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Handball);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.FTHDP,
                Trader = assignment.TraderHandballFTHDP,
                OtherTrader = assignment.OtherTraderHandballFTHDP,
                MaxBet = assignment.MaxBetHandballFTHDP,
                Spread = assignment.SpreadHandballFTHDP
            },
            new()
            {
                Bettype = Bettype.FirstHHDP,
                Trader = assignment.TraderHandball1HHDP,
                OtherTrader = assignment.OtherTraderHandball1HHDP,
                MaxBet = assignment.MaxBetHandball1HHDP,
                Spread = assignment.SpreadHandball1HHDP
            },
            new()
            {
                Bettype = Bettype.FTOsU,
                Trader = assignment.TraderHandballFTOU,
                OtherTrader = assignment.OtherTraderHandballFTOU,
                MaxBet = assignment.MaxBetHandballFTOU,
                Spread = assignment.SpreadHandballFTOU
            },
            new()
            {
                Bettype = Bettype.FirstHOsU,
                Trader = assignment.TraderHandball1HOU,
                OtherTrader = assignment.OtherTraderHandball1HOU,
                MaxBet = assignment.MaxBetHandball1HOU,
                Spread = assignment.SpreadHandball1HOU
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapHandball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Handball_in_AssignmentModel_not_Contain_Handball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapHandball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapNetball
    [Fact]
    public void Get_Netball_in_AssignmentModel_Contain_Netball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Netball);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.FTHDP,
                Trader = assignment.TraderNetballFTHDP,
                OtherTrader = assignment.OtherTraderNetballFTHDP,
                MaxBet = assignment.MaxBetNetballFTHDP,
                Spread = assignment.SpreadNetballFTHDP
            },
            new()
            {
                Bettype = Bettype.FirstHHDP,
                Trader = assignment.TraderNetball1HHDP,
                OtherTrader = assignment.OtherTraderNetball1HHDP,
                MaxBet = assignment.MaxBetNetball1HHDP,
                Spread = assignment.SpreadNetball1HHDP
            },
            new()
            {
                Bettype = Bettype.FirstQ3QHDP,
                Trader = assignment.TraderNetball1Q3QHDP,
                OtherTrader = assignment.OtherTraderNetball1Q3QHDP,
                MaxBet = assignment.MaxBetNetball1Q3QHDP,
                Spread = assignment.SpreadNetball1Q3QHDP
            },
            new()
            {
                Bettype = Bettype.FTOUOE,
                Trader = assignment.TraderNetballFTOUOE,
                OtherTrader = assignment.OtherTraderNetballFTOUOE,
                MaxBet = assignment.MaxBetNetballFTOUOE,
                Spread = assignment.SpreadNetballFTOUOE
            },
            new()
            {
                Bettype = Bettype.FTOE,
                MaxBet = assignment.MaxBetNetballFTOE,
                Spread = assignment.SpreadNetballFTOE
            },
            new()
            {
                Bettype = Bettype.FirstHOUOE,
                Trader = assignment.TraderNetball1HOUOE,
                OtherTrader = assignment.OtherTraderNetball1HOUOE,
                MaxBet = assignment.MaxBetNetball1HOUOE,
                Spread = assignment.SpreadNetball1HOUOE
            },
            new()
            {
                Bettype = Bettype.FirstHOE,
                MaxBet = assignment.MaxBetNetball1HOE,
                Spread = assignment.SpreadNetball1HOE
            },
            new()
            {
                Bettype = Bettype.FirstQ3QOUOE,
                Trader = assignment.TraderNetball1Q3QOUOE,
                OtherTrader = assignment.OtherTraderNetball1Q3QOUOE,
                MaxBet = assignment.MaxBetNetball1Q3QOUOE,
                Spread = assignment.SpreadNetball1Q3QOUOE
            },
            new()
            {
                Bettype = Bettype.FirstQ3QOE,
                MaxBet = assignment.MaxBetNetball1Q3QOE,
                Spread = assignment.SpreadNetball1Q3QOE
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapNetball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Netball_in_AssignmentModel_not_Contain_Netball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapNetball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapCricket
    [Fact]
    public void Get_Cricket_in_AssignmentModel_Contain_Cricket()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Cricket);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.MatchWinnerInningsOU,
                Trader = assignment.TraderCricketMatchWinnerInningsOU,
                OtherTrader = assignment.OtherTraderCricketMatchWinnerInningsOU,
                Spread = assignment.SpreadCricketMatchWinnerInningsOU
            },
            new()
            {
                Bettype = Bettype.NextOverOUPlayerOU,
                Trader = assignment.TraderCricketNextOverOUPlayerOU,
                OtherTrader = assignment.OtherTraderCricketNextOverOUPlayerOU,
                Spread = assignment.SpreadCricketNextOverOUPlayerOU
            },
            new()
            {
                Bettype = Bettype.FT1X2,
                Trader = assignment.TraderCricketFT1X2,
                OtherTrader = assignment.OtherTraderCricketFT1X2,
                Spread = assignment.SpreadCricketFT1X2
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapCricket(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Cricket_in_AssignmentModel_not_Contain_Cricket()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapCricket(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapSnooker
    [Fact]
    public void Get_Snooker_in_AssignmentModel_Contain_Snooker()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Snooker);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.MatchWinnerHDPOU,
                Trader = assignment.TraderSnookerMatchWinnerHDPOU,
                OtherTrader = assignment.OtherTraderSnookerMatchWinnerHDPOU,
                MaxBet = assignment.MaxBetSnookerMatchWinnerHDPOU,
                Spread = assignment.SpreadSnookerMatchWinnerHDPOU
            },
            new()
            {
                Bettype = Bettype.MatchWinnerOU,
                MaxBet = assignment.MaxBetSnookerMatchWinnerOU,
                Spread = assignment.SpreadSnookerMatchWinnerOU
            },
            new()
            {
                Bettype = Bettype.FramesWinnerHDPOUOE,
                Trader = assignment.TraderSnookerFramesWinnerHDPOUOE,
                OtherTrader = assignment.OtherTraderSnookerFramesWinnerHDPOUOE,
                MaxBet = assignment.MaxBetSnookerFramesWinnerHDPOUOE,
                Spread = assignment.SpreadSnookerFramesWinnerHDPOUOE
            },
            new()
            {
                Bettype = Bettype.FramesWinnerOU,
                MaxBet = assignment.MaxBetSnookerFramesWinnerOU,
                Spread = assignment.SpreadSnookerFramesWinnerOU
            },
            new()
            {
                Bettype = Bettype.FramesWinnerOE,
                MaxBet = assignment.MaxBetSnookerFramesWinnerOE,
                Spread = assignment.SpreadSnookerFramesWinnerOE
            },
            new()
            {
                Bettype = Bettype.First5FramesHDP,
                Trader = assignment.TraderSnookerFirst5FramesHDP,
                OtherTrader = assignment.OtherTraderSnookerFirst5FramesHDP,
                MaxBet = assignment.MaxBetSnookerFirst5FramesHDP,
                Spread = assignment.SpreadSnookerFirst5FramesHDP
            },
            new()
            {
                Bettype = Bettype.First8FramesHDP,
                Trader = assignment.TraderSnookerFirst8FramesHDP,
                OtherTrader = assignment.OtherTraderSnookerFirst8FramesHDP,
                MaxBet = assignment.MaxBetSnookerFirst8FramesHDP,
                Spread = assignment.SpreadSnookerFirst8FramesHDP
            },
            new()
            {
                Bettype = Bettype.First9FramesHDP,
                Trader = assignment.TraderSnookerFirst9FramesHDP,
                OtherTrader = assignment.OtherTraderSnookerFirst9FramesHDP,
                MaxBet = assignment.MaxBetSnookerFirst9FramesHDP,
                Spread = assignment.SpreadSnookerFirst9FramesHDP
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapSnooker(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Snooker_in_AssignmentModel_not_Contain_Snooker()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.OtherSports);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapSnooker(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapBaseball
    [Fact]
    public void Get_Baseball_in_AssignmentModel_Contain_Baseball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Baseball);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.FTRunLineHDP,
                Trader = assignment.TraderBaseballFTRunLineHDP,
                OtherTrader = assignment.OtherTraderBaseballFTRunLineHDP,
                MaxBet = assignment.MaxBetBaseballFTRunLineHDP,
                Spread = assignment.SpreadBaseballFTRunLineHDP
            },
            new()
            {
                Bettype = Bettype.FirstH1st5inningsHDP,
                Trader = assignment.TraderBaseball1H1st5inningsHDP,
                OtherTrader = assignment.OtherTraderBaseball1H1st5inningsHDP,
                MaxBet = assignment.MaxBetBaseball1H1st5inningsHDP,
                Spread = assignment.SpreadBaseball1H1st5inningsHDP
            },
            new()
            {
                Bettype = Bettype.FTTotalRunOU,
                Trader = assignment.TraderBaseballFTTotalRunOU,
                OtherTrader = assignment.OtherTraderBaseballFTTotalRunOU,
                MaxBet = assignment.MaxBetBaseballFTTotalRunOU,
                Spread = assignment.SpreadBaseballFTTotalRunOU
            },
            new()
            {
                Bettype = Bettype.FirstH1st5inningsOU,
                Trader = assignment.TraderBaseball1H1st5inningsOU,
                OtherTrader = assignment.OtherTraderBaseball1H1st5inningsOU,
                MaxBet = assignment.MaxBetBaseball1H1st5inningsOU,
                Spread = assignment.SpreadBaseball1H1st5inningsOU
            },
            new()
            {
                Bettype = Bettype.InningHitsAndRuns,
                Trader = assignment.TraderBaseballInningHitsRuns,
                OtherTrader = assignment.OtherTraderBaseballInningHitsRuns,
                MaxBet = assignment.MaxBetBaseballInningHitsRuns,
                Spread = assignment.SpreadBaseballInningHitsRuns
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBaseball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Baseball_in_AssignmentModel_not_Contain_Baseball()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.OtherSports);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBaseball(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapBadminton
    [Fact]
    public void Get_Badminton_in_AssignmentModel_Contain_Badminton()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Badminton);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.AllProductSetWinner,
                Trader = assignment.TraderBadmintonAllProductSetWinner,
                OtherTrader = assignment.OtherTraderBadmintonAllProductSetWinner,
                Spread = 10
            },
            new()
            {
                Bettype = Bettype.AllProductPointWinner,
                Trader = assignment.TraderBadmintonAllProductPointWinner,
                OtherTrader = assignment.OtherTraderBadmintonAllProductPointWinner,
                Spread = 10
            },
            new()
            {
                Bettype = Bettype.RacetoPoints7911,
                Trader = assignment.TraderBadmintonRaceToPoints7911,
                OtherTrader = assignment.OtherTraderBadmintonRaceToPoints7911,
                Spread = 10
            },
            new()
            {
                Bettype = Bettype.NextPointWinner,
                Trader = assignment.TraderBadmintonNextPointWinner,
                OtherTrader = assignment.OtherTraderBadmintonNextPointWinner,
                Spread = 10
            },
            new()
            {
                Bettype = Bettype.MatchHandicap,
                Trader = assignment.TraderBadmintonMatchHandicap,
                OtherTrader = assignment.OtherTraderBadmintonMatchHandicap,
                Spread = 10
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBadminton(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Badminton_in_AssignmentModel_not_Contain_Badminton()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.OtherSports);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapBadminton(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    //MapTennis
    [Fact]
    public void Get_Tennis_in_AssignmentModel_Contain_Tennis()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.Tennis);

        var expect = new List<AssignmentMaxbet> {
            new()
            {
                Bettype = Bettype.AllProductLS,
                Trader = assignment.TraderTennisAllProductLS,
                OtherTrader = assignment.OtherTraderTennisAllProductLS,
                Spread = assignment.SpreadTennisAllProductLS
            },
            new()
            {
                Bettype = Bettype.NextGame,
                Trader = assignment.TraderTennisNextGame,
                OtherTrader = assignment.OtherTraderTennisNextGame,
                Spread = assignment.SpreadTennisNextGame
            },
            new()
            {
                Bettype = Bettype.MostAces,
                Trader = assignment.TraderTennisMostAces,
                OtherTrader = assignment.OtherTraderTennisMostAces,
                Spread = assignment.SpreadTennisMostAces
            },
            new()
            {
                Bettype = Bettype.MostDoubleFaults,
                Trader = assignment.TraderTennisMostDoubleFaults,
                OtherTrader = assignment.OtherTraderTennisMostDoubleFaults,
                Spread = assignment.SpreadTennisMostDoubleFaults
            }};

        //Act
        var actual = AssignmentMaxbetMapperImp.MapTennis(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    [Fact]
    public void Get_Tennis_in_AssignmentModel_not_Contain_Tennis()
    {
        //Arrange
        var assignment = AssignmentModelMock(Sport.OtherSports);

        var expect = new List<AssignmentMaxbet>();

        //Act
        var actual = AssignmentMaxbetMapperImp.MapTennis(assignment);
        //Assert
        DeepAssert.Equal(expect, actual);
    }

    private static AssignmentModel AssignmentModelMock(string sport)
    {
        return new AssignmentModel
        {
            Sport = sport,
            MatchStatus = "MatchStatus",
            Home = "Home",
            Away = "Away",
            League = "League",
            AutoLink = "AutoLink",
            TraderBasketballFTHDP = "TraderBasketballFTHDP",
            TraderBasketball1Q3QHDP = "TraderBasketball1Q3QHDP",
            TraderBasketball1HHDP = "TraderBasketball1HHDP",
            TraderBasketballFTOUOE = "TraderBasketballFTOUOE",
            TraderBasketball1HOUOE = "TraderBasketball1HOUOE",
            TraderBasketball1Q3QOUOE = "TraderBasketball1Q3QOUOE",
            TraderFootballFTHDP = "TraderFootballFTHDP",
            TraderFootball1HHDP = "TraderFootball1HHDP",
            TraderFootball1Q3QHDP = "TraderFootball1Q3QHDP",
            TraderFootballFTOU = "TraderFootballFTOU",
            TraderFootball1HOU = "TraderFootball1HOU",
            TraderFootball1Q3QOU = "TraderFootball1Q3QOU",
            TraderFieldHockeyFTHDP = "TraderFieldHockeyFTHDP",
            TraderFieldHockey1HHDP = "TraderFieldHockey1HHDP",
            TraderFieldHockey1QHDP = "TraderFieldHockey1QHDP",
            TraderFieldHockeyFTOU = "TraderFieldHockeyFTOU",
            TraderFieldHockey1HOU = "TraderFieldHockey1HOU",
            TraderFieldHockey1QOU = "TraderFieldHockey1QOU",
            TraderIceHockeyFTHDP = "TraderIceHockeyFTHDP",
            TraderIceHockey1st2ndPeriodsHDP = "TraderIceHockey1st2ndPeriodsHDP",
            TraderIceHockeyFTOU = "TraderIceHockeyFTOU",
            TraderIceHockey1st2ndPeriodsOU = "TraderIceHockey1st2ndPeriodsOU",
            TraderOtherSportsMatchWinner = "TraderOtherSportsMatchWinner",
            TraderOtherSportsFTHDP = "TraderOtherSportsFTHDP",
            TraderOtherSports1HHDP = "TraderOtherSports1HHDP",
            TraderOtherSportsFTOU = "TraderOtherSportsFTOU",
            TraderOtherSports1HOU = "TraderOtherSports1HOU",
            TraderDartsLegsHDP = "TraderDartsLegsHDP",
            TraderDartsMost180sHDP = "TraderDartsMost180sHDP",
            TraderPoolMatchWinnerHDP = "TraderPoolMatchWinnerHDP",
            TraderPoolRacksWinnerHDP = "TraderPoolRacksWinnerHDP",
            TraderTableTennisSetWinnerHDP = "TraderTableTennisSetWinnerHDP",
            TraderTableTennisSetXWinnerHDP = "TraderTableTennisSetXWinnerHDP",
            TraderVolleyballSetWinnerHDPOE = "TraderVolleyballSetWinnerHDPOE",
            TraderVolleyballSetsXPointWinnerHDP = "TraderVolleyballSetsXPointWinnerHDP",
            TraderHandballFTHDP = "TraderHandballFTHDP",
            TraderHandball1HHDP = "TraderHandball1HHDP",
            TraderHandballFTOU = "TraderHandballFTOU",
            TraderHandball1HOU = "TraderHandball1HOU",
            TraderNetballFTHDP = "TraderNetballFTHDP",
            TraderNetball1HHDP = "TraderNetball1HHDP",
            TraderNetball1Q3QHDP = "TraderNetball1Q3QHDP",
            TraderNetballFTOUOE = "TraderNetballFTOUOE",
            TraderNetball1HOUOE = "TraderNetball1HOUOE",
            TraderNetball1Q3QOUOE = "TraderNetball1Q3QOUOE",
            TraderCricketMatchWinnerInningsOU = "TraderCricketMatchWinnerInningsOU",
            TraderCricketNextOverOUPlayerOU = "TraderCricketNextOverOUPlayerOU",
            TraderCricketFT1X2 = "TraderCricketFT1X2",
            TraderSnookerMatchWinnerHDPOU = "TraderSnookerMatchWinnerHDPOU",
            TraderSnookerFramesWinnerHDPOUOE = "TraderSnookerFramesWinnerHDPOUOE",
            TraderSnookerFirst5FramesHDP = "TraderSnookerFirst5FramesHDP",
            TraderSnookerFirst8FramesHDP = "TraderSnookerFirst8FramesHDP",
            TraderSnookerFirst9FramesHDP = "TraderSnookerFirst9FramesHDP",
            TraderBaseballFTRunLineHDP = "TraderBaseballFTRunLineHDP",
            TraderBaseball1H1st5inningsHDP = "TraderBaseball1H1st5inningsHDP",
            TraderBaseballFTTotalRunOU = "TraderBaseballFTTotalRunOU",
            TraderBaseball1H1st5inningsOU = "TraderBaseball1H1st5inningsOU",
            TraderBaseballInningHitsRuns = "TraderBaseballInningHitsRuns",
            TraderBadmintonAllProductSetWinner = "TraderBadmintonAllProductSetWinner",
            TraderBadmintonAllProductPointWinner = "TraderBadmintonAllProductPointWinner",
            TraderBadmintonRaceToPoints7911 = "TraderBadmintonRaceToPoints7911",
            TraderBadmintonNextPointWinner = "TraderBadmintonNextPointWinner",
            TraderBadmintonMatchHandicap = "TraderBadmintonMatchHandicap",
            TraderTennisAllProductLS = "TraderTennisAllProductLS",
            TraderTennisNextGame = "TraderTennisNextGame",
            TraderTennisMostAces = "TraderTennisMostAces",
            TraderTennisMostDoubleFaults = "TraderTennisMostDoubleFaults",
            OtherTraderBadmintonAllProductPointWinner = "OtherTraderBadmintonAllProductPointWinner",
            OtherTraderBadmintonAllProductSetWinner = "OtherTraderBadmintonAllProductSetWinner",
            OtherTraderBadmintonMatchHandicap = "OtherTraderBadmintonMatchHandicap",
            OtherTraderBadmintonNextPointWinner = "OtherTraderBadmintonNextPointWinner",
            OtherTraderBadmintonRaceToPoints7911 = "OtherTraderBadmintonRaceToPoints7911",
            OtherTraderBaseball1H1st5inningsHDP = "OtherTraderBaseball1H1st5inningsHDP",
            OtherTraderBaseball1H1st5inningsOU = "OtherTraderBaseball1H1st5inningsOU",
            OtherTraderBaseballFTRunLineHDP = "OtherTraderBaseballFTRunLineHDP",
            OtherTraderBaseballFTTotalRunOU = "OtherTraderBaseballFTTotalRunOU",
            OtherTraderBaseballInningHitsRuns = "OtherTraderBaseballInningHitsRuns",
            OtherTraderBasketball1HHDP = "OtherTraderBasketball1HHDP",
            OtherTraderBasketball1HOUOE = "OtherTraderBasketball1HOUOE",
            OtherTraderBasketball1Q3QHDP = "OtherTraderBasketball1Q3QHDP",
            OtherTraderBasketball1Q3QOUOE = "OtherTraderBasketball1Q3QOUOE",
            OtherTraderBasketballFTHDP = "OtherTraderBasketballFTHDP",
            OtherTraderBasketballFTOUOE = "OtherTraderBasketballFTOUOE",
            OtherTraderCricketFT1X2 = "OtherTraderCricketFT1X2",
            OtherTraderCricketMatchWinnerInningsOU = "OtherTraderCricketMatchWinnerInningsOU",
            OtherTraderCricketNextOverOUPlayerOU = "OtherTraderCricketNextOverOUPlayerOU",
            OtherTraderDartsLegsHDP = "OtherTraderDartsLegsHDP",
            OtherTraderDartsMost180sHDP = "OtherTraderDartsMost180sHDP",
            OtherTraderFieldHockey1HHDP = "OtherTraderFieldHockey1HHDP",
            OtherTraderFieldHockey1HOU = "OtherTraderFieldHockey1HOU",
            OtherTraderFieldHockey1QHDP = "OtherTraderFieldHockey1QHDP",
            OtherTraderFieldHockey1QOU = "OtherTraderFieldHockey1QOU",
            OtherTraderFieldHockeyFTHDP = "OtherTraderFieldHockeyFTHDP",
            OtherTraderFieldHockeyFTOU = "OtherTraderFieldHockeyFTOU",
            OtherTraderFootball1HHDP = "OtherTraderFootball1HHDP",
            OtherTraderFootball1HOU = "OtherTraderFootball1HOU",
            OtherTraderFootball1Q3QHDP = "OtherTraderFootball1Q3QHDP",
            OtherTraderFootball1Q3QOU = "OtherTraderFootball1Q3QOU",
            OtherTraderFootballFTHDP = "OtherTraderFootballFTHDP",
            OtherTraderFootballFTOU = "OtherTraderFootballFTOU",
            OtherTraderHandball1HHDP = "OtherTraderHandball1HHDP",
            OtherTraderHandball1HOU = "OtherTraderHandball1HOU",
            OtherTraderHandballFTHDP = "OtherTraderHandballFTHDP",
            OtherTraderHandballFTOU = "OtherTraderHandballFTOU",
            OtherTraderIceHockey1st2ndPeriodsHDP = "OtherTraderIceHockey1st2ndPeriodsHDP",
            OtherTraderIceHockey1st2ndPeriodsOU = "OtherTraderIceHockey1st2ndPeriodsOU",
            OtherTraderIceHockeyFTHDP = "OtherTraderIceHockeyFTHDP",
            OtherTraderIceHockeyFTOU = "OtherTraderIceHockeyFTOU",
            OtherTraderNetball1HHDP = "OtherTraderNetball1HHDP",
            OtherTraderNetball1HOUOE = "OtherTraderNetball1HOUOE",
            OtherTraderNetball1Q3QHDP = "OtherTraderNetball1Q3QHDP",
            OtherTraderNetball1Q3QOUOE = "OtherTraderNetball1Q3QOUOE",
            OtherTraderNetballFTHDP = "OtherTraderNetballFTHDP",
            OtherTraderNetballFTOUOE = "OtherTraderNetballFTOUOE",
            OtherTraderOtherSportsMatchWinner = "OtherTraderOtherSportsMatchWinner",
            OtherTraderOtherSportsFTHDP = "OtherTraderOtherSportsFTHDP",
            OtherTraderOtherSports1HHDP = "OtherTraderOtherSports1HHDP",
            OtherTraderOtherSportsFTOU = "OtherTraderOtherSportsFTOU",
            OtherTraderOtherSports1HOU = "OtherTraderOtherSports1HOU",
            OtherTraderPoolMatchWinnerHDP = "OtherTraderPoolMatchWinnerHDP",
            OtherTraderPoolRacksWinnerHDP = "OtherTraderPoolRacksWinnerHDP",
            OtherTraderSnookerFirst5FramesHDP = "OtherTraderSnookerFirst5FramesHDP",
            OtherTraderSnookerFirst8FramesHDP = "OtherTraderSnookerFirst8FramesHDP",
            OtherTraderSnookerFirst9FramesHDP = "OtherTraderSnookerFirst9FramesHDP",
            OtherTraderSnookerFramesWinnerHDPOUOE = "OtherTraderSnookerFramesWinnerHDPOUOE",
            OtherTraderSnookerMatchWinnerHDPOU = "OtherTraderSnookerMatchWinnerHDPOU",
            OtherTraderTableTennisSetWinnerHDP = "OtherTraderTableTennisSetWinnerHDP",
            OtherTraderTableTennisSetXWinnerHDP = "OtherTraderTableTennisSetXWinnerHDP",
            OtherTraderTennisAllProductLS = "OtherTraderTennisAllProductLS",
            OtherTraderTennisMostAces = "OtherTraderTennisMostAces",
            OtherTraderTennisMostDoubleFaults = "OtherTraderTennisMostDoubleFaults",
            OtherTraderTennisNextGame = "OtherTraderTennisNextGame",
            OtherTraderVolleyballSetsXPointWinnerHDP = "OtherTraderVolleyballSetsXPointWinnerHDP",
            OtherTraderVolleyballSetWinnerHDPOE = "OtherTraderVolleyballSetWinnerHDPOU",
            MaxBetBasketballFTHDP = 1200,
            MaxBetBasketballFTOUOE = 1200,
            MaxBetBasketballFTOE = 13000,
            MaxBetFootballFTHDP = 1200,
            MaxBetFootball1HHDP = 1200,
            MaxBetFootball1Q3QHDP = 1200,
            MaxBetFootballFTOU = 1200,
            MaxBetFootball1HOU = 1200,
            MaxBetFootball1Q3QOU = 1200,
            MaxBetFieldHockeyFTHDP = 1200,
            MaxBetFieldHockey1HHDP = 1200,
            MaxBetFieldHockey1QHDP = 1200,
            MaxBetFieldHockeyFTOU = 1200,
            MaxBetFieldHockey1HOU = 1200,
            MaxBetFieldHockey1QOU = 1200,
            MaxBetIceHockeyFTHDP = 1200,
            MaxBetIceHockey1st2ndPeriodsHDP = 1200,
            MaxBetIceHockeyFTOU = 1200,
            MaxBetIceHockey1st2ndPeriodsOU = 1200,
            MaxBetOtherSportsMatchWinner = 1200,
            MaxBetOtherSportsFTHDP = 1200,
            MaxBetOtherSports1HHDP = 1200,
            MaxBetOtherSportsFTOU = 1200,
            MaxBetOtherSports1HOU = 1200,
            MaxBetDartsLegsHDP = 1200,
            MaxBetDartsMost180sHDP = 1200,
            MaxBetPoolMatchWinnerHDP = 1200,
            MaxBetPoolRacksWinnerHDP = 1200,
            MaxBetTableTennisSetWinnerHDP = 1200,
            MaxBetTableTennisSetXWinnerHDP = 1200,
            MaxBetVolleyballSetWinnerHDPOE = 1200,
            MaxBetVolleyballSetWinnerOE = 1300,
            MaxBetVolleyballSetsXPointWinnerHDP = 1200,
            MaxBetHandballFTHDP = 1200,
            MaxBetHandball1HHDP = 1200,
            MaxBetHandballFTOU = 1200,
            MaxBetHandball1HOU = 1200,
            MaxBetNetballFTHDP = 1200,
            MaxBetNetball1HHDP = 1200,
            MaxBetNetball1Q3QHDP = 1200,
            MaxBetNetballFTOUOE = 1200,
            MaxBetNetballFTOE = 1300,
            MaxBetNetball1HOUOE = 1200,
            MaxBetNetball1HOE = 1300,
            MaxBetNetball1Q3QOUOE = 1200,
            MaxBetNetball1Q3QOE = 1300,
            MaxBetCricketMatchWinnerInningsOU = 1200,
            MaxBetCricketNextOverOUPlayerOU = 1200,
            MaxBetCricketFT1X2 = 1200,
            MaxBetSnookerMatchWinnerHDPOU = 1200,
            MaxBetSnookerMatchWinnerOU = 1300,
            MaxBetSnookerFramesWinnerHDPOUOE = 1200,
            MaxBetSnookerFramesWinnerOU = 1300,
            MaxBetSnookerFramesWinnerOE = 1300,
            MaxBetSnookerFirst5FramesHDP = 1200,
            MaxBetSnookerFirst8FramesHDP = 1200,
            MaxBetSnookerFirst9FramesHDP = 1200,
            MaxBetBaseballFTRunLineHDP = 1200,
            MaxBetBaseball1H1st5inningsHDP = 1200,
            MaxBetBaseballFTTotalRunOU = 1200,
            MaxBetBaseball1H1st5inningsOU = 1200,
            MaxBetBaseballInningHitsRuns = 1200,
            MaxBetBadmintonAllProductSetWinner = 1200,
            MaxBetBadmintonAllProductPointWinner = 1200,
            MaxBetBadmintonRaceToPoints7911 = 1200,
            MaxBetBadmintonNextPointWinner = 1200,
            MaxBetBadmintonMatchHandicap = 1200,
            MaxBetTennisAllProductLS = 1200,
            MaxBetTennisNextGame = 1200,
            MaxBetTennisMostAces = 1200,
            MaxBetTennisMostDoubleFaults = 1200,
            SpreadBasketballFTHDP = 10,
            SpreadBasketballFTOUOE = 10,
            SpreadBasketballFTOE = 10,
            SpreadFootballFTHDP = 10,
            SpreadFootball1HHDP = 10,
            SpreadFootball1Q3QHDP = 10,
            SpreadFootballFTOU = 10,
            SpreadFootball1HOU = 10,
            SpreadFootball1Q3QOU = 10,
            SpreadFieldHockeyFTHDP = 10,
            SpreadFieldHockey1HHDP = 10,
            SpreadFieldHockey1QHDP = 10,
            SpreadFieldHockeyFTOU = 10,
            SpreadFieldHockey1HOU = 10,
            SpreadFieldHockey1QOU = 10,
            SpreadIceHockeyFTHDP = 10,
            SpreadIceHockey1st2ndPeriodsHDP = 10,
            SpreadIceHockeyFTOU = 10,
            SpreadIceHockey1st2ndPeriodsOU = 10,
            SpreadOtherSportsMatchWinner = 10,
            SpreadOtherSportsFTHDP = 10,
            SpreadOtherSports1HHDP = 10,
            SpreadOtherSportsFTOU = 10,
            SpreadOtherSports1HOU = 10,
            SpreadDartsLegsHDP = 10,
            SpreadDartsMost180sHDP = 10,
            SpreadPoolMatchWinnerHDP = 10,
            SpreadPoolRacksWinnerHDP = 10,
            SpreadTableTennisSetWinnerHDP = 10,
            SpreadTableTennisSetXWinnerHDP = 10,
            SpreadVolleyballSetWinnerHDPOE = 10,
            SpreadVolleyballSetWinnerOE = 10,
            SpreadVolleyballSetsXPointWinnerHDP = 10,
            SpreadHandballFTHDP = 10,
            SpreadHandball1HHDP = 10,
            SpreadHandballFTOU = 10,
            SpreadHandball1HOU = 10,
            SpreadNetballFTHDP = 10,
            SpreadNetball1HHDP = 10,
            SpreadNetball1Q3QHDP = 10,
            SpreadNetballFTOUOE = 10,
            SpreadNetballFTOE = 10,
            SpreadNetball1HOUOE = 10,
            SpreadNetball1HOE = 10,
            SpreadNetball1Q3QOUOE = 10,
            SpreadNetball1Q3QOE = 10,
            SpreadCricketMatchWinnerInningsOU = 10,
            SpreadCricketNextOverOUPlayerOU = 10,
            SpreadCricketFT1X2 = 10,
            SpreadSnookerMatchWinnerHDPOU = 10,
            SpreadSnookerMatchWinnerOU = 10,
            SpreadSnookerFramesWinnerHDPOUOE = 10,
            SpreadSnookerFramesWinnerOU = 10,
            SpreadSnookerFramesWinnerOE = 10,
            SpreadSnookerFirst5FramesHDP = 10,
            SpreadSnookerFirst8FramesHDP = 10,
            SpreadSnookerFirst9FramesHDP = 10,
            SpreadBaseballFTRunLineHDP = 10,
            SpreadBaseball1H1st5inningsHDP = 10,
            SpreadBaseballFTTotalRunOU = 10,
            SpreadBaseball1H1st5inningsOU = 10,
            SpreadBaseballInningHitsRuns = 10,
            SpreadBadmintonAllProductSetWinner = 10,
            SpreadBadmintonAllProductPointWinner = 10,
            SpreadBadmintonRaceToPoints7911 = 10,
            SpreadBadmintonNextPointWinner = 10,
            SpreadBadmintonMatchHandicap = 10,
            SpreadTennisAllProductLS = 10,
            SpreadTennisNextGame = 10,
            SpreadTennisMostAces = 10,
            SpreadTennisMostDoubleFaults = 10
        };
    }
}