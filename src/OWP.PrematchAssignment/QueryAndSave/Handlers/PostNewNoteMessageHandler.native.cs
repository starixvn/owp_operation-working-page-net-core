using MediatR;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;

namespace OWP.PrematchAssignment.QueryAndSave.Handlers;

public class PostNewNoteMessageRequest : IRequest<Note>
{
    public int PreMatchNoteId { get; set; }

    public string Message { get; set; }
}

public class PostNewNoteMessageHandler : IRequestHandler<PostNewNoteMessageRequest, Note>
{
    private readonly NoteService noteService;

    public PostNewNoteMessageHandler(NoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<Note> Handle(PostNewNoteMessageRequest request, CancellationToken cancellationToken)
    {
        return await noteService.InsertNewNoteMessage(request);
    }
}