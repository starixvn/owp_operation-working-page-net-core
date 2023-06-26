using Olympus.Xml;
using OWP.AutoAssignment.Criteria.InternalApi;
using OWP.AutoAssignment.Models.InternalApi;

namespace OWP.AutoAssignment.Services.InternalApi;

public interface LABuilderService
{
    Task Build(
        IEnumerable<ExportMatchModel> models,
        IEnumerable<AutoAssignmentResultModel> autoAssignModels);
}

public class LABuilderServiceImp : LABuilderService
{
    private readonly Repository repository;

    public LABuilderServiceImp(Repository repository)
    {
        this.repository = repository;
    }

    public async Task Build(
        IEnumerable<ExportMatchModel> models,
        IEnumerable<AutoAssignmentResultModel> autoAssignModels)
    {
        var lAQueryInfo = BuildLAExportQueryInfo(autoAssignModels);
        var lAExportInfo = await QueryLAExportInfo(lAQueryInfo);

        BuildExportLaModel(models, lAExportInfo.ToList());
    }

    private IEnumerable<LAExportQueryInfo> BuildLAExportQueryInfo(IEnumerable<AutoAssignmentResultModel> autoAssignModels)
        => autoAssignModels
            .Select(model => new LAExportQueryInfo
            {
                InfoSiteMatchId = model.InfoSiteMatchId,
                LAAssigneeId = model.UserId_LA,
                Bettype = model.Bettype
            }).ToList();

    private async Task<IEnumerable<LAExportInfo>> QueryLAExportInfo(IEnumerable<LAExportQueryInfo> queryInfo)
    {
        var criteria = new GetLAExportInfoCriteria
        {
            Xml = XmlConverter.Serialize(queryInfo)
        };

        return await repository.GetLAExportInfo(criteria);
    }

    private void BuildExportLaModel(IEnumerable<ExportMatchModel> exportModels,
        List<LAExportInfo> laExportModel)
    {
        if (exportModels == null) throw new ArgumentNullException(nameof(exportModels));
        foreach (var exportMatchModel in exportModels)
        {
            var laInfo = GetSingleLaInfoById(laExportModel, exportMatchModel);
            exportMatchModel.LA = laInfo?.UserName;
        }
    }

    private LAExportInfo GetSingleLaInfoById(IEnumerable<LAExportInfo> laExportModel, ExportMatchModel exportMatchModel)
    {
        var bettype = exportMatchModel.Bettype;
        var id = exportMatchModel.InfoSiteMatchId;
        return laExportModel
            .FirstOrDefault(x => x.InfoSiteMatchId == id && x.Bettype == bettype);
    }
}