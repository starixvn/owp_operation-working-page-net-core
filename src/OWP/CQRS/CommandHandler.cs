using MediatR;

namespace OWP.CQRS;

public interface CommandHandler<in TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
where TRequest : Command<TResponse>
where TResponse : notnull
{

}