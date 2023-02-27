using Olympus.Xml;
using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment.Services.InternalApi;

public interface LABuilderService
{
    Task Build(
        IList<ExportMatchModel> models,
        IList<AutoAssignmentResultModel> autoAssignModels);
}

public class LABuilderServiceImp : LABuilderService
{
    private readonly Repository repository;

    public LABuilderServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task Build(
        IList<ExportMatchModel> models,
        IList<AutoAssignmentResultModel> autoAssignModels)
    {
        var lAQueryInfo = BuildLAExportQueryInfo(autoAssignModels);
        var lAExportInfo = await QueryLAExportInfo(lAQueryInfo);

        BuildExportLaModel(models, lAExportInfo.ToList());
    }

    private IList<LAExportQueryInfo> BuildLAExportQueryInfo(IList<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
            .Select(model => new LAExportQueryInfo
            {
                InfoSiteMatchId = model.InfoSiteMatchId,
                LAAssingeeId = model.UserId_LA,
                Bettype = model.Bettype
            }).ToList();

    private async Task<IEnumerable<LAExportInfo>> QueryLAExportInfo(IList<LAExportQueryInfo> queryInfo)
    {
        var criteria = new GetLAExportInfoCriteria
        {
            Xml = XmlConverter.Serialize(queryInfo)
        };

        return await repository.GetLAExportInfo(criteria);
    }

    private void BuildExportLaModel(IList<ExportMatchModel> exportModels,
        IList<LAExportInfo> laExportModel)
    {
        foreach (var exportMatchModel in exportModels)
        {
            var laInfo = GetSingleLaInfoById(laExportModel, exportMatchModel);
            exportMatchModel.LA = laInfo?.UserName;
        }
    }

    private LAExportInfo GetSingleLaInfoById(IList<LAExportInfo> laExportModel, ExportMatchModel exportMatchModel)
    {
        var bettype = exportMatchModel.Bettype;
        var id = exportMatchModel.InfoSiteMatchId;
        return laExportModel
            .FirstOrDefault(x => x.InfoSiteMatchId == id && x.Bettype == bettype);
    }
}