using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace OWP.CQRS
{
    public interface Command<out TResponse> : IRequest<TResponse>
    {

    }
}