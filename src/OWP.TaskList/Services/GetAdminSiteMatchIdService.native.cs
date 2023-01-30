namespace OWP.TaskList.Services
{
    using OWP.Criteria;
    using Fanex.Data.Repository;
    using Olympus.OWP.Constants;
    public interface GetAdminSiteMatchIdService
    {
        Task<int?> GetAdminSiteMatchId(int owpMatchId, int parentMatchId, int? minorBettypeId);
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

        public async Task<int?> GetAdminSiteMatchId(int owpMatchId, int parentMatchId, int? minorBettypeId)
        {
            var getAdminSiteMatchIdCriteria = new GetAdminSiteMatchIdCriteria
            {
                OWPMatchId = minorBettypeId.HasValue ? parentMatchId : owpMatchId
            };
            var adminSiteMatchIds = await dynamicRepository.FetchAsync<int?>(getAdminSiteMatchIdCriteria);
            var adminSiteMatchId = adminSiteMatchIds.FirstOrDefault();

            if (!minorBettypeId.HasValue || !adminSiteMatchId.HasValue)
            {
                return adminSiteMatchId;
            }

            int subType = 0;
            if (specialMinorBetTypes.Any(x => x == minorBettypeId.Value))
            {
                subType = GetSubTypeSpecialProduct(minorBettypeId.Value);
            }

            if (validMinorBetTypes.Any(x => x == minorBettypeId.Value))
            {
                subType = MinorBettype.GetRepresentativeSubtype(minorBettypeId.Value);
            }

            if (subType == 0)
            {
                return null;
            }

            var getMinorCriteria = new GetMinorAdminSiteMatchCriteria
            {
                MatchId = adminSiteMatchId.Value,
                SubType = subType
            };
            var minors = await dynamicRepository.FetchAsync<int?>(getMinorCriteria);

            return minors.FirstOrDefault();
        }

        private int GetSubTypeSpecialProduct(int minorBettypeId)
            => specialMinorSubTypes.ContainsKey(minorBettypeId) ? specialMinorSubTypes[minorBettypeId] : 0;
    }
}
