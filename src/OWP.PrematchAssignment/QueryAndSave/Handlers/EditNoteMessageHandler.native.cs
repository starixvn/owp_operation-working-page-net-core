using MediatR;
using OWP.PrematchAssignment.Models;
using OWP.PrematchAssignment.Services;

namespace OWP.PrematchAssignment.Handlers;

public class EditNoteMessageRequest : IRequest<Note>
{
    public int PreMatchNoteMessageId { get; set; }

    public string Message { get; set; }
}

public class EditNoteMessageHandler : IRequestHandler<EditNoteMessageRequest, Note>
{
    private readonly NoteService noteService;

    public EditNoteMessageHandler(NoteService noteService)
    {
        this.noteService = noteService;
    }

    public async Task<Note> Handle(EditNoteMessageRequest request, CancellationToken cancellationToken)
    {
        return await noteService.EditNoteMessage(request);
    }
}