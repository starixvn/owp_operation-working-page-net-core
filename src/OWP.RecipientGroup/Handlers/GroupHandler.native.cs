namespace OWP.RecipientGroup.Handlers
{
    using MediatR;
    using Models;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class QueryRecipientGroupRequest : IRequest<IEnumerable<RecipientGroup>> { }

    public class GroupHandler : IRequestHandler<QueryRecipientGroupRequest, IEnumerable<RecipientGroup>>
    {
        public async Task<IEnumerable<RecipientGroup>> Handle(QueryRecipientGroupRequest request, CancellationToken cancellationToken)
        {
            return new List<RecipientGroup>
            {
                new () { GroupId = 1, GroupName = "LA", Recipients = new List<User>{
                    new () { UserId = 111, Username="Mangooooo"},
                    new () { UserId = 222, Username="Candy"},
                } },
                new () { GroupId = 2, GroupName = "EST", Recipients = new List<User>{
                    new () { UserId = 100, Username="Mangooooo"},
                    new () { UserId = 200, Username="Candy"},
                    new () { UserId = 300, Username="Candy"},
                } }
            };
        }
    }
}