using OWP.API.SyncMaxbet.Models;
using OWP.Enums;
using OWP.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.API.SyncMaxbet.Builder
{
    public interface SpreadBuilder
    {
        Task<MaxbetSpread> BuildSpread(int owpMatchId);
    }

    public class SpreadBuilderImp : SpreadBuilder
    {
        private const int leagueGroupMJ = 1;

        private readonly SyncMaxbetReposiory syncMaxbetReposiory;

        public SpreadBuilderImp(SyncMaxbetReposiory syncMaxbetReposiory)
        {
            this.syncMaxbetReposiory = syncMaxbetReposiory;
        }

        public async Task<MaxbetSpread> BuildSpread(int owpMatchId)
        {
            var spreadTupple = await syncMaxbetReposiory.GetSpreadByOwpmatchId(owpMatchId);
            var spread = spreadTupple.Item1.ToList();
            var leagueGroupId = spreadTupple.Item2?.FirstOrDefault();

            if (!spread.Any())
            {
                return new MaxbetSpread();
            }

            if (spread.Count() == 4)
            {
                return BuildSpreadSender(spread);
            }

            return leagueGroupId == leagueGroupMJ
                ? BuildSpreadSenderForLeagueGroupMJ(spread)
                : BuildSpreadSenderForLeagueGroupNotAllowMJ(spread);
        }

        public MaxbetSpread BuildSpreadSenderForLeagueGroupNotAllowMJ(List<SpreadModel> spread)
        {
            var hasExistsFirstHalfHdp = spread.Any(s => s.BettypeId == (int)Bettype.FirstHalfHandicap);
            var hasExistsFirstHalfOu = spread.Any(s => s.BettypeId == (int)Bettype.FirstHalfOverUnder);

            var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.Handicap) ?? new SpreadModel();
            var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.OverUnder) ?? new SpreadModel();

            if (!hasExistsFirstHalfHdp)
            {
                if (handicap.Spread < (decimal)0.1)
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)Bettype.FirstHalfHandicap,
                        Spread = (decimal)0.1
                    });
                }
                else
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)Bettype.FirstHalfHandicap,
                        Spread = handicap.Spread
                    });
                }
            }

            if (!hasExistsFirstHalfOu)
            {
                if (overUnder.Spread < (decimal)0.1)
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)Bettype.FirstHalfOverUnder,
                        Spread = (decimal)0.1
                    });
                }
                else
                {
                    spread.Add(new SpreadModel
                    {
                        BettypeId = (int)Bettype.FirstHalfOverUnder,
                        Spread = overUnder.Spread
                    });
                }
            }

            return BuildSpreadSender(spread);
        }

        public MaxbetSpread BuildSpreadSenderForLeagueGroupMJ(List<SpreadModel> spread)
        {
            var hasExistsFirstHalfHdp = spread.Any(s => s.BettypeId == (int)Bettype.FirstHalfHandicap);
            var hasExistsFirstHalfOu = spread.Any(s => s.BettypeId == (int)Bettype.FirstHalfOverUnder);

            var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.Handicap) ?? new SpreadModel();
            var overUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.OverUnder) ?? new SpreadModel();
            if (!hasExistsFirstHalfHdp)
            {
                spread.Add(new SpreadModel
                {
                    BettypeId = (int)Bettype.FirstHalfHandicap,
                    Spread = handicap.Spread
                });
            }
            if (!hasExistsFirstHalfOu)
            {
                spread.Add(new SpreadModel
                {
                    BettypeId = (int)Bettype.FirstHalfOverUnder,
                    Spread = overUnder.Spread
                });
            }
            return BuildSpreadSender(spread);
        }

        public MaxbetSpread BuildSpreadSender(List<SpreadModel> spread)
        {
            var handicap = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.Handicap) ?? new SpreadModel();
            var overunder = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.OverUnder) ?? new SpreadModel();
            var firstHalfHandicap = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.FirstHalfHandicap) ?? new SpreadModel();
            var firstHalfOverUnder = MaxbetHelper.FindSpreadByBettype(spread, (int)Bettype.FirstHalfOverUnder) ?? new SpreadModel();
            return new MaxbetSpread
            {
                Handicap = handicap.Spread,
                OverUnder = overunder.Spread,
                FirstHalfHandicap = firstHalfHandicap.Spread,
                FirstHalfOverUnder = firstHalfOverUnder.Spread
            };
        }
    }
}
