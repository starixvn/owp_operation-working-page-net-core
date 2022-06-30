using Fanex.Data.Repository;
using MediatR;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Handlers;

public class GetStatusDropdownOptionHandler
{
    public class Request : IRequest<IList<DropdownOption>>
    {

    }

    public class Handler : IRequestHandler<Request, IList<DropdownOption>>
    {
        private readonly IDynamicRepository repository;

        public Handler(IDynamicRepository repository) => this.repository = repository;

        public async Task<IList<DropdownOption>> Handle(Request request, CancellationToken cancellationToken)
        {
            return new List<DropdownOption>
            {
                new DropdownOption{
                    Value=1,
                    Label= "Status 1",

                },
                 new DropdownOption{
                    Value=2,
                    Label= "Status 2",

                },
                 new DropdownOption{
                    Value=3,
                    Label= "Status 3",

                },
                 new DropdownOption{
                    Value=4,
                    Label= "Status 4"
                }
            };
        }
    }
}