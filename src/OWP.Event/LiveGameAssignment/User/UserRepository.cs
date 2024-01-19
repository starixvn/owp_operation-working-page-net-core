namespace OWP.Event.LiveGameAssignment.User;

using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface UserRepository
{
    Task<IEnumerable<UserInfo>> Query(DateTime eventDate);
}

public class UserRepositoryImp : UserRepository
{
    private readonly IDynamicRepository dynamicRepository;

    public UserRepositoryImp(IDynamicRepository dynamicRepository)
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
