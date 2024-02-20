using OWP.Event.LiveGameAssignment.Download.Models;

namespace OWP.Event.LiveGameAssignment.Download.Service;

using Fanex.Data.Repository;
using OWP.Event.LiveGameAssignment.Download.Criteria;
using OWP.Event.LiveGameAssignment.Download.Extensions;
using OWP.Event.LiveGameAssignment.Download.Models;
using OWP.Event.LiveGameAssignment.Download.Repositories;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EventModel = EventModel;

public interface IEventAssignmentService
{
    Task DownloadAssignment(DownLoadExportModel model, FileStream file);

    Task<AssignmentUploadHistoryModel> GetLastUpdated(DateTime eventDate);
}

public class EventAssignmentService : IEventAssignmentService
{
    private readonly IEventAssignmentRepository eventAssignmentRepository;
    private readonly IDynamicRepository dynamicRepository;
    private readonly EventAssignmentExportFilter eventAssignmentExportFilter;
    private readonly LstNoteMapperService lstNoteMapperService;

    public EventAssignmentService(
        IEventAssignmentRepository eventAssignmentRepository,
        IDynamicRepository dynamicRepository,
        EventAssignmentExportFilter eventAssignmentExportFilter,
        LstNoteMapperService lstNoteMapperService)
    {
        this.eventAssignmentRepository = eventAssignmentRepository;
        this.dynamicRepository = dynamicRepository;
        this.eventAssignmentExportFilter = eventAssignmentExportFilter;
        this.lstNoteMapperService = lstNoteMapperService;
    }

    public async Task<AssignmentUploadHistoryModel> GetLastUpdated(DateTime eventDate)
    {
        var criteria = new GetLastUpdatedCriteria
        {
            EventDate = eventDate,
        };

        var result = await dynamicRepository.GetAsync<AssignmentUploadHistoryModel>(criteria);
        return result ?? new AssignmentUploadHistoryModel();
    }

    public async Task DownloadAssignment(DownLoadExportModel model, FileStream file)
    {
        var criteria = new MatchGetAllCriteria
        {
            EventDate = model.EventDate,
            MatchStatusIds = model.MatchStatusIds
        };
        var eventInfos = (await eventAssignmentRepository.GetEventInformationByEventDate(criteria)).ToList();

        eventInfos = eventAssignmentExportFilter.Filter(eventInfos, model.PermissionModel, model.Username, model.SyncLiveStatus, model.MatchType, model.OffVRs);
        var maxNote = eventInfos.Any() ? eventInfos.Max(n => n.Notes.Count()) : 0;
        var writer = new AssignmentWriter(maxNote, model);

        var lstNoteMapped = await lstNoteMapperService.ApplyLstTNote(eventInfos);
        lstNoteMapped.WriteExcel(writer, file, typeof(EventModel));
    }
}
