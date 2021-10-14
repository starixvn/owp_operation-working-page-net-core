namespace OWP.OvertimeReport.Services
{
    using Criteria;
    using Fanex.Data.Repository;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface CommandRepository
    {
        Task<IEnumerable<OvertimeByMonthData>> GetReportByMonth();
    }

    public class CommandRepositoryImp : CommandRepository
    {
        private readonly IDynamicRepository repository;

        public CommandRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<OvertimeByMonthData>> GetReportByMonth()
        {
            var criteria = new GetReportByMonthCriteria();

            return await repository.FetchAsync<OvertimeByMonthData>(criteria);
        }
    }
}
