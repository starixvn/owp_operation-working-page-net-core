using OWP.CQRS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace OWP.ChatServer.Handler;

public record GetChatServerRequest() : Command<object>;

public class GetChatServerHandler(
    [FromServices] IConfiguration configuration) : CommandHandler<GetChatServerRequest, object>
{
    public Task<object> Handle(
        GetChatServerRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult<object>(new
        {
            url = configuration["ChatServer:UIUrl"],
        });
    }
}