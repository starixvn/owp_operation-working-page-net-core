namespace OWP.LiveDetail.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OWP.LiveDetail.Agent;
    using NSubstitute;
    using Xunit;

    public class AgentControllerUnitTests
    {
        private readonly AgentRepository agentRepository;
        private readonly AgentController controller;

        public AgentControllerUnitTests()
        {
            agentRepository = Substitute.For<AgentRepository>();
            controller = new AgentController(agentRepository);
        }

        [Fact]
        public async Task GetAll_Always_ReturnAgentList()
        {
            // Arrange
            var agents = new List<Agent> { new Agent { AgentId = 1, AgentName = "an1" } };
            agentRepository.GetAll().Returns(agents);

            // Act
            var actual = await controller.GetAll();

            // Assert
            Assert.Equal(agents, actual);
        }
    }
}