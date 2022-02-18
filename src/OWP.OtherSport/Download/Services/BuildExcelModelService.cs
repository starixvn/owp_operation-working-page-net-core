namespace OWP.OtherSport.Download.Services
{
    using AutoMapper;
    using Constants;
    using Models;
    using OWP.Extensions;
    using OWP.OtherSport.Shared.Helper;
    using OWP.OtherSport.Shared.Services;
    using OWP.OtherSport.Submit.Models;
    using Submit.Contants;

    public interface BuildExcelModelService
    {
        Task<ExcelModel> BuildExcelModel(DownloadAssignmentRequest request);
    }

    public class BuildExcelModelServiceImp : BuildExcelModelService
    {
        private readonly CommandRepository commandRepository;

        private readonly IMapper mapper;

        private readonly MatchStatusCalculator matchStatusCalculator;

        public BuildExcelModelServiceImp(
            CommandRepository commandRepository,
            MatchStatusCalculator matchStatusCalculator,
            IMapper mapper)
        {
            this.commandRepository = commandRepository;
            this.matchStatusCalculator = matchStatusCalculator;
            this.mapper = mapper;
        }

        public async Task<ExcelModel> BuildExcelModel(DownloadAssignmentRequest request)
        {
            var excelModel = new ExcelModel();

            excelModel.HeaderInfo = BuildExcelHeader(request);
            excelModel.ExcelAssignmentModels = await BuildExcelAssignments(request);

            return excelModel;
        }

        private static ExcelHeaderInfoModel BuildExcelHeader(DownloadAssignmentRequest request)
        {
            return new ExcelHeaderInfoModel
            {
                FromDate = request.From.ToString("MMM dd, yyyy"),
                ToDate = request.To.ToString("MMM dd, yyyy"),
                Employee = string.IsNullOrEmpty(request.Employee) ? "All" : request.Employee,
                Sports = request.Sports,
                OfferLive = request.OfferLives,
                MatchStatus = request.MatchStatuses,
                LinkedMatch = request.LinkedMatches
            };
        }

        public async Task<ExcelAssignmentModel> BuildExcelAssignments(DownloadAssignmentRequest request)
        {
            var assignmentSnapshot = await commandRepository.Query(request);
            await matchStatusCalculator.Map(assignmentSnapshot.Matches);

            assignmentSnapshot.Matches = FilterHelper.FilterMatchesByEmployee(request.Employee, assignmentSnapshot);

            var matchAssignments = BuildTraders(assignmentSnapshot);
            var matchMaxbets = BuildMatchSettings(assignmentSnapshot);

            return new ExcelAssignmentModel
            {
                Matches = assignmentSnapshot.Matches.Values.OrderBy(m => m.MatchNo).ToList(),
                MatchAssignments = matchAssignments,
                MaxBetSpread = matchMaxbets
            };
        }

        public Dictionary<int, List<ExcelMatchAssignment>> BuildTraders(AssignmentSnapshot assignmentSnapshot)
        {
            var matchAssignments = new Dictionary<int, List<ExcelMatchAssignment>>();

            foreach (var key in assignmentSnapshot.Matches.Keys)
            {
                var sport = assignmentSnapshot.Matches[key].Sport;
                var assignments = assignmentSnapshot.MatchAssignments.GetValue(key, new List<MatchAssignment>());
                var excelAssignments = BuildTrader(sport, assignments);

                matchAssignments.Add(key, excelAssignments.ToList());
            }

            return matchAssignments;
        }

        private IEnumerable<ExcelMatchAssignment> BuildTrader(string sport, List<MatchAssignment> assignments)
        {
            foreach (var assignment in assignments)
            {
                var excelAssignment = mapper.Map<ExcelMatchAssignment>(assignment);
                excelAssignment.ColumnIndex = GetTraderColumnIndex(assignment, sport);
                excelAssignment.Value = GetTraderValue(assignment);

                yield return excelAssignment;
            }
        }

        private static int GetTraderColumnIndex(MatchAssignment assignment, string sport)
            => assignment.AssignmentType == (int)AssignmentType.Assignment
                ? ExcelConst.TraderColumn.GetValue(new Tuple<string, string>(sport, assignment.BetTypeName), 1)
                : ExcelConst.OtherTraderColumn.GetValue(new Tuple<string, string>(sport, assignment.BetTypeName), 2);

        private static string GetTraderValue(MatchAssignment assignment)
            => assignment.UserId != 0
                    ? assignment.AssignmentNote?.Replace("<user>", assignment.Username)
                    : assignment.AssignmentNote?.Replace("#<user>", "");

        public Dictionary<int, List<ExcelMatchMaxBetSpread>> BuildMatchSettings(AssignmentSnapshot assignmentSnapshot)
        {
            var matchMaxbets = new Dictionary<int, List<ExcelMatchMaxBetSpread>>();

            foreach (var key in assignmentSnapshot.Matches.Keys)
            {
                var sport = assignmentSnapshot.Matches[key].Sport;
                var maxbets = assignmentSnapshot.MatchSettings.GetValue(key, new List<MatchSetting>());
                var excelAssignments = BuildMatchSetting(sport, maxbets);

                matchMaxbets.Add(key, excelAssignments.ToList());
            }

            return matchMaxbets;
        }

        private IEnumerable<ExcelMatchMaxBetSpread> BuildMatchSetting(string sport, List<MatchSetting> matchSettings)
        {
            foreach (var matchSetting in matchSettings)
            {
                var excelMatchSettings = mapper.Map<ExcelMatchMaxBetSpread>(matchSetting);
                excelMatchSettings.ColumnIndexMaxbet = GetColumnIndex(ExcelConst.MaxbetColumn, matchSetting, sport);
                excelMatchSettings.ValueMaxbet = GetMaxbetSpreadValue(matchSetting.MaxBet);
                excelMatchSettings.ColumnIndexSpread = GetColumnIndex(ExcelConst.SpreadColumn, matchSetting, sport);
                excelMatchSettings.ValueSpread = GetMaxbetSpreadValue(matchSetting.Spread * 100);
                yield return excelMatchSettings;
            }
        }

        private static int GetColumnIndex<T>(T excelColumn, MatchSetting matchSetting, string sport) where T : Dictionary<Tuple<string, string>, int>
            => excelColumn.GetValue(new Tuple<string, string>(sport, matchSetting.BetTypeName), 3);

        private static string GetMaxbetSpreadValue(decimal? value)
            => value?.ToString().TrimEnd('0').TrimEnd('.');
    }
}