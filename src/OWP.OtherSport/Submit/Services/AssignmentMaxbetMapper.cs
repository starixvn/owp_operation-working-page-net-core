namespace OWP.OtherSport.Submit.Services
{
    using Contants;
    using Models;
    using Validation.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public interface AssignmentMaxbetMapper
    {
        Dictionary<int, List<AssignmentMaxbet>> MapBySport(List<AssignmentModel> assignments);
    }

    public class AssignmentMaxbetMapperImp : AssignmentMaxbetMapper
    {
        public Dictionary<int, List<AssignmentMaxbet>> MapBySport(List<AssignmentModel> assignments)
            => assignments.ToDictionary(a => a.OSMatchId, a => MapBySport(a));

        public static List<AssignmentMaxbet> MapBySport(AssignmentModel assignment)
        {
            var assignmentMaxBet = new List<AssignmentMaxbet>();
            assignmentMaxBet.AddRange(MapBasketball(assignment));
            assignmentMaxBet.AddRange(MapFootball(assignment));
            assignmentMaxBet.AddRange(MapFieldHockey(assignment));
            assignmentMaxBet.AddRange(MapIceHockey(assignment));
            assignmentMaxBet.AddRange(MapOtherSports(assignment));
            assignmentMaxBet.AddRange(MapDarts(assignment));
            assignmentMaxBet.AddRange(MapPool(assignment));
            assignmentMaxBet.AddRange(MapTableTennis(assignment));
            assignmentMaxBet.AddRange(MapVolleyball(assignment));
            assignmentMaxBet.AddRange(MapHandball(assignment));
            assignmentMaxBet.AddRange(MapNetball(assignment));
            assignmentMaxBet.AddRange(MapCricket(assignment));
            assignmentMaxBet.AddRange(MapSnooker(assignment));
            assignmentMaxBet.AddRange(MapBaseball(assignment));
            assignmentMaxBet.AddRange(MapBadminton(assignment));
            assignmentMaxBet.AddRange(MapTennis(assignment));

            return assignmentMaxBet;
        }

        public static List<AssignmentMaxbet> MapBasketball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Basketball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new()
                {
                    Bettype = Bettype.FTHDP,
                    TraderId = assignment.TraderBasketballFTHDP,
                    OtherTrader = assignment.OtherTraderBasketballFTHDP,
                    MaxBet = assignment.MaxBetBasketballFTHDP
                },
                new()
                {
                    Bettype = Bettype.FirstHHDP,
                    TraderId = assignment.TraderBasketball1HHDP,
                    OtherTrader = assignment.OtherTraderBasketball1HHDP,
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QHDP,
                    TraderId = assignment.TraderBasketball1Q3QHDP,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QHDP,
                },
                new()
                {
                    Bettype = Bettype.FTOUOE,
                    TraderId = assignment.TraderBasketballFTOUOE,
                    OtherTrader = assignment.OtherTraderBasketballFTOUOE,
                    MaxBet = assignment.MaxBetBasketballFTOE
                },
                new()
                {
                    Bettype = Bettype.FirstHOUOE,
                    TraderId = assignment.TraderBasketball1HOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1HOUOE
                },
                new()
                {
                    Bettype = Bettype.FirstQ3QOUOE,
                    TraderId = assignment.TraderBasketball1Q3QOUOE,
                    OtherTrader = assignment.OtherTraderBasketball1Q3QOUOE
                }
            };
        }

        public static List<AssignmentMaxbet> MapFootball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Football, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new()
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapFieldHockey(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.FieldHockey, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new()
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapIceHockey(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.IceHockey, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapOtherSports(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.OtherSports, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
                new ()
                {
                    Bettype = Bettype.MatchWinner,
                    TraderId = assignment.TraderOtherSportsMatchWinner,
                    OtherTrader = assignment.OtherTraderOtherSportsMatchWinner,
                    MaxBet = assignment.MaxBetOtherSportsMatchWinner
                }
            };
        }

        public static List<AssignmentMaxbet> MapDarts(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Darts, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }
            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapPool(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Pool, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }
            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapTableTennis(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.TableTennis, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }
            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapVolleyball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Volleyball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }
            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapHandball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Handball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapNetball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Netball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapCricket(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Cricket, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapSnooker(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Snooker, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapBaseball(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Baseball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapBadminton(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Baseball, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }

        public static List<AssignmentMaxbet> MapTennis(AssignmentModel assignment)
        {
            if (!assignment.Sport.Equals(Sport.Tennis, StringComparison.CurrentCultureIgnoreCase))
            {
                return new List<AssignmentMaxbet>();
            }

            return new List<AssignmentMaxbet>
            {
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
                }
            };
        }
    }
}