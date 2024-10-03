using Fanex.NAP.Client;
using Microsoft.AspNetCore.Mvc;

namespace OWP.Application.DocumentTitle;

[Route("api/application/title")]
public class DocumentTitleController : ControllerBase
{
    private readonly DocumentTitleRepository repository;

    private readonly INapSession currentSession;

    public DocumentTitleController(DocumentTitleRepository repository, INapSession currentSession)
    {
        this.repository = repository;
        this.currentSession = currentSession;
    }

    [HttpGet]
    [Route("unread-message")]
    public async Task<int> CountUnreadMessage()
        => await repository.CountUnreadMessage(new CountUnreadMessageCriteria
        {
            UserId = currentSession.UserId
        });
}
