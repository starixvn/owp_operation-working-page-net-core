namespace OWP.Event.LiveGameAssignment.User;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Data.Repository;

public interface Repository
{
    Task<IEnumerable<UserInfo>> Query(DateTime eventDate);
}

public class RepositoryImp : Repository
{
    private readonly IDynamicRepository dynamicRepository;

    public RepositoryImp(IDynamicRepository dynamicRepository)
    {
        this.dynamicRepository = dynamicRepository;
    }

    public async Task<IEnumerable<UserInfo>> Query(DateTime eventDate)
    {
        var criteria = new GetUserCriteria
        {
            EventDate = eventDate
        };
        return await dynamicRepository.FetchAsync<UserInfo>(criteria);
    }
}
