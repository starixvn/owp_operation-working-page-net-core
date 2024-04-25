
using OWP.NewLeagueSetting.Helper;

namespace OWP.NewLeagueSetting.Data;

public record InitDataRequest(Stream Stream) : IRequest<IList<LeagueSettingExcelInfo>>;
public class InitDataHandler : IRequestHandler<InitDataRequest, IList<LeagueSettingExcelInfo>>
{
    private readonly Repository repository;

    public InitDataHandler(Repository repository)
    {
        this.repository = repository;
    }

    public async Task<IList<LeagueSettingExcelInfo>> Handle(InitDataRequest request, CancellationToken cancellationToken)
    {
        var dataExcel = ReadExcel.LoadData(request.Stream);


    }

}