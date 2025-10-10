using OWP.Event.LiveGameAssignment.Download.Models;

namespace OWP.Event.LiveGameAssignment.Download.Service;

using Criteria;
using Extensions;
using Fanex.Data.Repository;
using Models;
using Repositories;
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
            EventDate = eventDate
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
        var events = (await eventAssignmentRepository.GetEventInformationByEventDate(criteria)).ToList();

        events = eventAssignmentExportFilter.Filter(events, model.PermissionModel, model.Username, model.SyncLiveStatus, model.MatchType, model.OffVRs);
        var maxNote = events.Any() ? events.Max(n => n.Notes.Count()) : 0;
        var writer = new AssignmentWriter(maxNote, model);

        var lstNoteMapped = await lstNoteMapperService.ApplyLstTNote(events);
        lstNoteMapped.WriteExcel(writer, file, typeof(EventModel));
    }
}
