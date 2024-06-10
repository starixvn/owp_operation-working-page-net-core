using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace OWP.CQRS;

public interface QueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
where TQuery : Query<TResponse>
where TResponse : notnull
{

}