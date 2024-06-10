using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace OWP.CQRS;

public interface Query<out TResponse> : IRequest<TResponse> where TResponse : notnull
{

}