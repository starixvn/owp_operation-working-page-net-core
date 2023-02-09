namespace OWP.AssignmentUpload.Validation.Services;

using Models;
using SyncInfoSite.Helper;
using SyncInfoSite.Services;

public interface InfoSiteMatchUpdater
{
    Task UpdateInfoSiteMatchToOwpMatch(List<AssignmentModel> owpMatch, DateTime eventDateTime);
}

public class InfoSiteMatchUpdaterImp : InfoSiteMatchUpdater
{
    private readonly SyncInfoSiteService syncInfoSiteService;

    private static readonly List<MainMatchBackupModel> mainMatchBackupModel = new();

    public InfoSiteMatchUpdaterImp(
        Repository repository,
        SyncInfoSiteService syncInfoSiteIdService)
    {
        syncInfoSiteService = syncInfoSiteIdService;
    }

    public async Task UpdateInfoSiteMatchToOwpMatch(List<AssignmentModel> owpMatch, DateTime eventDateTime)
    {
        var mainMatch = InfoSiteHelper.GetMainMatch(owpMatch);
        var minorMatch= InfoSiteHelper.GetMinorMatch(owpMatch);

        BackupMainAssignmentInfo(mainMatch);

        await syncInfoSiteService.SyncInfoMainMatch(mainMatch, eventDateTime);
        syncInfoSiteService.SyncInfoMinorMatch(minorMatch, mainMatchBackupModel, mainMatch);
    }

    public void BackupMainAssignmentInfo(List<AssignmentModel> mainMatch)
    {
        mainMatchBackupModel.Clear();
        mainMatch.ForEach(assignment =>
        {
            mainMatchBackupModel.Add(new MainMatchBackupModel
            {
                key = assignment.HashKey,
                League = assignment.League,
                Home = assignment.Home,
                Away = assignment.Away
            });
        });
    }
}