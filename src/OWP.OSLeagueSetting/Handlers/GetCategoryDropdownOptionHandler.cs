using Fanex.Data.Repository;
using MediatR;
using OWP.OSLeagueSetting.Models;

namespace OWP.OSLeagueSetting.Handlers;

public class GetCategoryDropdownOptionHandler
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
                    Label= "Category 1",

                },
                 new DropdownOption{
                    Value=2,
                    Label= "Category 2",

                },
                 new DropdownOption{
                    Value=3,
                    Label= "Category 3",

                },
                 new DropdownOption{
                    Value=4,
                    Label= "Category 4"
                }
            };
        }
    }
}