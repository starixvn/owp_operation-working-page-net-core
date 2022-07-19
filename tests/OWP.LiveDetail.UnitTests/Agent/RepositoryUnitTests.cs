namespace OWP.LiveDetail.UnitTests.Agent
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using LiveDetail.Agent;
    using NSubstitute;
    using Xunit;

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
            var agents = new List<Agent> { new Agent { AgentId = 2, AgentName = "ag" } };
            dynamicRepository.FetchAsync<Agent>(Arg.Compat.Any<GetAllCriteria>()).Returns(agents);

            // Act
            var actual = await agentRepository.GetAll();

            // Assert
            Assert.Equal(agents, actual);
            await dynamicRepository.Received().FetchAsync<Agent>(Arg.Compat.Any<GetAllCriteria>());
        }
    }
}