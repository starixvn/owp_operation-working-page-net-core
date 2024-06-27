using Fanex.NAP.Client.Configuration;
using OWP.Services;

namespace OWP.UserManagement.UnitTests;

using Criteria;
using Fanex.Data.Repository;
using Models;
using NSubstitute;
using OWP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Asserts.Compare;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly Repository repository;

    private readonly RefResourceService resourceRepository;

    public RepositoryUnitTests()
    {
        var napSettings = Substitute.For<INapSettings>();
        dynamicRepository = Substitute.For<IDynamicRepository>();
        resourceRepository = Substitute.For<RefResourceService>();
        repository = new RepositoryImp(dynamicRepository, napSettings, resourceRepository);
    }

    [Fact]
    public async Task GetUserPagingAsync_WhenCalled_DynamicRepositoryFetchAsyncCriteria()
    {
        var queryModel = new QueryModel
        {
            CategoryId = "1",
            User = "",
            DepartmentId = "1",
            LocationCode = "SMS",
            RoleId = "1",
            SectionId = "1"
        };


        // Arrange
        var expected = new List<User>
        {
            new() {UserId=1, UserName="sa" },
            new() {UserId=2, UserName="starix15" }
        };

        dynamicRepository.FetchAsync<User>(Arg.Compat.Any<QueryUsersCriteria>()).Returns(_ => expected);
        resourceRepository.GetRefResourcesByName(Arg.Compat.Any<string>()).Returns(_ => new List<ReferenceResource>
        {
            new() { Name="Language", Value="1", ResourceId=1}
        });
        // Act
        var actual = await repository.GetUserPagingAsync(queryModel);

        // Assert

        DeepAssert.Equal(expected, actual);

        await dynamicRepository.Received(1).FetchAsync<User>(Arg.Compat.Is<QueryUsersCriteria>(p =>
            p.CategoryId == queryModel.CategoryId &&
            p.DepartmentId == queryModel.DepartmentId &&
            p.User == queryModel.User &&
            p.RoleId == queryModel.RoleId &&
            p.SectionId == queryModel.SectionId

        ));
    }
}