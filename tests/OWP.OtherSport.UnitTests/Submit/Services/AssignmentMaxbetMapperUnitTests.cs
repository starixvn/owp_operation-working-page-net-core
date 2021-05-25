namespace OWP.OtherSport.UnitTests.Submit.Services
{
    using System.Collections.Generic;
    using OWP.OtherSport.Submit.Models;
    using Xunit.Asserts.Compare;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;
    using Xunit;

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
                new AssignmentMaxbet()
                {
                    Bettype = "Match Winner",
                    Trader = "TraderOtherSportsMatchWinner",
                    OtherTrader = "OtherTraderOtherSportsMatchWinner",
                    MaxBet = 1200
                }
            };

            var expect = new Dictionary<int, List<AssignmentMaxbet>>();
            expect.Add(0, assignmentMaxbets);

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
                new() {Bettype = "FT (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderFootballFTHDP", Trader = "TraderFootballFTHDP"},
                new() {Bettype = "1H (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1HHDP", Trader = "TraderFootball1HHDP" },
                new() {Bettype = "1Q & 3Q (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1Q3QHDP", Trader = "TraderFootball1Q3QHDP"},
                new() {Bettype = "FT (OU)", MaxBet = 1200, OtherTrader = "OtherTraderFootballFTOU", Trader = "TraderFootballFTOU"},
                new() {Bettype = "1H (OU)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1HOU", Trader = "TraderFootball1HOU"},
                new() {Bettype = "1Q & 3Q (OU)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1Q3QOU", Trader = "TraderFootball1Q3QOU"}
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
                new() {Bettype = "FT (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderBasketballFTHDP", Trader = "TraderBasketballFTHDP"},
                new() {Bettype = "1H (HDP)", MaxBet = null, OtherTrader = "OtherTraderBasketball1HHDP", Trader = "TraderBasketball1HHDP"},
                new() {Bettype = "1Q & 3Q (HDP)", MaxBet = null, OtherTrader = "OtherTraderBasketball1Q3QHDP", Trader = "TraderBasketball1Q3QHDP"},
                new() {Bettype = "FT (OU/OE)", MaxBet = 1200, OtherTrader = "OtherTraderBasketballFTOUOE", Trader = "TraderBasketballFTOUOE"},
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

            var expect = new List<AssignmentMaxbet> { };

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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetFootballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    Trader = assignment.TraderFootball1HHDP,
                    OtherTrader = assignment.OtherTraderFootball1HHDP,
                    MaxBet = assignment.MaxBetFootball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    Trader = assignment.TraderFootball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderFootball1Q3QHDP,
                    MaxBet = assignment.MaxBetFootball1Q3QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOU,
                    Trader = assignment.TraderFootballFTOU,
                    OtherTrader = assignment.OtherTraderFootballFTOU,
                    MaxBet = assignment.MaxBetFootballFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOU,
                    Trader = assignment.TraderFootball1HOU,
                    OtherTrader = assignment.OtherTraderFootball1HOU,
                    MaxBet = assignment.MaxBetFootball1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOU,
                    Trader = assignment.TraderFootball1Q3QOU,
                    OtherTrader = assignment.OtherTraderFootball1Q3QOU,
                    MaxBet = assignment.MaxBetFootball1Q3QOU
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
                    MaxBet = assignment.MaxBetFieldHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    Trader = assignment.TraderFieldHockey1HHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1HHDP,
                    MaxBet = assignment.MaxBetFieldHockey1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQHDP,
                    Trader = assignment.TraderFieldHockey1QHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1QHDP,
                    MaxBet = assignment.MaxBetFieldHockey1QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOsU,
                    Trader = assignment.TraderFieldHockeyFTOU,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTOU,
                    MaxBet = assignment.MaxBetFieldHockeyFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOsU,
                    Trader = assignment.TraderFieldHockey1HOU,
                    OtherTrader = assignment.OtherTraderFieldHockey1HOU,
                    MaxBet = assignment.MaxBetFieldHockey1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstQOU,
                    Trader = assignment.TraderFieldHockey1QOU,
                    OtherTrader = assignment.OtherTraderFieldHockey1QOU,
                    MaxBet = assignment.MaxBetFieldHockey1QOU
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetIceHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.First2ndPeriodsHDP,
                    Trader = assignment.TraderIceHockey1st2ndPeriodsHDP,
                    OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsHDP,
                    MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsHDP,
                },
                new()
                {
                    Bettype = Bettype.FTOsU,
                    Trader = assignment.TraderIceHockeyFTOU,
                    OtherTrader = assignment.OtherTraderIceHockeyFTOU,
                    MaxBet = assignment.MaxBetIceHockeyFTOU
                },
                new()
                {
                    Bettype = Bettype.First2ndPeriodsOU,
                    Trader = assignment.TraderIceHockey1st2ndPeriodsOU,
                    OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsOU,
                    MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsOU
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetOtherSportsMatchWinner
                }};

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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetDartsLegsHDP
                },
                new()
                {
                    Bettype = Bettype.Most180sHDP,
                    Trader = assignment.TraderDartsMost180sHDP,
                    OtherTrader = assignment.OtherTraderDartsMost180sHDP,
                    MaxBet = assignment.MaxBetDartsMost180sHDP
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetPoolMatchWinnerHDP
                },
                new()
                {
                    Bettype = Bettype.RacksWinnerHDP,
                    Trader = assignment.TraderPoolRacksWinnerHDP,
                    OtherTrader = assignment.OtherTraderPoolRacksWinnerHDP,
                    MaxBet = assignment.MaxBetPoolRacksWinnerHDP
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetTableTennisSetWinnerHDP
                },
                new()
                {
                    Bettype = Bettype.SetXWinnerHDP,
                    Trader = assignment.TraderTableTennisSetXWinnerHDP,
                    OtherTrader = assignment.OtherTraderTableTennisSetXWinnerHDP,
                    MaxBet = assignment.MaxBetTableTennisSetXWinnerHDP
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetVolleyballSetWinnerHDPOE
                },
                new()
                {
                    Bettype = Bettype.SetsXPointWinnerHDP,
                    Trader = assignment.TraderVolleyballSetsXPointWinnerHDP,
                    OtherTrader = assignment.OtherTraderVolleyballSetsXPointWinnerHDP,
                    MaxBet = assignment.MaxBetVolleyballSetsXPointWinnerHDP
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetHandballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    Trader = assignment.TraderHandball1HHDP,
                    OtherTrader = assignment.OtherTraderHandball1HHDP,
                    MaxBet = assignment.MaxBetHandball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FTOsU,
                    Trader = assignment.TraderHandballFTOU,
                    OtherTrader = assignment.OtherTraderHandballFTOU,
                    MaxBet = assignment.MaxBetHandballFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOsU,
                    Trader = assignment.TraderHandball1HOU,
                    OtherTrader = assignment.OtherTraderHandball1HOU,
                    MaxBet = assignment.MaxBetHandball1HOU
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetNetballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    Trader = assignment.TraderNetball1HHDP,
                    OtherTrader = assignment.OtherTraderNetball1HHDP,
                    MaxBet = assignment.MaxBetNetball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    Trader = assignment.TraderNetball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderNetball1Q3QHDP,
                    MaxBet = assignment.MaxBetNetball1Q3QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOUOE,
                    Trader = assignment.TraderNetballFTOUOE,
                    OtherTrader = assignment.OtherTraderNetballFTOUOE,
                    MaxBet = assignment.MaxBetNetballFTOUOE
                },
                new()
                {
                    Bettype = Bettype.FirstHOUOE,
                    Trader = assignment.TraderNetball1HOUOE,
                    OtherTrader = assignment.OtherTraderNetball1HOUOE,
                    MaxBet = assignment.MaxBetNetball1HOUOE
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    Trader = assignment.TraderNetball1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderNetball1Q3QOUOE,
                    MaxBet = assignment.MaxBetNetball1Q3QOUOE
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

            var expect = new List<AssignmentMaxbet> { };

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
                    OtherTrader = assignment.OtherTraderCricketMatchWinnerInningsOU
                },
                new()
                {
                    Bettype = Bettype.NextOverOUPlayerOU,
                    Trader = assignment.TraderCricketNextOverOUPlayerOU,
                    OtherTrader = assignment.OtherTraderCricketNextOverOUPlayerOU
                },
                new()
                {
                    Bettype = Bettype.FT1X2,
                    Trader = assignment.TraderCricketFT1X2,
                    OtherTrader = assignment.OtherTraderCricketFT1X2
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetSnookerMatchWinnerHDPOU
                },
                new()
                {
                    Bettype = Bettype.FramesWinnerHDPOUOE,
                    Trader = assignment.TraderSnookerFramesWinnerHDPOUOE,
                    OtherTrader = assignment.OtherTraderSnookerFramesWinnerHDPOUOE,
                    MaxBet = assignment.MaxBetSnookerFramesWinnerHDPOUOE
                },
                new()
                {
                    Bettype = Bettype.First5FramesHDP,
                    Trader = assignment.TraderSnookerFirst5FramesHDP,
                    OtherTrader = assignment.OtherTraderSnookerFirst5FramesHDP,
                    MaxBet = assignment.MaxBetSnookerFirst5FramesHDP
                },
                new()
                {
                    Bettype = Bettype.First8FramesHDP,
                    Trader = assignment.TraderSnookerFirst8FramesHDP,
                    OtherTrader = assignment.OtherTraderSnookerFirst8FramesHDP,
                    MaxBet = assignment.MaxBetSnookerFirst8FramesHDP
                },
                new()
                {
                    Bettype = Bettype.First9FramesHDP,
                    Trader = assignment.TraderSnookerFirst9FramesHDP,
                    OtherTrader = assignment.OtherTraderSnookerFirst9FramesHDP,
                    MaxBet = assignment.MaxBetSnookerFirst9FramesHDP
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

            var expect = new List<AssignmentMaxbet> { };

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
                    MaxBet = assignment.MaxBetBaseballFTRunLineHDP
                },
                new()
                {
                    Bettype = Bettype.FirstH1st5inningsHDP,
                    Trader = assignment.TraderBaseball1H1st5inningsHDP,
                    OtherTrader = assignment.OtherTraderBaseball1H1st5inningsHDP,
                    MaxBet = assignment.MaxBetBaseball1H1st5inningsHDP
                },
                new()
                {
                    Bettype = Bettype.FTTotalRunOU,
                    Trader = assignment.TraderBaseballFTTotalRunOU,
                    OtherTrader = assignment.OtherTraderBaseballFTTotalRunOU,
                    MaxBet = assignment.MaxBetBaseballFTTotalRunOU
                },
                new()
                {
                    Bettype = Bettype.FirstH1st5inningsOU,
                    Trader = assignment.TraderBaseball1H1st5inningsOU,
                    OtherTrader = assignment.OtherTraderBaseball1H1st5inningsOU,
                    MaxBet = assignment.MaxBetBaseball1H1st5inningsOU
                },
                new()
                {
                    Bettype = Bettype.InningHitsAndRuns,
                    Trader = assignment.TraderBaseballInningHitsRuns,
                    OtherTrader = assignment.OtherTraderBaseballInningHitsRuns,
                    MaxBet = assignment.MaxBetBaseballInningHitsRuns
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

            var expect = new List<AssignmentMaxbet> { };

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
                    OtherTrader = assignment.OtherTraderBadmintonAllProductSetWinner
                },
                new()
                {
                    Bettype = Bettype.AllProductPointWinner,
                    Trader = assignment.TraderBadmintonAllProductPointWinner,
                    OtherTrader = assignment.OtherTraderBadmintonAllProductPointWinner
                },
                new()
                {
                    Bettype = Bettype.RacetoPoints7911,
                    Trader = assignment.TraderBadmintonRacetoPoints7911,
                    OtherTrader = assignment.OtherTraderBadmintonRacetoPoints7911
                },
                new()
                {
                    Bettype = Bettype.NextPointWinner,
                    Trader = assignment.TraderBadmintonNextPointWinner,
                    OtherTrader = assignment.OtherTraderBadmintonNextPointWinner
                },
                new()
                {
                    Bettype = Bettype.MatchHandicap,
                    Trader = assignment.TraderBadmintonMatchHandicap,
                    OtherTrader = assignment.OtherTraderBadmintonMatchHandicap
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

            var expect = new List<AssignmentMaxbet> { };

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
                    OtherTrader = assignment.OtherTraderTennisAllProductLS
                },
                new()
                {
                    Bettype = Bettype.NextGame,
                    Trader = assignment.TraderTennisNextGame,
                    OtherTrader = assignment.OtherTraderTennisNextGame
                },
                new()
                {
                    Bettype = Bettype.MostAces,
                    Trader = assignment.TraderTennisMostAces,
                    OtherTrader = assignment.OtherTraderTennisMostAces
                },
                new()
                {
                    Bettype = Bettype.MostDoubleFaults,
                    Trader = assignment.TraderTennisMostDoubleFaults,
                    OtherTrader = assignment.OtherTraderTennisMostDoubleFaults
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

            var expect = new List<AssignmentMaxbet> { };

            //Act
            var actual = AssignmentMaxbetMapperImp.MapTennis(assignment);
            //Assert
            DeepAssert.Equal(expect, actual);
        }

        private static AssignmentModel AssignmentModelMock(string sport)
        {
            return new()
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
                TraderBadmintonRacetoPoints7911 = "TraderBadmintonRacetoPoints7911",
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
                OtherTraderBadmintonRacetoPoints7911 = "OtherTraderBadmintonRacetoPoints7911",
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
                MaxBetDartsLegsHDP = 1200,
                MaxBetDartsMost180sHDP = 1200,
                MaxBetPoolMatchWinnerHDP = 1200,
                MaxBetPoolRacksWinnerHDP = 1200,
                MaxBetTableTennisSetWinnerHDP = 1200,
                MaxBetTableTennisSetXWinnerHDP = 1200,
                MaxBetVolleyballSetWinnerHDPOE = 1200,
                MaxBetVolleyballSetsXPointWinnerHDP = 1200,
                MaxBetHandballFTHDP = 1200,
                MaxBetHandball1HHDP = 1200,
                MaxBetHandballFTOU = 1200,
                MaxBetHandball1HOU = 1200,
                MaxBetNetballFTHDP = 1200,
                MaxBetNetball1HHDP = 1200,
                MaxBetNetball1Q3QHDP = 1200,
                MaxBetNetballFTOUOE = 1200,
                MaxBetNetball1HOUOE = 1200,
                MaxBetNetball1Q3QOUOE = 1200,
                MaxBetCricketMatchWinnerInningsOU = 1200,
                MaxBetCricketNextOverOUPlayerOU = 1200,
                MaxBetCricketFT1X2 = 1200,
                MaxBetSnookerMatchWinnerHDPOU = 1200,
                MaxBetSnookerFramesWinnerHDPOUOE = 1200,
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
                MaxBetBadmintonRacetoPoints7911 = 1200,
                MaxBetBadmintonNextPointWinner = 1200,
                MaxBetBadmintonMatchHandicap = 1200,
                MaxBetTennisAllProductLS = 1200,
                MaxBetTennisNextGame = 1200,
                MaxBetTennisMostAces = 1200,
                MaxBetTennisMostDoubleFaults = 1200
            };
        }
    }
}
