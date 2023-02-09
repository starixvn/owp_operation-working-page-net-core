namespace OWP.PreMatchAssignment.Services;

using Criteria;
using Fanex.Data.Repository;
using Fanex.NAP.Client.Helpers.Web;
using Handlers;
using Models;

public interface NoteService
{
    Task<Note> CreateNewNote(CreateNewNoteRequest request);

    Task<Note> InsertNewNoteMessage(PostNewNoteMessageRequest request);

    Task<Note> EditNoteMessage(EditNoteMessageRequest request);
}

public class NoteServiceImp : NoteService
{
    private readonly INapSession napSession;

    private readonly IDynamicRepository dynamicRepo;

    public NoteServiceImp(INapSession napSession, IDynamicRepository dynamicRepo)
    {
        this.napSession = napSession;
        this.dynamicRepo = dynamicRepo;
    }

    public async Task<Note> CreateNewNote(CreateNewNoteRequest request)
    {
        var criteria = new InsertNewNoteCriteria
        {
            ActorId = napSession.UserId,
            EventDate = request.EventDate,
            InfoSiteMatchId = request.InfoSiteMatchId,
            Message = request.Message,
            OWPLeagueId = request.OWPLeagueId,
            CreatedDate = DateTime.Now
        };

        await dynamicRepo.ExecuteAsync(criteria);

        return new Note
        {
            CreatedDate = criteria.CreatedDate,
            Message = request.Message,
            PreMatchNoteId = criteria.PreMatchNoteId,
            PreMatchNoteMessageId = criteria.PreMatchNoteMessageId,
            Username = criteria.Username
        };
    }

    public async Task<Note> EditNoteMessage(EditNoteMessageRequest request)
    {
        var criteria = new EditNoteMessageCriteria
        {
            ActorId = napSession.UserId,
            Message = request.Message,
            PreMatchNoteMessageId = request.PreMatchNoteMessageId,
            CreatedDate = DateTime.Now
        };

        await dynamicRepo.ExecuteAsync(criteria);

        return new Note
        {
            CreatedDate = criteria.CreatedDate,
            Message = request.Message,
            Username = criteria.Username,
            PreMatchNoteMessageId = request.PreMatchNoteMessageId
        };
    }

    public async Task<Note> InsertNewNoteMessage(PostNewNoteMessageRequest request)
    {
        var criteria = new InsertNewNoteMessageCriteria
        {
            ActorId = napSession.UserId,
            Message = request.Message,
            PreMatchNoteId = request.PreMatchNoteId,
            CreatedDate = DateTime.Now
        };

        await dynamicRepo.ExecuteAsync(criteria);

        return new Note
        {
            CreatedDate = criteria.CreatedDate,
            Message = request.Message,
            PreMatchNoteId = criteria.PreMatchNoteId,
            PreMatchNoteMessageId = criteria.PreMatchNoteMessageId,
            Username = criteria.Username
        };
    }
}