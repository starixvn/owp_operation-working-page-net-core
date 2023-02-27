using Fanex.Data.Repository;
using NSubstitute;
using OWP.LiveDetail.Agent;
using Xunit;

namespace OWP.LiveDetail.UnitTests.Agent;

public class RepositoryUnitTests
{
    private readonly IDynamicRepository dynamicRepository;
    private readonly AgentRepository agentRepository;

    public RepositoryUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        agentRepository = new AgentRepositoryImp(dynamicRepository);
    }

    [Fact]
    public async Task GetAll_Always_ReturnAgentList()
    {
        // Arrange
        var agents = new List<LiveDetail.Agent.Agent> { new() { AgentId = 2, AgentName = "ag" } };
        dynamicRepository.FetchAsync<LiveDetail.Agent.Agent>(Arg.Compat.Any<GetAllCriteria>()).Returns(agents);

        // Act
        var actual = await agentRepository.GetAll();

        // Assert
        Assert.Equal(agents, actual);
        await dynamicRepository.Received().FetchAsync<LiveDetail.Agent.Agent>(Arg.Compat.Any<GetAllCriteria>());
    }
}