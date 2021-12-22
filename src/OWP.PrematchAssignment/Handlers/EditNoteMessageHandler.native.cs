namespace OWP.PreMatchAssignment.Handlers
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;
    using Services;

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
}