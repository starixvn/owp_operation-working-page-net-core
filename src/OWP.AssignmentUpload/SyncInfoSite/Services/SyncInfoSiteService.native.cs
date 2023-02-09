
namespace OWP.AssignmentUpload.SyncInfoSite.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Validation.Models;

public interface SyncInfoSiteService
{
    Task SyncInfoMainMatch(List<AssignmentModel> owpMainMatch, DateTime eventDate);

    void SyncInfoMinorMatch(
        List<AssignmentModel> minorMatch,
        List<MainMatchBackupModel> mainMatchOriginModel,
        List<AssignmentModel> newMainMatch);
}

public class SyncInfoSiteIdServiceImp : SyncInfoSiteService
{
    private readonly InfositeMappingService mappingService;
    private readonly SyncInfoSiteRepository repository;

    public SyncInfoSiteIdServiceImp(
        SyncInfoSiteRepository repository,
        InfositeMappingService mappingService)
    {
        this.mappingService = mappingService;
        this.repository = repository;
    }

    public async Task SyncInfoMainMatch(List<AssignmentModel> owpMainMatch, DateTime eventDate)
    {
        var infoSiteMatches = await repository.QueryInfositeMatch(eventDate);
        mappingService.MapInfoSiteToMainMatch(owpMainMatch, infoSiteMatches);
    }

    public void SyncInfoMinorMatch(
        List<AssignmentModel> minorMatch,
        List<MainMatchBackupModel> mainMatchOriginModel,
        List<AssignmentModel> newMainMatch
    )
        => mappingService.MapMainInfoToMinor(minorMatch, mainMatchOriginModel, newMainMatch);
}