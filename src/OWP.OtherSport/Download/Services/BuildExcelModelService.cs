namespace OWP.OtherSport.Download.Services
{
    using AutoMapper;
    using OWP.Extensions;
    using OWP.OtherSport.Download.Constants;
    using OWP.OtherSport.Download.Models;
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface BuildExcelModelService
    {
        Task<ExcelModel> BuildExcelModel(DownloadAssignmentRequest request);
    }

    public class BuildExcelModelServiceImp : BuildExcelModelService
    {
        private readonly CommandRepository commandRepository;

        private readonly IMapper mapper;

        public BuildExcelModelServiceImp(
            CommandRepository commandRepository,
            IMapper mapper)
        {
            this.commandRepository = commandRepository;
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
                MatchStatus = request.MatchStatuses
            };
        }

        public async Task<ExcelAssignmentModel> BuildExcelAssignments(DownloadAssignmentRequest request)
        {
            var assignmentSnapshot = await commandRepository.Query(request);
            assignmentSnapshot.Matches = FilterMatchesByEmployee(request.Employee, assignmentSnapshot);

            var matchAssignments = BuildTraders(assignmentSnapshot);
            var matchMaxbets = BuildMatchSettings(assignmentSnapshot);

            return new ExcelAssignmentModel
            {
                Matches = assignmentSnapshot.Matches.Values.OrderBy(m => m.MatchNo).ToList(),
                MatchAssignments = matchAssignments,
                MaxBetSpread = matchMaxbets
            };
        }

        private static Dictionary<int, MatchInfo> FilterMatchesByEmployee(string employee, AssignmentSnapshot assignmentSnapshot)
        {
            if (string.IsNullOrEmpty(employee))
            {
                return assignmentSnapshot.Matches;
            }

            var matches = new Dictionary<int, MatchInfo>();
            foreach (var key in assignmentSnapshot.Matches.Keys)
            {
                var assignments = assignmentSnapshot.MatchAssignments.GetValue(key, new List<MatchAssignment>());
                var existsAssignment = assignments.Any(match =>
                    match.Username.IsContains(employee) ||
                    match.Nickname.IsContains(employee));
                if (existsAssignment)
                {
                    matches.Add(key, assignmentSnapshot.Matches[key]);
                }
            }

            return matches;
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
                excelMatchSettings.ValueMaxbet = GetMaxbetValue(matchSetting);
                excelMatchSettings.ColumnIndexSpread = GetColumnIndex(ExcelConst.SpreadColumn, matchSetting, sport);
                excelMatchSettings.ValueSpread = matchSetting.Spread?.ToString();
                yield return excelMatchSettings;
            }
        }

        private static int GetColumnIndex<T>(T excelColumn, MatchSetting matchSetting, string sport) where T : Dictionary<Tuple<string, string>, int>
            => excelColumn.GetValue(new Tuple<string, string>(sport, matchSetting.BetTypeName), 3);

        private static string GetMaxbetValue(MatchSetting matchSetting)
            => matchSetting
                .MaxBet?
                    .ToString()
                        .TrimEnd('0')
                            .TrimEnd('.');
    }
}
