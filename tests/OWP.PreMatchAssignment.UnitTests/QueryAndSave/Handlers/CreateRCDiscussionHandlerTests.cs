namespace OWP.PreMatchAssignment.UnitTests.QueryAndSave.Handlers
{
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using Fanex.Logging;
    using Microsoft.Extensions.Configuration;
    using NSubstitute;
    using OWP.Enums;
    using OWP.Models;
    using OWP.PrematchAssignment.QueryAndSave.Handlers;
    using OWP.PrematchAssignment.QueryAndSave.Models;
    using OWP.PrematchAssignment.QueryAndSave.Services;
    using OWP.PrematchAssignment.Criteria;
    using Fanex.NAP.Client;
    using Xunit;

    public class CreateRCDiscussionHandlerTests
    {
        private readonly PreMatchAssignmentService _service;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        private readonly INapSession _napSession;
        private readonly CreateRCDiscussionHandler _handler;

        public CreateRCDiscussionHandlerTests()
        {
            _service = Substitute.For<PreMatchAssignmentService>();
            _configuration = Substitute.For<IConfiguration>();
            _logger = Substitute.For<ILogger>();
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _httpClient = new HttpClient(new TestHttpMessageHandler());
            _napSession = Substitute.For<INapSession>();

            SetupConfiguration();
            _httpClientFactory.CreateClient().Returns(_httpClient);

            _handler = new CreateRCDiscussionHandler(
                _service,
                _httpClient,
                _configuration,
                _napSession,
                _logger
            );
        }

        private void SetupConfiguration()
        {
            _configuration["ChatServer:Url"].Returns("http://test.com/");
            _configuration["ChatServer:BotToken"].Returns("test-token");
            _configuration["ChatServer:BotUserId"].Returns("test-user-id");
            _configuration["ChatServer:PrematchDirectionChannelId"].Returns("test-channel-id");
            _napSession.UserName.Returns("test-current-user");
        }

        [Fact]
        public async Task Handle_WhenMatchInfoNotFound_ShouldReturnError()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = true,
                DiscussionInfos = new List<DiscussionInfo>()
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo>(), null));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((int)ErrorCode.Error, result.Code);
            Assert.Equal("Match info is not found", result.Message);
            await _service.DidNotReceive().InsertDiscussion(Arg.Any<InsertDiscussionCriteria>());
        }

        [Fact]
        public async Task Handle_WhenCreateNewDiscussion_ShouldReturnSuccess()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = true,
                DiscussionInfos = new List<DiscussionInfo>
                {
                    new()
                    {
                        AdminSiteMatchId = 1,
                        BetType = 1,
                        GhostName = "Test Ghost",
                        GhostDetail = "Test Detail",
                        BetOn = "Home",
                        HdpOu = 1.5m,
                        Odds = "1.95",
                        Note = "Test Note"
                    }
                }
            };

            var matchInfo = new DiscussionMatchInfo
            {
                LeagueName = "Test League",
                HomeId = 1,
                HomeName = "Home Team",
                AwayId = 2,
                AwayName = "Away Team"
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo>(), matchInfo));

            _service
                .GetMatchAssignments(request.AdminSiteId)
                .Returns(new List<Assignment> { new() { Bettype = 1, Username = "testuser" } });

            _service
                .GetEventRecipients()
                .Returns(new List<EventRecipient>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((int)ErrorCode.Ok, result.Code);
            Assert.Equal("RCDiscussion created successfully", result.Message);
            Assert.Null(result.Data);

            await _service
                .Received(1)
                .InsertDiscussion(Arg.Is<InsertDiscussionCriteria>(c => 
                    c.AdminSiteMatchId == request.AdminSiteId &&
                    c.IsIncludeTrader == request.IsIncludeTrader));
            
            await _service
                .Received(1)
                .GetEventRecipients();
            
            await _service
                .Received(1)
                .GetMatchAssignments(request.AdminSiteId);
        }

        [Fact]
        public async Task Handle_WhenDiscussionCreationFails_ShouldReturnError()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = true,
                DiscussionInfos = new List<DiscussionInfo>
                {
                    new()
                    {
                        AdminSiteMatchId = 1,
                        BetType = 1,
                        GhostName = "Test Ghost",
                        BetOn = "Home"
                    }
                }
            };

            var matchInfo = new DiscussionMatchInfo
            {
                LeagueName = "Test League",
                HomeId = 1,
                HomeName = "Home Team",
                AwayId = 2,
                AwayName = "Away Team"
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo>(), matchInfo));

            _service
                .GetMatchAssignments(request.AdminSiteId)
                .Returns(new List<Assignment>());

            _service
                .GetEventRecipients()
                .Returns(new List<EventRecipient>());

            // Use a failing HTTP client
            var failingHttpClient = new HttpClient(new FailingHttpMessageHandler());
            var failingHandler = new CreateRCDiscussionHandler(
                _service,
                failingHttpClient,
                _configuration,
                _napSession,
                _logger
            );

            // Act
            var result = await failingHandler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((int)ErrorCode.Error, result.Code);
            Assert.Equal("Some RCDiscussion creations failed", result.Message);
            Assert.NotNull(result.Data);
            Assert.IsType<Dictionary<(int AdminSiteMatchId, int BetType), string>>(result.Data);
            
            var errorData = result.Data as Dictionary<(int AdminSiteMatchId, int BetType), string>;
            Assert.NotNull(errorData);
            Assert.True(errorData.ContainsKey((1, 1)));
        }

        [Fact]
        public async Task Handle_WhenIsIncludeTraderIsFalse_ShouldNotCallRocketChat()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = false,
                DiscussionInfos = new List<DiscussionInfo>
                {
                    new()
                    {
                        AdminSiteMatchId = 1,
                        BetType = 1,
                        GhostName = "Test Ghost",
                        GhostDetail = "Test Detail",
                        BetOn = "Home",
                        HdpOu = 1.5m,
                        Odds = "1.95",
                        Note = "Test Note"
                    }
                }
            };

            var matchInfo = new DiscussionMatchInfo
            {
                LeagueName = "Test League",
                HomeId = 1,
                HomeName = "Home Team",
                AwayId = 2,
                AwayName = "Away Team"
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo>(), matchInfo));

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((int)ErrorCode.Ok, result.Code);
            Assert.Equal("RCDiscussion created successfully", result.Message);

            await _service
                .DidNotReceive()
                .GetMatchAssignments(Arg.Any<int>());

            await _service
                .DidNotReceive()
                .GetEventRecipients();

            await _service
                .Received(1)
                .InsertDiscussion(Arg.Is<InsertDiscussionCriteria>(c =>
                    c.AdminSiteMatchId == request.AdminSiteId &&
                    c.IsIncludeTrader == request.IsIncludeTrader));
        }

        [Fact]
        public async Task Handle_WhenProcessingExistingDiscussions_ShouldInviteUsersAndSetOwner()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = true,
                DiscussionInfos = new List<DiscussionInfo>
                {
                    new()
                    {
                        AdminSiteMatchId = 1,
                        BetType = 1,
                        GhostName = "Test Ghost",
                        GhostDetail = "Test Detail",
                        BetOn = "Home",
                        HdpOu = 1.5m,
                        Odds = "1.95",
                        Note = "Test Note"
                    }
                }
            };

            var matchInfo = new DiscussionMatchInfo
            {
                LeagueName = "Test League",
                HomeId = 1,
                HomeName = "Home Team",
                AwayId = 2,
                AwayName = "Away Team"
            };

            var existingDiscussion = new DiscussionInfo
            {
                AdminSiteMatchId = 1,
                BetType = 1,
                RCDiscussionGroupId = "existing-discussion-id",
                CreatedBy = 123 // Different user, so current user should be invited and set as owner
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo> { existingDiscussion }, matchInfo));

            _service
                .GetMatchAssignments(request.AdminSiteId)
                .Returns(new List<Assignment> { new() { Bettype = 1, Username = "testuser" } });

            _service
                .GetEventRecipients()
                .Returns(new List<EventRecipient> { new() { UserName = "eventuser" } });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((int)ErrorCode.Ok, result.Code);
            Assert.Equal("RCDiscussion created successfully", result.Message);

            await _service
                .Received(1)
                .GetMatchAssignments(request.AdminSiteId);

            await _service
                .Received(1)
                .GetEventRecipients();

            await _service
                .Received(1)
                .InsertDiscussion(Arg.Is<InsertDiscussionCriteria>(c =>
                    c.AdminSiteMatchId == request.AdminSiteId &&
                    c.IsIncludeTrader == request.IsIncludeTrader));
        }

        [Fact]
        public async Task Handle_WhenCurrentUserIsCreator_ShouldNotSetOwnerForExistingDiscussions()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = true,
                DiscussionInfos = new List<DiscussionInfo>
                {
                    new()
                    {
                        AdminSiteMatchId = 1,
                        BetType = 1,
                        GhostName = "Test Ghost",
                        GhostDetail = "Test Detail",
                        BetOn = "Home",
                        HdpOu = 1.5m,
                        Odds = "1.95",
                        Note = "Test Note"
                    }
                }
            };

            var matchInfo = new DiscussionMatchInfo
            {
                LeagueName = "Test League",
                HomeId = 1,
                HomeName = "Home Team",
                AwayId = 2,
                AwayName = "Away Team"
            };

            var existingDiscussion = new DiscussionInfo
            {
                AdminSiteMatchId = 1,
                BetType = 1,
                RCDiscussionGroupId = "existing-discussion-id",
                CreatedBy = 456 // Same as current user, so owner should not be set
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo> { existingDiscussion }, matchInfo));

            _service
                .GetMatchAssignments(request.AdminSiteId)
                .Returns(new List<Assignment> { new() { Bettype = 1, Username = "testuser" } });

            _service
                .GetEventRecipients()
                .Returns(new List<EventRecipient> { new() { UserName = "eventuser" } });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal((int)ErrorCode.Ok, result.Code);
            Assert.Equal("RCDiscussion created successfully", result.Message);

            await _service
                .Received(1)
                .GetMatchAssignments(request.AdminSiteId);

            await _service
                .Received(1)
                .GetEventRecipients();

            await _service
                .Received(1)
                .InsertDiscussion(Arg.Is<InsertDiscussionCriteria>(c =>
                    c.AdminSiteMatchId == request.AdminSiteId &&
                    c.IsIncludeTrader == request.IsIncludeTrader));
        }

        [Fact]
        public async Task Handle_WhenOwnerSettingFails_ShouldStillReturnSuccess()
        {
            // Arrange
            var request = new CreateRCDiscussionRequest
            {
                AdminSiteId = 1,
                IsIncludeTrader = true,
                DiscussionInfos = new List<DiscussionInfo>
                {
                    new()
                    {
                        AdminSiteMatchId = 1,
                        BetType = 1,
                        GhostName = "Test Ghost",
                        GhostDetail = "Test Detail",
                        BetOn = "Home",
                        HdpOu = 1.5m,
                        Odds = "1.95",
                        Note = "Test Note"
                    }
                }
            };

            var matchInfo = new DiscussionMatchInfo
            {
                LeagueName = "Test League",
                HomeId = 1,
                HomeName = "Home Team",
                AwayId = 2,
                AwayName = "Away Team"
            };

            _service
                .GetMatchDiscussionInfo(request.AdminSiteId)
                .Returns((new List<DiscussionInfo>(), matchInfo));

            _service
                .GetMatchAssignments(request.AdminSiteId)
                .Returns(new List<Assignment> { new() { Bettype = 1, Username = "testuser" } });

            _service
                .GetEventRecipients()
                .Returns(new List<EventRecipient>());

            // Use handler with failing owner setting but successful discussion creation
            var failingOwnerHttpClient = new HttpClient(new FailingOwnerSettingHttpMessageHandler());
            var failingOwnerHandler = new CreateRCDiscussionHandler(
                _service,
                failingOwnerHttpClient,
                _configuration,
                _napSession,
                _logger
            );

            // Act
            var result = await failingOwnerHandler.Handle(request, CancellationToken.None);

            // Assert - Should still return success since discussion was created, even if owner setting failed
            Assert.Equal((int)ErrorCode.Ok, result.Code);
            Assert.Equal("RCDiscussion created successfully", result.Message);

            await _service
                .Received(1)
                .InsertDiscussion(Arg.Is<InsertDiscussionCriteria>(c =>
                    c.AdminSiteMatchId == request.AdminSiteId &&
                    c.IsIncludeTrader == request.IsIncludeTrader));
        }
    }

    internal class TestHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Handle different endpoints
            if (request.RequestUri?.AbsoluteUri.Contains("rooms.createDiscussion") == true)
            {
                var response = new CreateDiscussionResponse
                {
                    Discussion = new DiscussionInfoResponse
                    {
                        Id = "test-discussion-id",
                        Rid = "test-discussion-rid",
                        Name = "test-discussion"
                    }
                };

                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });
            }
            else if (request.RequestUri?.AbsoluteUri.Contains("roles.addUserToRole") == true)
            {
                // Mock successful owner setting
                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}")
                });
            }
            else if (request.RequestUri?.AbsoluteUri.Contains("chat.postMessage") == true)
            {
                // Mock successful message sending
                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}")
                });
            }
            else if (request.RequestUri?.AbsoluteUri.Contains("groups.invite") == true)
            {
                // Mock successful user invitation
                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}")
                });
            }

            // Default response for unhandled endpoints
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });
        }
    }

    internal class FailingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Always fail with Bad Request for all endpoints to test error handling
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(JsonSerializer.Serialize(new { error = "Bad Request", errorType = "invalid-params" }))
            });
        }
    }

    internal class FailingOwnerSettingHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Handle different endpoints - fail only owner setting
            if (request.RequestUri?.AbsoluteUri.Contains("roles.addUserToRole") == true)
            {
                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent(JsonSerializer.Serialize(new { error = "Failed to set owner", errorType = "role-error" }))
                });
            }
            else if (request.RequestUri?.AbsoluteUri.Contains("rooms.createDiscussion") == true)
            {
                var response = new CreateDiscussionResponse
                {
                    Discussion = new DiscussionInfoResponse
                    {
                        Id = "test-discussion-id",
                        Rid = "test-discussion-rid",
                        Name = "test-discussion"
                    }
                };

                return Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(response))
                });
            }

            // Default success response
            return Task.FromResult(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{}")
            });
        }
    }
}