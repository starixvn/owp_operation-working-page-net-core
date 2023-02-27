
using AutoMapper;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using OWP.Enums;
using OWP.Extensions;
using OWP.LiveDetail.Common;
using OWP.Models;
using OWP.Utils;

namespace OWP.LiveDetail.MatchInfo;

public interface MatchInfoRepository
{
    Task<MatchInfoModel> Query(int owpMatchId);

    Task<ValidateResult> ValidatePhone(ValidatePhoneModel model);

    Task<ResultDataObject> UpdateMatchInfo(UpdateMatchInfoModel model);

    Task<AssignmentSnapshot> QueryMatchesByDate(DateTime eventDate);

    Task<ResultDataObject> UpdateProviderInfo(UpdateProviderInfoModel model);

    Task<ResultDataObject> UpdateTradingInfo(UpdateTradingInfoModel model);

    Task<IEnumerable<Television.Television>> GetTelevisionsByMatchId(int owpMatchId);

    Task<IEnumerable<OtherProvider.OtherProvider>> GetOtherProvidersByMatchId(int owpMatchId);

    Task<IEnumerable<Provider.Provider>> GetProvidersByMatchId(int owpMatchId);

    Task<IEnumerable<Agent.Agent>> GetAgentsByMatchId(int owpMatchId);

    Task<IEnumerable<MatchStatusModel>> GetMatchStatusByMatchId(int owpMatchId);

    Task<IEnumerable<MatchOfferModel>> GetMatchOfferByMatchId(int owpMatchId);

    Task<IEnumerable<RefResourceModel>> GetAllByRefName(string refName);

    Task<IsLiveStatusModel> GetMatchInfoForCheckIsLive(int matchId, int infoSiteMatchId);
}

public class MatchInfoRepositoryImp : MatchInfoRepository
{
    private readonly IMapper mapper;
    private readonly IDynamicRepository repository;
    private readonly INapSession session;

    public MatchInfoRepositoryImp(IMapper mapper, IDynamicRepository repository, INapSession session)
    {
        this.mapper = mapper;
        this.repository = repository;
        this.session = session;
    }

    public async Task<MatchInfoModel> Query(int owpMatchId)
    {
        var criteria = new QueryCriteria
        {
            OWPMatchId = owpMatchId
        };

        var matchInfo = await repository.GetAsync<MatchInfoModel>(criteria);
        if (matchInfo != null)
        {
            matchInfo.Channel = TemplateIpTvChannelBuilder.Build(matchInfo.IpTv, matchInfo.Channel);
        }

        return matchInfo;
    }

    public async Task<ValidateResult> ValidatePhone(ValidatePhoneModel model)
    {
        var criteria = mapper.Map<ValidatePhoneCriteria>(model);
        criteria.ThrowExceptionIfInvalid();
        var results = await repository.FetchAsync<ValidateEntity>(criteria);

        return new ValidateResult
        {
            Results = results.Where(x => x.ResultType != (int)RuleReturnType.Passed)
        };
    }

    public async Task<ResultDataObject> UpdateMatchInfo(UpdateMatchInfoModel model)
    {
        var updateMatchInfoCriteria = mapper.Map<UpdateMatchInfoCriteria>(model);

        updateMatchInfoCriteria.ActorId = session.UserId;

        await repository.ExecuteAsync(updateMatchInfoCriteria);

        return new ResultDataObject
        {
            Data = updateMatchInfoCriteria.SessionId,
            Code = updateMatchInfoCriteria.ErrCode,
            Message = updateMatchInfoCriteria.ErrMsg
        };
    }

    public async Task<AssignmentSnapshot> QueryMatchesByDate(DateTime eventDate)
    {
        var matchesTask = repository.FetchAsync<GeneralMatch>(new GetAllMatchesByTimeRangeCriteria { EventDate = eventDate });

        return new AssignmentSnapshot
        {
            Matches = (await matchesTask).ToDictionary(m => m.MatchId)
        };
    }

    public async Task<ResultDataObject> UpdateProviderInfo(UpdateProviderInfoModel model)
    {
        //model.IpTv=
        var splitIpTvChannel = model.Channel.Trim().Split('/');
        model.IpTv = splitIpTvChannel.Any()
            ? splitIpTvChannel.First()
            : string.Empty;
        model.Channel = splitIpTvChannel.Length > 1
            ? string.Join("/", splitIpTvChannel.Skip(1))
            : string.Empty;
        var criteriaUpdateOwnMatch = mapper.Map<UpdateProviderInfoCriteria>(model);
        criteriaUpdateOwnMatch.ActorId = session.UserId;
        await repository.ExecuteAsync(criteriaUpdateOwnMatch);
        return new ResultDataObject
        {
            Data = criteriaUpdateOwnMatch.SessionId,
            Code = criteriaUpdateOwnMatch.ErrCode,
            Message = criteriaUpdateOwnMatch.ErrMsg
        };
    }

    public async Task<ResultDataObject> UpdateTradingInfo(UpdateTradingInfoModel model)
    {
        var criteria = mapper.Map<UpdateTradingInfoCriteria>(model);
        criteria.ActorId = session.UserId;
        await repository.ExecuteAsync(criteria);
        return new ResultDataObject
        {
            Data = criteria.SessionId,
            Code = criteria.ErrCode,
            Message = criteria.ErrMsg
        };
    }

    public async Task<IEnumerable<Television.Television>> GetTelevisionsByMatchId(int owpMatchId)
    {
        var criteria = new GetTelevisionsByMatchIdCriteria
        {
            OWPMatchId = owpMatchId
        };
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<Television.Television>(criteria);
    }

    public async Task<IEnumerable<OtherProvider.OtherProvider>> GetOtherProvidersByMatchId(int owpMatchId)
    {
        var criteria = new GetOtherProvidersByMatchIdCriteria
        {
            OWPMatchId = owpMatchId
        };
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<OtherProvider.OtherProvider>(criteria);
    }

    public async Task<IEnumerable<Provider.Provider>> GetProvidersByMatchId(int owpMatchId)
    {
        var criteria = new GetProvidersByMatchIdCriteria
        {
            OWPMatchId = owpMatchId
        };
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<Provider.Provider>(criteria);
    }

    public async Task<IEnumerable<Agent.Agent>> GetAgentsByMatchId(int owpMatchId)
    {
        var criteria = new GetAgentsByMatchIdCriteria
        {
            OWPMatchId = owpMatchId
        };
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<Agent.Agent>(criteria);
    }

    public async Task<IEnumerable<MatchStatusModel>> GetMatchStatusByMatchId(int owpMatchId)
    {
        var criteria = new GetMatchStatusByMatchIdCriteria
        {
            OWPMatchId = owpMatchId
        };
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<MatchStatusModel>(criteria);
    }

    public async Task<IEnumerable<MatchOfferModel>> GetMatchOfferByMatchId(int owpMatchId)
    {
        var criteria = new GetMatchOfferByMatchIdCriteria
        {
            OWPMatchId = owpMatchId
        };
        criteria.ThrowExceptionIfInvalid();

        return await repository.FetchAsync<MatchOfferModel>(criteria);
    }

    public async Task<IEnumerable<RefResourceModel>> GetAllByRefName(string refName)
    {
        var criteria = new GetRefResourceByNameCriteria
        {
            RefName = refName
        };

        var data = await repository.FetchAsync<ReferenceResource>(criteria);
        return data.Select(x => new RefResourceModel
        {
            RefName = refName,
            Id = x.ResourceId,
            Value = x.Value
        }).ToList();
    }

    public async Task<IsLiveStatusModel> GetMatchInfoForCheckIsLive(int matchId, int infoSiteMatchId)
    {
        var criteria = new ValidateInfoSiteMatchIdCriteria
        {
            MatchId = matchId,
            InfoSiteMatchId = infoSiteMatchId
        };

        return await repository.GetAsync<IsLiveStatusModel>(criteria);
    }
}