namespace OWP.Report.OvertimeReport.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Configuration;
    using Models;

    public interface CommandRepository
    {
        Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByMonth(OverTimeRequest request);

        Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByYear(OverTimeRequest request);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;
        private readonly INapSettings napSetting;

        public CommandRepositoryImp(IDynamicRepository repository, INapSettings napSetting)
        {
            this.repository = repository;
            this.napSetting = napSetting;
        }

        public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByMonth(OverTimeRequest request)
        {
            var endMonthDay = DateTime.DaysInMonth(request.Year, request.Month);
            var fromDate = new DateTime(request.Year, request.Month, 1);
            var toDate = new DateTime(request.Year, request.Month, endMonthDay);

            return await GetReportByDateRange(request, fromDate, toDate);
        }

        public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByYear(OverTimeRequest request)
        {
            var fromDate = new DateTime(request.Year, 1, 1);
            var toDate = new DateTime(request.Year, 12, 31);

            return await GetReportByDateRange(request, fromDate, toDate);
        }

        public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByDateRange(OverTimeRequest request, DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetReportByDateRangeCriteria
            {
                TraderName = request.Trader ?? string.Empty,
                Location = request.Location ?? string.Empty,
                FromDate = fromDate,
                ToDate = toDate,
                SiteId = napSetting.SiteId
            };

            return await repository.FetchMultipleAsync<User, OvertimeData>(criteria);
        }
    }
}