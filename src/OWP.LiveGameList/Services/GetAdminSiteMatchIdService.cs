using Fanex.Data.Repository;
using Newtonsoft.Json;
using Olympus.OWP.Constants;
using OWP.LiveGameList.Criteria;
using OWP.LiveGameList.Models;
using System.Data;

namespace OWP.LiveGameList.Services
{
    public interface GetAdminSiteMatchIdService
    {
        Task UpdateMinorAdminMatchId(IList<MatchInfo> matchInfos);
    }
    public class GetAdminSiteMatchIdServiceImp : GetAdminSiteMatchIdService
    {
        private readonly IDynamicRepository dynamicRepository;

        private readonly IList<int> validMinorBetTypes = new List<int>
        {
            -3, // No.of Corners
            -6, // Total Booking
            -11, // Penalty Shoot-out
            -14, // Winner
            -15, // Which Team Advance To Next Round
            -90, // Last Corner
            -91, // Last Booking
            -92, // Home Single Team O/U
            -93, // Away Single Team O/U
            -94, // Home SINGLE TEAM O/U OF CORNERS
            -95, // Away SINGLE TEAM O/U OF CORNERS
            -96, // 1st Penalty Score
            -97, // 1st Penalty Awarded
            -101, // Last Goal Kick
            -102, // 2H 1st Goal Kick
            -103, // Last Offside
            -104, // 2H 1st Offside
            -105, // Last Throw In
            -106, // 2H 1st Throw In
            -107, // Last Free Kick
            -108 // 2H 1st Free Kick
        };

        private readonly List<int> specialMinorBetTypes = new List<int>
        {
            -4, // Next Corners
            -7, // Next Booking
        };

        private readonly Dictionary<int, int> specialMinorSubTypes = new Dictionary<int, int>
        {
            { -4, 2101 }, // 1st Next Corners
            { -7, 2901 } // Next Booking
        };

        public GetAdminSiteMatchIdServiceImp(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task UpdateMinorAdminMatchId(IList<MatchInfo> matchInfos)
        {
            var minorMatches = matchInfos.Where(x => x.MinorBettypeId.HasValue && x.AdminSiteId.HasValue)
                .Select(x => new MinorMatch { ParentMatchId = x.AdminSiteId, OwpMatchId = x.OWPMatchId, SubType = GetSubTypeByMinorBetTypeId(x.MinorBettypeId) });

            var minorInfos = (await dynamicRepository
                .FetchAsync<MinorMatchInfo>(new GetMinorAdminSiteIdsCriteria { MinorMatches = JsonConvert.SerializeObject(minorMatches) }))
                .ToList();

            foreach (var match in matchInfos)
            {
                var minorMatch = minorInfos.FirstOrDefault(x => x.OwpMatchId == match.OWPMatchId);
                if (minorMatch is not null)
                {
                    match.AdminSiteId = minorMatch.AdminMatchId;
                }
            }

            var a = matchInfos.Where(x => x.OWPMatchId == 1912614);

        }

        public int? GetSubTypeByMinorBetTypeId(int? minorBetTypeId)
        {
            if (specialMinorBetTypes.Any(x => x == minorBetTypeId))
            {
                return GetSubTypeSpecialProduct(minorBetTypeId.GetValueOrDefault());
            }

            if (validMinorBetTypes.Any(x => x == minorBetTypeId))
            {
                return MinorBettype.GetRepresentativeSubtype(minorBetTypeId.GetValueOrDefault());
            }

            return null;
        }

        private int GetSubTypeSpecialProduct(int minorBetTypeId)
            => specialMinorSubTypes.ContainsKey(minorBetTypeId) ? specialMinorSubTypes[minorBetTypeId] : 0;
    }
}
