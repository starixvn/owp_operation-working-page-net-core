using MediatR;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

public class CreateNewNoteRequest : IRequest<Note>
{
    public int? OWPLeagueId { get; set; }

    public int? InfoSiteMatchId { get; set; }

    public string Message { get; set; }

    public DateTime EventDate { get; set; }
}

public class CreateNewNoteHandler : IRequestHandler<CreateNewNoteRequest, Note>
{
    private readonly NoteService noteService;

    public CreateNewNoteHandler(NoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<Note> Handle(CreateNewNoteRequest request, CancellationToken cancellationToken)
    {
        return await noteService.CreateNewNote(request);
    }
}