using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using MediatR;
using OWP.MaxbetSetting.Criteria;
using OWP.MaxbetSetting.Models;
using OWP.MaxbetSetting.Helpers;
using AutoMapper;

namespace OWP.MaxbetSetting.Handlers;

public class GetLogMaxbetSettingRequest : IRequest<IList<MaxBetLogSetting>>
{
    public int FT_HDP { get; set; }
}

public class GetLogMaxbetSettingHandler : IRequestHandler<GetLogMaxbetSettingRequest, IList<MaxBetLogSetting>>
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly IMapper mapper;

    public GetLogMaxbetSettingHandler(IDynamicRepository dynamicRepository, IMapper mapper)
    {
        this.dynamicRepository = dynamicRepository;
        this.mapper = mapper;
    }

    public async Task<IList<MaxBetLogSetting>> Handle(GetLogMaxbetSettingRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetLogMaxBetSettingCriteria
        {
            FT_HDP = request.FT_HDP
        };

        var maxBetSettingLog = await dynamicRepository.FetchAsync<UserLogModel>(criteria);

        var result = LogBuilder.Build(maxBetSettingLog.ToList(), mapper);
        return result;
    }
}
