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
            var assignment = new List<AssignmentModel> ( );
            var expect = new Dictionary<int, List<AssignmentMaxbet>>();           
            //Act
            var actual = services.MapBySports(assignment);
           
            //Assert
            Assert.Equal(expect, actual);
        } 
        
        //MapBySport
        [Fact]
        public void MapBySport_Get_AssignmentMaxbet_in_AssignmentModel()
        {
            //Arrange
            var assignment = AssignmentModelMock(Sport.Football);

            var expect = new List<AssignmentMaxbet>
            {
                new() {Bettype = "FT (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderFootballFTHDP", TraderId = 7},
                new() {Bettype = "1H (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1HHDP", TraderId = 8},
                new() {Bettype = "1Q & 3Q (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1Q3QHDP", TraderId = 9},
                new() {Bettype = "FT (OU)", MaxBet = 1200, OtherTrader = "OtherTraderFootballFTOU", TraderId = 10},
                new() {Bettype = "1H (OU)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1HOU", TraderId = 11},
                new() {Bettype = "1Q & 3Q (OU)", MaxBet = 1200, OtherTrader = "OtherTraderFootball1Q3QOU", TraderId = 12}
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
                new() {Bettype = "FT (HDP)", MaxBet = 1200, OtherTrader = "OtherTraderBasketballFTHDP", TraderId = 1},
                new() {Bettype = "1H (HDP)", MaxBet = null, OtherTrader = "OtherTraderBasketball1HHDP", TraderId = 2},
                new() {Bettype = "1Q & 3Q (HDP)", MaxBet = null, OtherTrader = "OtherTraderBasketball1Q3QHDP", TraderId = 3},
                new() {Bettype = "FT (OU/OE)", MaxBet = 1200, OtherTrader = "OtherTraderBasketballFTOUOE", TraderId = 4},
                new() {Bettype = "1H (OU/OE)", MaxBet = null, OtherTrader = "OtherTraderBasketball1HOUOE", TraderId = 5},
                new() {Bettype = "1Q & 3Q (OU/OE)", MaxBet = null, OtherTrader = "OtherTraderBasketball1Q3QOUOE", TraderId = 6}
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

            var expect = new List<AssignmentMaxbet>{ };

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

            var expect = new List<AssignmentMaxbet>{};

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
                    TraderId = assignment.TraderFootballFTHDP,
                    OtherTrader = assignment.OtherTraderFootballFTHDP,
                    MaxBet = assignment.MaxBetFootballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderFootball1HHDP,
                    OtherTrader = assignment.OtherTraderFootball1HHDP,
                    MaxBet = assignment.MaxBetFootball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderFootball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderFootball1Q3QHDP,
                    MaxBet = assignment.MaxBetFootball1Q3QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOU,
                    TraderId = assignment.TraderFootballFTOU,
                    OtherTrader = assignment.OtherTraderFootballFTOU,
                    MaxBet = assignment.MaxBetFootballFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOU,
                    TraderId = assignment.TraderFootball1HOU,
                    OtherTrader = assignment.OtherTraderFootball1HOU,
                    MaxBet = assignment.MaxBetFootball1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOU,
                    TraderId = assignment.TraderFootball1Q3QOU,
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
                    TraderId = assignment.TraderFieldHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetFieldHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderFieldHockey1HHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1HHDP,
                    MaxBet = assignment.MaxBetFieldHockey1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQHDP,
                    TraderId = assignment.TraderFieldHockey1QHDP,
                    OtherTrader = assignment.OtherTraderFieldHockey1QHDP,
                    MaxBet = assignment.MaxBetFieldHockey1QHDP
                },
                new()
                {
                    Bettype = Bettype.FTOU,
                    TraderId = assignment.TraderFieldHockeyFTOU,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTOU,
                    MaxBet = assignment.MaxBetFieldHockeyFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOsU,
                    TraderId = assignment.TraderFieldHockey1HOU,
                    OtherTrader = assignment.OtherTraderFieldHockey1HOU,
                    MaxBet = assignment.MaxBetFieldHockey1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstQOU,
                    TraderId = assignment.TraderFieldHockey1QOU,
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
                    TraderId = assignment.TraderFieldHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetFieldHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderIceHockeyFTHDP,
                    OtherTrader = assignment.OtherTraderFieldHockeyFTHDP,
                    MaxBet = assignment.MaxBetIceHockeyFTHDP
                },
                new()
                {
                    Bettype = Bettype.First2ndPeriodsHDP,
                    TraderId = assignment.TraderIceHockey1st2ndHDP,
                    OtherTrader = assignment.OtherTraderIceHockey1st2ndPeriodsHDP,
                    MaxBet = assignment.MaxBetIceHockey1st2ndPeriodsHDP,
                },
                new()
                {
                    Bettype = Bettype.FTOsU,
                    TraderId = assignment.TraderIceHockeyFTOU,
                    OtherTrader = assignment.OtherTraderIceHockeyFTOU,
                    MaxBet = assignment.MaxBetIceHockeyFTOU
                },
                new()
                {
                    Bettype = Bettype.First2ndPeriodsOU,
                    TraderId = assignment.TraderIceHockey1st2ndOU,
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
                    TraderId = assignment.TraderOtherSportsMatchWinner,
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
                    TraderId = assignment.TraderDartsLegsHDP,
                    OtherTrader = assignment.OtherTraderOtherSportsMatchWinner,
                    MaxBet = assignment.MaxBetDartsLegsHDP
                },
                new()
                {
                    Bettype = Bettype.Most180sHDP,
                    TraderId = assignment.TraderDartsMost180sHDP,
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
                    TraderId = assignment.TraderPoolMatchWinnerHDP,
                    OtherTrader = assignment.OtherTraderPoolMatchWinnerHDP,
                    MaxBet = assignment.MaxBetPoolMatchWinnerHDP
                },
                new()
                {
                    Bettype = Bettype.RacksWinnerHDP,
                    TraderId = assignment.TraderPoolRacksWinnerHDP,
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
                    TraderId = assignment.TraderTableTennisSetWinnerHDP,
                    OtherTrader = assignment.OtherTraderTableTennisSetWinnerHDP,
                    MaxBet = assignment.MaxBetTableTennisSetWinnerHDP
                },
                new()
                {
                    Bettype = Bettype.SetXWinnerHDP,
                    TraderId = assignment.TraderTableTennisSetXWinnerHDP,
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
                    TraderId = assignment.TraderVolleyballSetWinnerHDPOE,
                    OtherTrader = assignment.OtherTraderVolleyballSetWinnerHDPOU,
                    MaxBet = assignment.MaxBetVolleyballSetWinnerHDPOU
                },
                new()
                {
                    Bettype = Bettype.SetsXPointWinnerHDP,
                    TraderId = assignment.TraderVolleyballSetsXPointWinnerHDP,
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
                    TraderId = assignment.TraderHandballFTHDP,
                    OtherTrader = assignment.OtherTraderHandball1HHDP,
                    MaxBet = assignment.MaxBetHandballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderHandball1HHDP,
                    OtherTrader = assignment.OtherTraderHandball1HOU,
                    MaxBet = assignment.MaxBetHandball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FTOsU,
                    TraderId = assignment.TraderHandballFTOU,
                    OtherTrader = assignment.OtherTraderHandballFTHDP,
                    MaxBet = assignment.MaxBetHandballFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOsU,
                    TraderId = assignment.TraderHandball1HOU,
                    OtherTrader = assignment.OtherTraderHandballFTOU,
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
                    TraderId = assignment.TraderNetballFTHDP,
                    OtherTrader = assignment.OtherTraderNetballFTHDP,
                    MaxBet = assignment.MaxBetHandballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderNetball1HHDP,
                    OtherTrader = assignment.OtherTraderNetball1HHDP,
                    MaxBet = assignment.MaxBetHandball1HHDP
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderNetball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderHandballFTHDP,
                    MaxBet = assignment.MaxBetHandballFTOU
                },
                new()
                {
                    Bettype = Bettype.FTOUOE,
                    TraderId = assignment.TraderNetballFTOUOE,
                    OtherTrader = assignment.OtherTraderNetball1Q3QHDP,
                    MaxBet = assignment.MaxBetHandball1HOU
                },
                new()
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderNetball1HOUOE,
                    OtherTrader = assignment.OtherTraderNetball1HOUOE,
                    MaxBet = assignment.MaxBetHandballFTOU
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    TraderId = assignment.TraderNetball1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderNetball1Q3QOUOE,
                    MaxBet = assignment.MaxBetHandball1HOU
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
                    TraderId = assignment.TraderCricketMatchWinnerInningsOU,
                    OtherTrader = assignment.OtherTraderCricketMatchWinnerInningsOU,
                    MaxBet = assignment.MaxBetCricketMatchWinnerInningsOU
                },
                new()
                {
                    Bettype = Bettype.NextOverOUPlayerOU,
                    TraderId = assignment.TraderCricketNextOverOUPlayerOU,
                    OtherTrader = assignment.OtherTraderCricketNextOverOUPlayerOU,
                    MaxBet = assignment.MaxBetCricketNextOverOUPlayerOU
                },
                new()
                {
                    Bettype = Bettype.FT1X2,
                    TraderId = assignment.TraderCricketFT1X2,
                    OtherTrader = assignment.OtherTraderCricketFT1X2,
                    MaxBet = assignment.MaxBetCricketFT1X2
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
                    TraderId = assignment.TraderSnookerMatchWinnerHDPOU,
                    OtherTrader = assignment.OtherTraderSnookerMatchWinnerHDPOU,
                    MaxBet = assignment.MaxBetSnookerMatchWinnerHDPOU
                },
                new()
                {
                    Bettype = Bettype.FramesWinnerHDPOUOE,
                    TraderId = assignment.TraderSnookerFramesWinnerHDPOUOE,
                    OtherTrader = assignment.OtherTraderSnookerFramesWinnerHDPOUOE,
                    MaxBet = assignment.MaxBetSnookerFramesWinnerHDPOUOE
                },
                new()
                {
                    Bettype = Bettype.First5FramesHDP,
                    TraderId = assignment.TraderSnookerFirst5FramesHDP,
                    OtherTrader = assignment.OtherTraderSnookerFirst5FramesHDP,
                    MaxBet = assignment.MaxBetSnookerFirst5FramesHDP
                },
                new()
                {
                    Bettype = Bettype.First8FramesHDP,
                    TraderId = assignment.TraderSnookerFirst8FramesHDP,
                    OtherTrader = assignment.OtherTraderSnookerFirst8FramesHDP,
                    MaxBet = assignment.MaxBetSnookerFirst8FramesHDP
                },
                new()
                {
                    Bettype = Bettype.First9FramesHDP,
                    TraderId = assignment.TraderSnookerFirst9FramesHDP,
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
                    TraderId = assignment.TraderBaseballFTRunLineHDP,
                    OtherTrader = assignment.OtherTraderBaseballFTRunLineHDP,
                    MaxBet = assignment.MaxBetBaseballFTRunLineHDP
                },
                new()
                {
                    Bettype = Bettype.FirstH1st5inningsHDP,
                    TraderId = assignment.TraderBaseball1H1st5InningsHDP,
                    OtherTrader = assignment.OtherTraderBaseball1H1st5inningsHDP,
                    MaxBet = assignment.MaxBetBaseball1H1st5inningsHDP
                },
                new()
                {
                    Bettype = Bettype.FTTotalRunOU,
                    TraderId = assignment.TraderBaseballFTTotalRunOU,
                    OtherTrader = assignment.OtherTraderBaseballFTTotalRunOU,
                    MaxBet = assignment.MaxBetBaseballFTTotalRunOU
                },
                new()
                {
                    Bettype = Bettype.FirstH1st5inningsOU,
                    TraderId = assignment.TraderBaseball1H1st5InningsOU,
                    OtherTrader = assignment.OtherTraderBaseball1H1st5inningsOU,
                    MaxBet = assignment.MaxBetBaseball1H1st5inningsOU
                },
                new()
                {
                    Bettype = Bettype.InningHitsAndRuns,
                    TraderId = assignment.TraderBaseballInningHitsRuns,
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
                    TraderId = assignment.TraderBadmintonAllProductSetWinner,
                    OtherTrader = assignment.OtherTraderBadmintonAllProductSetWinner,
                    MaxBet = assignment.MaxBetBadmintonAllProductSetWinner
                },
                new()
                {
                    Bettype = Bettype.AllProductPointWinner,
                    TraderId = assignment.TraderBadmintonAllProductPointWinner,
                    OtherTrader = assignment.OtherTraderBadmintonAllProductPointWinner,
                    MaxBet = assignment.MaxBetBadmintonAllProductPointWinner
                },
                new()
                {
                    Bettype = Bettype.RacetoPoints7911,
                    TraderId = assignment.TraderBadmintonRacetoPoints7911,
                    OtherTrader = assignment.OtherTraderBadmintonRacetoPoints7911,
                    MaxBet = assignment.MaxBetBadmintonRacetoPoints7911
                },
                new()
                {
                    Bettype = Bettype.NextPointWinner,
                    TraderId = assignment.TraderBadmintonNextPointWinner,
                    OtherTrader = assignment.OtherTraderBadmintonNextPointWinner,
                    MaxBet = assignment.MaxBetBadmintonNextPointWinner
                },
                new()
                {
                    Bettype = Bettype.MatchHandicap,
                    TraderId = assignment.TraderBadmintonMatchHandicap,
                    OtherTrader = assignment.OtherTraderBadmintonMatchHandicap,
                    MaxBet = assignment.MaxBetBadmintonMatchHandicap
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
                    TraderId = assignment.TraderTennisAllProductLS,
                    OtherTrader = assignment.OtherTraderTennisAllProductLS,
                    MaxBet = assignment.MaxBetTennisAllProductLS
                },
                new()
                {
                    Bettype = Bettype.NextGame,
                    TraderId = assignment.TraderTennisNextGame,
                    OtherTrader = assignment.OtherTraderTennisNextGame,
                    MaxBet = assignment.MaxBetTennisNextGame
                },
                new()
                {
                    Bettype = Bettype.MostAces,
                    TraderId = assignment.TraderTennisMostAces,
                    OtherTrader = assignment.OtherTraderTennisMostAces,
                    MaxBet = assignment.MaxBetTennisMostAces
                },
                new()
                {
                    Bettype = Bettype.MostDoubleFaults,
                    TraderId = assignment.TraderTennisMostDoubleFaults,
                    OtherTrader = assignment.OtherTraderTennisMostDoubleFaults,
                    MaxBet = assignment.MaxBetTennisMostDoubleFaults
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
                TraderBasketballFTHDP = 1,
                TraderBasketball1Q3QHDP = 3,
                TraderBasketball1HHDP = 2,
                TraderBasketballFTOUOE = 4,
                TraderBasketball1HOUOE = 5,
                TraderBasketball1Q3QOUOE = 6,
                TraderFootballFTHDP = 7,
                TraderFootball1HHDP = 8,
                TraderFootball1Q3QHDP = 9,
                TraderFootballFTOU = 10,
                TraderFootball1HOU = 11,
                TraderFootball1Q3QOU = 12,
                TraderFieldHockeyFTHDP = 13,
                TraderFieldHockey1HHDP = 14,
                TraderFieldHockey1QHDP = 15,
                TraderFieldHockeyFTOU = 16,
                TraderFieldHockey1HOU = 17,
                TraderFieldHockey1QOU = 18,
                TraderIceHockeyFTHDP = 19,
                TraderIceHockey1st2ndHDP = 20,
                TraderIceHockeyFTOU = 21,
                TraderIceHockey1st2ndOU = 22,
                TraderOtherSportsMatchWinner = 23,
                TraderDartsLegsHDP = 24,
                TraderDartsMost180sHDP = 25,
                TraderPoolMatchWinnerHDP = 26,
                TraderPoolRacksWinnerHDP = 27,
                TraderTableTennisSetWinnerHDP = 28,
                TraderTableTennisSetXWinnerHDP = 29,
                TraderVolleyballSetWinnerHDPOE = 30,
                TraderVolleyballSetsXPointWinnerHDP = 31,
                TraderHandballFTHDP = 32,
                TraderHandball1HHDP = 33,
                TraderHandballFTOU = 34,
                TraderHandball1HOU = 35,
                TraderNetballFTHDP = 36,
                TraderNetball1HHDP = 37,
                TraderNetball1Q3QHDP = 38,
                TraderNetballFTOUOE = 39,
                TraderNetball1HOUOE = 40,
                TraderNetball1Q3QOUOE = 41,
                TraderCricketMatchWinnerInningsOU = 42,
                TraderCricketNextOverOUPlayerOU = 43,
                TraderCricketFT1X2 = 44,
                TraderSnookerMatchWinnerHDPOU = 45,
                TraderSnookerFramesWinnerHDPOUOE = 46,
                TraderSnookerFirst5FramesHDP = 47,
                TraderSnookerFirst8FramesHDP = 48,
                TraderSnookerFirst9FramesHDP = 49,
                TraderBaseballFTRunLineHDP = 50,
                TraderBaseball1H1st5InningsHDP = 51,
                TraderBaseballFTTotalRunOU = 52,
                TraderBaseball1H1st5InningsOU = 53,
                TraderBaseballInningHitsRuns = 54,
                TraderBadmintonAllProductSetWinner = 55,
                TraderBadmintonAllProductPointWinner = 56,
                TraderBadmintonRacetoPoints7911 = 57,
                TraderBadmintonNextPointWinner = 58,
                TraderBadmintonMatchHandicap = 59,
                TraderTennisAllProductLS = 60,
                TraderTennisNextGame = 61,
                TraderTennisMostAces = 62,
                TraderTennisMostDoubleFaults = 63,
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
                OtherTraderVolleyballSetWinnerHDPOU = "OtherTraderVolleyballSetWinnerHDPOU",
                MaxBetBasketballFTHDP = 1200,
                MaxBetBasketballFTOE = 1200,
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
                MaxBetVolleyballSetWinnerHDPOU = 1200,
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
