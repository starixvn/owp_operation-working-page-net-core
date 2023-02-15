namespace OWP.MaxbetSetting.Handlers;

using AutoMapper;
using Criteria;
using Fanex.Data.Repository;
using Helpers;
using MediatR;
using Models;

public class GetLogMaxbetSettingRequest : IRequest<IList<LogDisplayModel>>
{
    public int FT_HDP { get; set; }
}

public class GetLogMaxbetSettingHandler : IRequestHandler<GetLogMaxbetSettingRequest, IList<LogDisplayModel>>
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly IMapper mapper;

    public GetLogMaxbetSettingHandler(IDynamicRepository dynamicRepository, IMapper mapper)
    {
        this.dynamicRepository = dynamicRepository;
        this.mapper = mapper;
    }

    public async Task<IList<LogDisplayModel>> Handle(GetLogMaxbetSettingRequest request, CancellationToken cancellationToken)
    {
        var criteria = new GetLogMaxBetSettingCriteria
        {
            FT_HDP = request.FT_HDP
        };

        var maxBetSettingLog = await dynamicRepository.FetchAsync<UserLogModel>(criteria);

        var result = LogBuilder.Build(maxBetSettingLog.OrderByDescending(x => x.ModifiedDate).ToList(), mapper);
        return result;
    }
}
