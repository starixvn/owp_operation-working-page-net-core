namespace OWP.LiveDetail.InfositeMatch
{
    using System.Linq;
    using System.Threading.Tasks;
    using Agent;
    using Fanex.Data.Repository;
    using Models;
    using OWP.Extensions;
    using Television;

    public interface InfositeMatchRepository
    {
        Task<ResultDataObject> Get(GetInfositeMatchModel model);
    }

    public class InfositeMatchRepositoryImp : InfositeMatchRepository
    {
        private readonly IDynamicRepository repository;

        public InfositeMatchRepositoryImp(IDynamicRepository repository)
        {
            this.repository = repository;
        }

        public async Task<ResultDataObject> Get(GetInfositeMatchModel model)
        {
            var criteria = new GetInfoSiteMatchCriteria
            {
                InfoSiteMatchId = model.InfositeMatchId,
                EventDate = model.EventDate
            };

            criteria.ThrowExceptionIfInvalid();
            var tuple = await repository.FetchMultipleAsync<InfositeMatchModel, Agent, Television>(criteria);
            var infositeMatch = tuple.Item1.FirstOrDefault();
            if (infositeMatch != null)
            {
                infositeMatch.Agents = tuple.Item2.ToList();
                infositeMatch.Televisions = tuple.Item3.ToList();
            }

            return new ResultDataObject
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg,
                Data = infositeMatch
            };
        }
    }
}