namespace OWP.Services;

using Fanex.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

public interface ScopedDynamicRepository
{
    Task<int> ExecuteAsync(INonQueryCommand command);

    Task<IEnumerable<T>> FetchAsync<T>(ICriteria criteria);

    Task<(IEnumerable<TFirst> firstResultSet, IEnumerable<TSecond> secondResultSe)> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria);

    Task<(IEnumerable<TFirst> firstResultSet, IEnumerable<TSecond> secondResultSet, IEnumerable<TThird> thirdResultSet)> FetchMultipleAsync<TFirst, TSecond, TThird>(ICriteria criteria);
}

public class ScopedDynamicRepositoryImp : ScopedDynamicRepository
{
    private readonly IServiceProvider serviceProvider;

    public ScopedDynamicRepositoryImp(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<int> ExecuteAsync(INonQueryCommand command)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<IDynamicRepository>();

            return await scopedProcessingService.ExecuteAsync(command);
        }
    }

    public async Task<IEnumerable<T>> FetchAsync<T>(ICriteria criteria)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<IDynamicRepository>();

            return await scopedProcessingService.FetchAsync<T>(criteria);
        }
    }

    public async Task<(IEnumerable<TFirst> firstResultSet, IEnumerable<TSecond> secondResultSe)> FetchMultipleAsync<TFirst, TSecond>(ICriteria criteria)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<IDynamicRepository>();

            return await scopedProcessingService.FetchMultipleAsync<TFirst, TSecond>(criteria);
        }
    }

    public async Task<(IEnumerable<TFirst> firstResultSet, IEnumerable<TSecond> secondResultSet, IEnumerable<TThird> thirdResultSet)> FetchMultipleAsync<TFirst, TSecond, TThird>(ICriteria criteria)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedProcessingService =
                scope.ServiceProvider.GetRequiredService<IDynamicRepository>();

            return await scopedProcessingService.FetchMultipleAsync<TFirst, TSecond, TThird>(criteria);
        }
    }
}