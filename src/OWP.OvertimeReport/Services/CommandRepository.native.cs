namespace OWP.OvertimeReport.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface CommandRepository
    {
        Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByMonth(OverTimeByMonthRequest request);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;

        public CommandRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<(IEnumerable<User>, IEnumerable<OvertimeData>)> GetReportByMonth(OverTimeByMonthRequest request)
        {
            var endMonthDay = DateTime.DaysInMonth(request.Year, request.Month);
            var criteria = new GetReportByDateRangeCriteria
            {
                TraderName = request.Trader ?? string.Empty,
                Location = request.Location ?? string.Empty,
                FromDate = new DateTime(request.Year, request.Month, 1),
                ToDate = new DateTime(request.Year, request.Month, endMonthDay)
            };

            return await repository.FetchMultipleAsync<User, OvertimeData>(criteria);
        }
    }
}
