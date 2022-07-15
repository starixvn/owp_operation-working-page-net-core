namespace OWP.LiveDetail.UnitTests
{
    using Fanex.Data.Repository;
    using LiveDetail.MatchInfo;
    using Models;
    using NSubstitute;
    using OWP.LiveDetail.Common;
    // using OWP.RabbitMQ;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Xunit;
    using AgentSpace = LiveDetail.Agent;
    using OtherProviderSpace = LiveDetail.OtherProvider;
    using ProviderSpace = LiveDetail.Provider;
    using TelevisionSpace = LiveDetail.Television;

    public class MatchInfoControllerUnitTests
    {
        private readonly MatchInfoRepository repository;
        // private readonly NotificationService notificationService;
        private readonly PermissionService permissionService;
        private readonly LiveDetailCacheManipulator cacheManipulator;
        private readonly CheckIsLiveStatusServices checkIsLiveStatusServices;
        private readonly GetAdminSiteMatchIdService getAdminSiteMatchIdService;
        private readonly MatchInfoController controller;
        // private readonly RabbitMqClient rabbitMqClient;

        public MatchInfoControllerUnitTests()
        {
            repository = Substitute.For<MatchInfoRepository>();
            Substitute.For<IDynamicRepository>();
            // notificationService = Substitute.For<NotificationService>();
            permissionService = Substitute.For<PermissionService>();
            cacheManipulator = Substitute.For<LiveDetailCacheManipulator>();
            checkIsLiveStatusServices = Substitute.For<CheckIsLiveStatusServices>();
            getAdminSiteMatchIdService = Substitute.For<GetAdminSiteMatchIdService>();
            // rabbitMqClient = Substitute.For<RabbitMqClient>();
            controller = new MatchInfoController(
                repository,
                // notificationService,
                permissionService,
                cacheManipulator,
                checkIsLiveStatusServices,
                getAdminSiteMatchIdService
                // rabbitMqClient
                );
        }

        [Fact]
        public async Task Query_Always_ReturnsData()
        {
            // Arrange
            var matchInfo = new MatchInfoModel
            {
                HomeName = "Home",
                AwayName = "Away"
            };
            repository.Query(Arg.Compat.Any<int>()).Returns(Task.FromResult(matchInfo));

            IEnumerable<TelevisionSpace.Television> tvs = new List<TelevisionSpace.Television>
            {
                new TelevisionSpace.Television { TVId = 1, TVCode = "TV", Color = "#444" }
            };
            repository.GetTelevisionsByMatchId(Arg.Compat.Any<int>()).Returns(Task.FromResult(tvs));

            IEnumerable<OtherProviderSpace.OtherProvider> otherProviders = new List<OtherProviderSpace.OtherProvider>
            {
                new OtherProviderSpace.OtherProvider { OtherProviderId = 1, OtherProviderName = "otherProvider" }
            };
            repository.GetOtherProvidersByMatchId(Arg.Compat.Any<int>()).Returns(Task.FromResult(otherProviders));

            IEnumerable<ProviderSpace.Provider> providers = new List<ProviderSpace.Provider>
            {
                new ProviderSpace.Provider { ProviderId = 1, ProviderName = "provider" }
            };
            repository.GetProvidersByMatchId(Arg.Compat.Any<int>()).Returns(Task.FromResult(providers));

            IEnumerable<AgentSpace.Agent> agents = new List<AgentSpace.Agent>
            {
                new AgentSpace.Agent { AgentId = 1, AgentName = "Agent" }
            };
            repository.GetAgentsByMatchId(Arg.Compat.Any<int>()).Returns(Task.FromResult(agents));

            IEnumerable<MatchStatusModel> matchStatusList = new List<MatchStatusModel>
            {
                new MatchStatusModel { Value = "1", DisplayText = "Live" }
            };
            repository.GetMatchStatusByMatchId(Arg.Compat.Any<int>()).Returns(Task.FromResult(matchStatusList));

            IEnumerable<RefResourceModel> noOfHDPs = new List<RefResourceModel>
            {
                new RefResourceModel {Id = 1, IsSelected = false, RefName = "aaa", Value = "bbb" }
            };
            repository.GetAllByRefName(Arg.Compat.Is<string>(x => x.Equals("NoOfHDP"))).Returns(Task.FromResult(noOfHDPs));

            // Act
            var result = await controller.Query(1);

            // Assert
            Assert.Equal(matchInfo, result);
            Assert.Equal(tvs, result.TVs);
            Assert.Equal(otherProviders, result.OtherProviders);
            Assert.Equal(providers, result.Providers);
            Assert.Equal(agents, result.Agents);
            Assert.Equal(matchStatusList, result.MatchStatusList);
            Assert.Equal(noOfHDPs, result.NoOfHDPs);
        }

        [Fact]
        public async Task UpdateMatchInfo_ValidateSuccessfulAndUpdateSuccessful_DoNotify()
        {
            // Arrange
            var model = new UpdateMatchInfoModel
            {
                KickOffTime = new DateTime(2021, 7, 12, 15, 0, 0)
            };

            var updateResult = new ResultDataObject
            {
                Code = 0
            };

            repository.UpdateMatchInfo(Arg.Compat.Any<UpdateMatchInfoModel>()).Returns(Task.FromResult(updateResult));

            // Act
            var result = await controller.UpdateMatchInfo(model);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            await repository.Received().UpdateMatchInfo(Arg.Compat.Any<UpdateMatchInfoModel>());
            //await notificationService.Received().Notify(Arg.Compat.Any<string>());
        }
    }
}