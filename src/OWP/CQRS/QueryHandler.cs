using MediatR;

namespace OWP.CQRS;

public interface QueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
    where TQuery : Query<TResponse>
    where TResponse : notnull;