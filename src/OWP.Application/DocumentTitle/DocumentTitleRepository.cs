using Fanex.Data.Repository;

namespace OWP.Application.DocumentTitle;
public interface DocumentTitleRepository
{
    Task<int> CountUnreadMessage(CriteriaBase criteria);
}

public class DocumentTitleRepositoryImp : DocumentTitleRepository
{
    private readonly IDynamicRepository repository;

    public DocumentTitleRepositoryImp(IDynamicRepository repository)
    {
        this.repository = repository;
    }

    public async Task<int> CountUnreadMessage(CriteriaBase criteria)
    {
        var totalUnreadMessage = await repository.FetchAsync<int>(criteria);
        return totalUnreadMessage.FirstOrDefault();
    }
}
