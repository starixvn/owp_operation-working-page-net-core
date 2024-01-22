using OWP.Services;

namespace OWP.Event.LiveGameAssignment.Download;

using Fanex.NAP.Client.Attributes;
using Fanex.NAP.Client.Helpers.Business;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OWP.Constants;
using OWP.Event.LiveGameAssignment.Download.Models;
using OWP.Event.LiveGameAssignment.Download.Service;
using OWP.Event.LiveGameAssignment.Services;
using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

[Route("api/event-assignment/export")]
[ApiController]
[EnableCors]
[AllowAnonymous]
public class DownloadAssignmentController : ControllerBase
{
    private readonly IEventAssignmentService eventAssignmentService;
    private readonly EventAssignmentPermission eventAssignmentPermission;
    private readonly AccessCounter accessCounter;

    public DownloadAssignmentController(IEventAssignmentService eventAssignmentService,
        EventAssignmentPermission eventAssignmentPermission, AccessCounter accessCounter)
    {
        this.eventAssignmentService = eventAssignmentService;
        this.eventAssignmentPermission = eventAssignmentPermission;
        this.accessCounter = accessCounter;
    }

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
    public async Task<FileResult> DownloadLiveGameAssignment(DateTime date, string userName, string matchStatusIds, int syncLiveStatus, string matchType, string offVRs)
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
        await accessCounter.UpdateCounter(ConfigurationManager.AppSettings["LiveGameAssignment_Export"]);
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
