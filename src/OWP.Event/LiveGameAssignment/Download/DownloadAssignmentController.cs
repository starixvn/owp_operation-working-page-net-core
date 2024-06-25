using OWP.Services;

namespace OWP.Event.LiveGameAssignment.Download;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OWP.Constants;
using Models;
using Service;
using Services;
using OWP.Models;
using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

[Route("api/event-assignment/export")]
[ApiController]
[AllowAnonymous]
public class DownloadAssignmentController(IEventAssignmentService eventAssignmentService,
    EventAssignmentPermission eventAssignmentPermission,
    AccessCounter accessCounter,
    IOptions<AccessCounterModel> counterOptions) : ControllerBase
{
   
    private readonly AccessCounterModel counterModel = counterOptions.Value;

   [HttpGet("download-assignment")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Host)]
    public async Task<FileResult> DownloadAssignment(DateTime date)
    {
        var lastUpdated = await eventAssignmentService.GetLastUpdated(date);
        var eventDownLoadModel = new DownLoadExportModel
        {
            EventDate = date,
            Username = string.Empty,
            MatchStatusIds = string.Empty,
            UploadDateTime = lastUpdated.UploadDateTime,
            PermissionModel = eventAssignmentPermission.BuildPermission()
        };
        return await DownLoadExport(eventDownLoadModel);
    }

    [HttpGet("download-live-game")]
    [NapAuth(FunctionKey = FunctionKeys.LiveGameAssignment, Policy = Constant.Policy.Read)]
    public async Task<IActionResult> DownloadLiveGameAssignment(DateTime date, string userName, string matchStatusIds, int syncLiveStatus, string matchType, string offVRs)
    {
        var lastUpdated = await eventAssignmentService.GetLastUpdated(date);
        var eventExportModel = new DownLoadExportModel
        {
            EventDate = date,
            Username = userName,
            MatchStatusIds = matchStatusIds,
            UploadDateTime = lastUpdated.UploadDateTime,
            SyncLiveStatus = syncLiveStatus,
            PermissionModel = eventAssignmentPermission.BuildPermission(),
            MatchType = matchType,
            OffVRs = offVRs
        };
        await accessCounter.UpdateCounter(counterModel.LiveGameAssignment_Export);
        return await DownLoadExport(eventExportModel);
    }

    public async Task<FileResult> DownLoadExport(DownLoadExportModel model)
    {
        using (var tmpFiles = new TempFileCollection())
        {
            var file = tmpFiles.AddExtension("xlsx");
            using (var fileStream = System.IO.File.Open(file, FileMode.OpenOrCreate))
            {
                await eventAssignmentService.DownloadAssignment(model, fileStream);
            }

            var fileName = string.Format(CultureInfo.InvariantCulture, "Assignment - {0}.xlsx", model.EventDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            return File(System.IO.File.Open(file, FileMode.OpenOrCreate), "application/vnd.ms-excel", fileName);
        }
    }
}
