using MediatR;

namespace OWP.CQRS;

public interface Command<out TResponse> : IRequest<TResponse>
{

}