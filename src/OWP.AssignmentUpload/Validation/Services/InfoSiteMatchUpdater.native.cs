namespace OWP.AssignmentUpload.Validation.Services;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Models;
using SyncInfoSite.Services;
using System;
using SyncInfoSite.Helper;

public interface InfoSiteMatchUpdater
{
    Task UpdateInfoSiteMatchToOwpMatch(List<AssignmentModel> owpMatch, DateTime eventDateTime);
}

public class InfoSiteMatchUpdaterImp : InfoSiteMatchUpdater
{
    private readonly SyncInfoSiteService syncInfoSiteService;

    private static List<MainMatchBackupModel> mainMatchBackupModel = new List<MainMatchBackupModel>();

    public InfoSiteMatchUpdaterImp(
        Repository repository,
        SyncInfoSiteService syncInfoSiteIdService)
    {
        this.syncInfoSiteService = syncInfoSiteIdService;
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