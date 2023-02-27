using OWP.OtherSport.Submit.Constants;
using OWP.OtherSport.Submit.Models;
using OWP.OtherSport.Validation.Models;

namespace OWP.OtherSport.Submit.Services;

public interface AssignmentMaxbetMapper
{
    Dictionary<int, List<AssignmentMaxbet>> MapBySports(List<AssignmentModel> assignments);
}

public class AssignmentMaxbetMapperImp : AssignmentMaxbetMapper
{
    public Dictionary<int, List<AssignmentMaxbet>> MapBySports(List<AssignmentModel> assignments)
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
                Trader = assignment.TraderBasketballFTHDP,
                OtherTrader = assignment.OtherTraderBasketballFTHDP,
                MaxBet = assignment.MaxBetBasketballFTHDP,
                Spread = assignment.SpreadBasketballFTHDP
            },
            new()
            {
                Bettype = Bettype.FirstHHDP,
                Trader = assignment.TraderBasketball1HHDP,
                OtherTrader = assignment.OtherTraderBasketball1HHDP
            },
            new()
            {
                Bettype = Bettype.FirstQ3QHDP,
                Trader = assignment.TraderBasketball1Q3QHDP,
                OtherTrader = assignment.OtherTraderBasketball1Q3QHDP
            },
            new()
            {
                Bettype = Bettype.FTOUOE,
                Trader = assignment.TraderBasketballFTOUOE,
                OtherTrader = assignment.OtherTraderBasketballFTOUOE,
                MaxBet = assignment.MaxBetBasketballFTOUOE,
                Spread = assignment.SpreadBasketballFTOUOE
            },
            new()
            {
                Bettype = Bettype.FTOE,
                MaxBet = assignment.MaxBetBasketballFTOE,
                Spread = assignment.SpreadBasketballFTOE
            },
            new()
            {
                Bettype = Bettype.FirstHOUOE,
                Trader = assignment.TraderBasketball1HOUOE,
                OtherTrader = assignment.OtherTraderBasketball1HOUOE
            },
            new()
            {
                Bettype = Bettype.FirstQ3QOUOE,
                Trader = assignment.TraderBasketball1Q3QOUOE,
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
            }
        };
    }

    public static List<AssignmentMaxbet> MapBadminton(AssignmentModel assignment)
    {
        if (!assignment.Sport.Equals(Sport.Badminton, StringComparison.CurrentCultureIgnoreCase))
        {
            return new List<AssignmentMaxbet>();
        }

        return new List<AssignmentMaxbet>
        {
            new()
            {
                Bettype = Bettype.AllProductSetWinner,
                Trader = assignment.TraderBadmintonAllProductSetWinner,
                OtherTrader = assignment.OtherTraderBadmintonAllProductSetWinner,
                Spread = assignment.SpreadBadmintonAllProductSetWinner
            },
            new()
            {
                Bettype = Bettype.AllProductPointWinner,
                Trader = assignment.TraderBadmintonAllProductPointWinner,
                OtherTrader = assignment.OtherTraderBadmintonAllProductPointWinner,
                Spread = assignment.SpreadBadmintonAllProductPointWinner
            },
            new()
            {
                Bettype = Bettype.RacetoPoints7911,
                Trader = assignment.TraderBadmintonRacetoPoints7911,
                OtherTrader = assignment.OtherTraderBadmintonRacetoPoints7911,
                Spread = assignment.SpreadBadmintonRacetoPoints7911
            },
            new()
            {
                Bettype = Bettype.NextPointWinner,
                Trader = assignment.TraderBadmintonNextPointWinner,
                OtherTrader = assignment.OtherTraderBadmintonNextPointWinner,
                Spread = assignment.SpreadBadmintonNextPointWinner
            },
            new()
            {
                Bettype = Bettype.MatchHandicap,
                Trader = assignment.TraderBadmintonMatchHandicap,
                OtherTrader = assignment.OtherTraderBadmintonMatchHandicap,
                Spread = assignment.SpreadBadmintonMatchHandicap
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
            }
        };
    }
}