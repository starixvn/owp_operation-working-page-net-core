using OWP.Event.LiveGameAssignment.Download.Models;

namespace OWP.Event.LiveGameAssignment.Download.Repositories;

using Enums;
using EventShared.Helper;
using Fanex.Data.Repository;
using OWP.Event.EventShared.Models;
using OWP.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AssignmentModel = AssignmentModel;
using MatchInfo = MatchInfo;
using MatchNote = MatchNote;
using MatchOffer = MatchOffer;

public interface IEventAssignmentRepository
{
    Task<IEnumerable<EventModel>> GetEventInformationByEventDate(ICriteria criteria);
}

public class EventAssignmentRepository : IEventAssignmentRepository
{
    private readonly CheckIsLiveStatusServices checkIsLiveStatusServices;
    private readonly IDynamicRepository dynamicRepository;

    public EventAssignmentRepository(CheckIsLiveStatusServices checkIsLiveStatusServices, IDynamicRepository dynamicRepository)
    {
        this.checkIsLiveStatusServices = checkIsLiveStatusServices;
        this.dynamicRepository = dynamicRepository;
    }
    public async Task<IEnumerable<EventModel>> GetEventInformationByEventDate(ICriteria criteria)
    {
        var result = await dynamicRepository.FetchMultipleAsync<MatchInfo, MatchTrader, MatchNote, MatchMoney, Television, MatchOffer>(criteria);

        var matchInfos = result.Item1;
        var traders = result.Item2;
        var notes = result.Item3;
        var matchSettings = result.Item4;
        var _ = result.Item5; // NOSONAR
        var matchOffers = result.Item6;
        var events = await GetInfo(matchInfos, traders, notes, matchSettings, matchOffers);
        return events;
    }

    private static List<string> GetMatchNote(IEnumerable<MatchNote> notes, int matchId)
    {
        return notes.Where(x => x.OWPMatchId == matchId).Select(x => x.Note).ToList();
    }

    private static string GetMainTrader(IEnumerable<MatchTrader> traders, int matchId, int bettype)
    {
        return traders
            .Where(x => x.OWPMatchId == matchId && x.Bettype == bettype && x.AssignmentType == (short)AssignmentType.Main)
            .Select(tr => tr.Username).FirstOrDefault();
    }

    private static string GetOtherTraders(IEnumerable<MatchTrader> traders, int matchId, int bettype)
    {
        var otherAssignments = traders
            .Where(x => x.OWPMatchId == matchId && x.Bettype == bettype && x.AssignmentType == (short)AssignmentType.Others)
            .Select(x => new OtherAssignment
            {
                Username = x.Username,
                Note = x.AssignmentNote
            });

        return string.Join(", ", otherAssignments.Select(x => x.Note.Replace("#<user>", "#" + x.Username)));
    }

    private static string GetMainTraderNameForSpecialBettype(IEnumerable<MatchTrader> traders, MatchInfo matchInfo, int minorBettype)
    {
        return GetSpecialBettypeTraders(traders, matchInfo, minorBettype, (int)AssignmentType.Main)
                .Select(tr => tr.Username).FirstOrDefault();
    }

    private static string GetOtherTradersNameForSpecialBettype(IEnumerable<MatchTrader> traders, MatchInfo matchInfo, int minorBettype)
    {
        var otherTraders = GetSpecialBettypeTraders(traders, matchInfo, minorBettype, (int)AssignmentType.Others)
            .Select(x => new OtherAssignment
            {
                Username = x.Username,
                Note = x.AssignmentNote
            });

        return string.Join(", ", otherTraders.Select(x => x.Note.Replace("#<user>", "#" + x.Username)));
    }

    private static IEnumerable<MatchTrader> GetSpecialBettypeTraders(IEnumerable<MatchTrader> traders, MatchInfo matchInfo, int minorBettype, int assignmentType)
    {
        IEnumerable<MatchTrader> resultTraders;

        if (!matchInfo.IsMinorMatch)
        {
            resultTraders = traders.Where(x => x.MinorBettypeId == minorBettype
                        && x.ParentMatchId == matchInfo.OWPMatchId
                        && x.Bettype == (int)MajorBetType.Handicap
                        && x.AssignmentType == assignmentType);
        }
        else if (minorBettype == (int)MinorBetType.Specific15Mins)
        {
            resultTraders = GetTradersForSpecificMatch(traders, matchInfo, assignmentType, (int)MinorBetType.Specific15MinsCorners, (int)MinorBetType.Specific15MinsTotalBookings);
        }
        else if (minorBettype == (int)MinorBetType.AllInOne)
        {
            resultTraders = GetTradersForSpecificMatch(traders, matchInfo, assignmentType, (int)MinorBetType.AllInOneCorners, (int)MinorBetType.AllInOneTotalBookings);
        }
        else if (minorBettype == (int)MinorBetType.FastMarket)
        {
            resultTraders = GetTradersForSpecificMatch(traders, matchInfo, assignmentType, (int)MinorBetType.FastMarketCorners, (int)MinorBetType.FastMarketTotalBookings);
        }
        else
        {
            resultTraders = new List<MatchTrader>();
        }

        return resultTraders;
    }

    private static IEnumerable<MatchTrader> GetTradersForSpecificMatch(IEnumerable<MatchTrader> traders, MatchInfo matchInfo, int assignmentType, int minorBetTypeCorner, int minorBetTypeBooking)
    {
        IEnumerable<MatchTrader> resultTraders;

        switch (matchInfo.MinorBettypeId)
        {
            case (int)MinorBetType.NoOfCorner:
                resultTraders = traders.Where(x => x.MinorBettypeId == minorBetTypeCorner
                                                   && x.ParentMatchId == matchInfo.ParentMatchId
                                                   && x.Bettype == (int)MajorBetType.Handicap
                                                   && x.AssignmentType == assignmentType);
                break;

            case (int)MinorBetType.TotalBooking:
                resultTraders = traders.Where(x => x.MinorBettypeId == minorBetTypeBooking
                                                   && x.ParentMatchId == matchInfo.ParentMatchId
                                                   && x.Bettype == (int)MajorBetType.Handicap
                                                   && x.AssignmentType == assignmentType);
                break;

            default:
                resultTraders = new List<MatchTrader>();
                break;
        }

        return resultTraders;
    }

    private static string GetMaxBet(
        IEnumerable<MatchMoney> matchSettings,
        int matchId,
        int bettype)
    {
        var matchSetting = matchSettings.FirstOrDefault(x => x.OWPMatchId == matchId && x.Bettype == bettype);
        return GetMaxBet(matchSetting);
    }

    private static string GetMaxBet(MatchMoney matchSetting)
    {
        return matchSetting == null ? string.Empty : matchSetting.MaxBet.ToOWPMaxBetFormat();
    }

    private static string GetSpread(
        IEnumerable<MatchMoney> matchSettings,
        int matchId,
        int bettype)
    {
        var matchSetting = matchSettings.FirstOrDefault(x => x.OWPMatchId == matchId && x.Bettype == bettype);
        return GetSpread(matchSetting);
    }

    private static string GetSpread(MatchMoney matchSetting)
    {
        return matchSetting == null ? string.Empty : matchSetting.Spread.ToOWPSpreadFormat();
    }

    private static MatchMoney GetMatchSettingForSpecialBettype(
        IEnumerable<MatchMoney> matchSettings,
        MatchInfo matchInfo,
        int bettypeId)
    {
        MatchMoney matchSetting;
        if (!matchInfo.IsMinorMatch)
        {
            matchSetting = matchSettings.FirstOrDefault(x => x.MinorBettypeId == bettypeId && x.ParentMatchId == matchInfo.OWPMatchId);
        }
        else if (matchInfo.MinorBettypeId == (int)MinorBetType.NoOfCorner && bettypeId == (int)MinorBetType.Specific15Mins)
        {
            matchSetting = GetMatchSettingForCornerMatch(matchSettings, matchInfo);
        }
        else if (matchInfo.MinorBettypeId == (int)MinorBetType.TotalBooking && bettypeId == (int)MinorBetType.Specific15Mins)
        {
            matchSetting = GetMatchSettingForBookingMatch(matchSettings, matchInfo);
        }
        else
        {
            matchSetting = null;
        }

        return matchSetting;
    }

    private static MatchMoney GetMatchSettingForBookingMatch(IEnumerable<MatchMoney> matchSettings, MatchInfo matchInfo)
    {
        return matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsTotalBookings
                    && x.Bettype == (int)MajorBetType.Handicap
                    && x.ParentMatchId == matchInfo.ParentMatchId);
    }

    private static MatchMoney GetMatchSettingForCornerMatch(IEnumerable<MatchMoney> matchSettings, MatchInfo matchInfo)
    {
        return matchSettings.FirstOrDefault(x => x.MinorBettypeId == (int)MinorBetType.Specific15MinsCorners
                    && x.Bettype == (int)MajorBetType.Handicap
                    && x.ParentMatchId == matchInfo.ParentMatchId);
    }

    private async Task<IEnumerable<EventModel>> GetInfo( //NOSONAR
       IEnumerable<MatchInfo> matchInfos,
       IEnumerable<MatchTrader> traders,
       IEnumerable<MatchNote> notes,
       IEnumerable<MatchMoney> matchSettings,
       IEnumerable<MatchOffer> matchOffers)
    {
        var matchList = new List<EventModel>();
        var adminSiteIds = string.Join(",", matchInfos.Select(x => x.ParentAdminSiteId).Distinct());
        var mainDBResult = await checkIsLiveStatusServices.GetOfferLiveStatusFromMainDB(adminSiteIds);
        foreach (var matchInfo in matchInfos)
        {
            var match = new EventModel
            {
                MatchNo = matchInfo.MatchNo,
                InfoSiteMatchId = matchInfo.InfoSiteMatchId.HasValue ? matchInfo.InfoSiteMatchId.ToString() : string.Empty,
                AdminSiteId = matchInfo.MatchId.HasValue ? matchInfo.MatchId.ToString() : string.Empty,
                Time = matchInfo.KickOffTime.ToOWPTimeFormat(),
                OtherTime = matchInfo.OtherTime.HasValue ? matchInfo.OtherTime.Value.ToOWPTimeFormat() : string.Empty,
                MatchOffer = WebUtility.HtmlDecode(matchInfo.MatchOffer),
                MatchOfferList = matchOffers.Where(x => x.OWPMatchId == matchInfo.OWPMatchId).ToList(),
                LeagueName = matchInfo.League,
                HomeName = matchInfo.Home,
                AwayName = matchInfo.Away,
                Round = matchInfo.Round,
                TeamRound = matchInfo.TeamRound,
                OutSide = matchInfo.OutSide,
                Remark = matchInfo.Remark,
                MatchStatus = matchInfo.MatchStatus,
                Notes = GetMatchNote(notes, matchInfo.OWPMatchId),
                NoOfHDP = matchInfo.NoOfHDP,
                EventDateTime = matchInfo.EventDateTime,
                MinorBettypeId = matchInfo.MinorBettypeId.HasValue ? matchInfo.MinorBettypeId.Value : 0,
                Assignments = new AssignmentModel
                {
                    MatchId = matchInfo.OWPMatchId,
                    Handicap = new HandicapAssignmentModel
                    {
                        MaxBet = GetMaxBet(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.Handicap),
                        Spread = GetSpread(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.Handicap),
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.Handicap),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.Handicap)
                    },
                    Handicap1st = new Handicap1stAssignmentModel
                    {
                        MaxBet = GetMaxBet(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.Handicap1St),
                        Spread = GetSpread(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.Handicap1St),
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.Handicap1St),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.Handicap1St)
                    },
                    OverUnder = new OverUnderAssignmentModel
                    {
                        MaxBet = GetMaxBet(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder),
                        Spread = GetSpread(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder),
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder)
                    },
                    OverUnder1st = new OverUnder1stAssignmentModel
                    {
                        MaxBet = GetMaxBet(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder1St),
                        Spread = GetSpread(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder1St),
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder1St),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.OverUnder1St)
                    },
                    OneXTwo = new OneXTwoAssignmentModel
                    {
                        MaxBet = GetMaxBet(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo),
                        Spread = GetSpread(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo),
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo)
                    },
                    OneXTwo1st = new OneXTwo1stAssignmentModel
                    {
                        MaxBet = GetMaxBet(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo1St),
                        Spread = GetSpread(matchSettings, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo1St),
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo1St),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.OneXTwo1St)
                    },
                    Specific15Mins = new Specific15MinsAssignmentModel
                    {
                        MainTrader = GetMainTraderNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.Specific15Mins),
                        OtherTraders = GetOtherTradersNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.Specific15Mins)
                    },
                    Specific10Mins = new Specific10MinsAssignmentModel
                    {
                        MainTrader = GetMainTraderNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.Specific10Mins),
                        OtherTraders = GetOtherTradersNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.Specific10Mins)
                    },
                    FM = new FMAssignmentModel
                    {
                        MaxBet = GetMaxBet(GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.FastMarket)),
                        Spread = GetSpread(GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.FastMarket)),
                        MainTrader = GetMainTraderNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.FastMarket),
                        OtherTraders = GetOtherTradersNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.FastMarket)
                    },
                    AIO = new AIOAssignmentModel
                    {
                        MainTrader = GetMainTraderNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.AllInOne),
                        OtherTraders = GetOtherTradersNameForSpecialBettype(traders, matchInfo, (int)MinorBetType.AllInOne)
                    },
                    LA2nd = new LA2ndAssignmentModel
                    {
                        MainTrader = GetMainTrader(traders, matchInfo.OWPMatchId, (int)MajorBetType.La2Nd),
                        OtherTraders = GetOtherTraders(traders, matchInfo.OWPMatchId, (int)MajorBetType.La2Nd)
                    }
                },
                TVCode = matchInfo.TV,
                Channel = matchInfo.Channel,
                IPTV = matchInfo.IPTV,
                WatchBet = matchInfo.WatchBet,
                Agent = matchInfo.Agent,
                AgentEst = matchInfo.AgentEst,
                RemarkEst = matchInfo.RemarkEst,
                RemarkBy = matchInfo.RemarkBy,
                OtherProvider = matchInfo.OtherProvider,
                Provider = matchInfo.Provider,
                ReporterPhone = matchInfo.Phone,
                Website = matchInfo.Website,
                Website2 = matchInfo.Website2,
                Website3 = matchInfo.Website3,
                Website4 = matchInfo.Website4,
                IsSyncLive = matchInfo.IsLive &&
                    checkIsLiveStatusServices.GetOfferLiveStatus(matchInfo.MinorBettypeId, matchInfo.ParentAdminSiteId, mainDBResult)
            };

            var matchSettingForXmins = GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.Specific15Mins)
                                    ?? GetMatchSettingForSpecialBettype(matchSettings, matchInfo, (int)MinorBetType.Specific10Mins);

            match.Assignments.SpecificXMins = new SpecificXMinsAssignmentModel
            {
                MaxBet = GetMaxBet(matchSettingForXmins),
                Spread = GetSpread(matchSettingForXmins)
            };
            matchList.Add(match);
        }

        return matchList;
    }
}
