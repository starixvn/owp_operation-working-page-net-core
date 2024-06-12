using MediatR;

namespace OWP.CQRS;

public interface Query<out TResponse> : IRequest<TResponse> where TResponse : notnull
{

}