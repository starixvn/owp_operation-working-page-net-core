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
        Task<IEnumerable<OvertimeData>> GetReportByMonth(OverTimeByMonthRequest request);
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;

        public CommandRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<OvertimeData>> GetReportByMonth(OverTimeByMonthRequest request)
        {
            var criteria = new GetReportByDateRangeCriteria
            {
                TraderName = request.Trader,
                Location = request.Location,
                FromDate = new DateTime(request.Year, request.Month, 1),
                ToDate = new DateTime(request.Year, request.Month + 1, 1)
            };

            return await repository.FetchAsync<OvertimeData>(criteria);
        }
    }
}
