using Fanex.Data.Repository;
using MediatR;
using OWP.OSBasketballMaxbetSetting.Models;

namespace OWP.OSBasketballMaxbetSetting.Handlers;

public class GetRoundDropdownOptionHandler
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
                    Label= "Round 1",

                },
                 new DropdownOption{
                    Value=2,
                    Label= "Round 2",

                },
                 new DropdownOption{
                    Value=3,
                    Label= "Round 3",

                },
                 new DropdownOption{
                    Value=4,
                    Label= "Round 4"
                }
            };
        }
    }
}