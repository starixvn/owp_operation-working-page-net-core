using Fanex.Data.Repository;
using OWP.Extensions;
using OWP.Models;
// ReSharper disable InvertIf

namespace OWP.LiveDetail.InfositeMatch;

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
        var tuple = await repository.FetchMultipleAsync<InfositeMatchModel, Agent.Agent, Television.Television>(criteria);
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