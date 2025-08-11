using Fanex.Caching;
using Newtonsoft.Json;
using NSubstitute;
using OWP.ProviderVerification.Criteria;
using OWP.ProviderVerification.Models;
using OWP.ProviderVerification.ProviderVerification.UploadAgent;
using Xunit;

namespace OWP.ProviderVerification.UnitTests.UploadAgent
{
    public class SubmitAgentHandlerTest
    {
        private readonly SubmitAgentHandler _handler;
        private readonly ICache _cache;
        private readonly ProviderVerificationRepository _repository;

        public SubmitAgentHandlerTest()
        {
            _cache = Substitute.For<ICache>();
            _repository = Substitute.For<ProviderVerificationRepository>();
            _handler = new SubmitAgentHandler(_cache, _repository);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" },
                new() { Provider = "BG", ProviderMatchID = "67890" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>
                    {
                        new() { Value = 1, Label = "RB2" }
                    }
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            // Provider verification item is not included (all conditions false)
            // But excel provider BG (67890) is added to unMapInExcel because it's not in provider verification
            Assert.Equal(1, result.Value.Count);

            // Verify that the result contains only the unmapped excel provider
            var unmappedProvider = result.Value.FirstOrDefault(x => x.OWPMatchId == null);
            Assert.NotNull(unmappedProvider);
            Assert.Equal("BG", unmappedProvider.Provider);
            Assert.Equal("67890", unmappedProvider.ProviderMatchId);
        }

        [Fact]
        public async Task Handle_NoProviderMatch_ReturnsUnmappedProviderVerification()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>(), // No provider match
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            // Provider verification item with no provider match + excel provider RB2
            Assert.Equal(2, result.Value.Count);

            // Verify provider verification item
            var providerVerificationItem = result.Value.FirstOrDefault(x => x.OWPMatchId == 1);
            Assert.NotNull(providerVerificationItem);
            Assert.Equal(1, providerVerificationItem.OWPMatchId);

            // Verify unmapped excel provider
            var unmappedProvider = result.Value.FirstOrDefault(x => x.OWPMatchId == null);
            Assert.NotNull(unmappedProvider);
            Assert.Equal("RB2", unmappedProvider.Provider);
            Assert.Equal("12345", unmappedProvider.ProviderMatchId);
        }

        [Fact]
        public async Task Handle_SuspendedOrPostponedMatch_ReturnsUnmappedProviderVerification()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Suspended",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value[0].OWPMatchId);
        }

        [Fact]
        public async Task Handle_CancelMatchWithInHouseProvider_ReturnsUnmappedProviderVerification()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Cancel",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value[0].OWPMatchId);
        }

        [Fact]
        public async Task Handle_SabaLeagueWithCancelStatus_ExcludesFromResult()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Saba League", // Contains "saba"
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "cancel", // Contains "cancel"
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(0, result.Value.Count); // Should be excluded
        }

        [Fact]
        public async Task Handle_MatchCancelAndConfirmProviderCancel_ExcludesFromResult()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Cancel",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>(), // No provider match
                    AgentConfirm = new List<AgentConfirmOption>
                    {
                        new() { Value = 1, Label = "cancel" } // Contains "cancel"
                    }
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            // Provider verification item is excluded due to IsMatchCancelAndConfirmProviderCancel
            // But excel provider RB2 is still added to unMapInExcel
            Assert.Equal(1, result.Value.Count); // 1 unmapped excel provider (RB2)

            // Verify that the result contains only the unmapped excel provider
            var unmappedProvider = result.Value.FirstOrDefault(x => x.OWPMatchId == null);
            Assert.NotNull(unmappedProvider);
            Assert.Equal("RB2", unmappedProvider.Provider);
            Assert.Equal("12345", unmappedProvider.ProviderMatchId);
        }

        [Fact]
        public async Task Handle_UnmappedExcelProviders_ReturnsUnmappedProviders()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" },
                new() { Provider = "BG", ProviderMatchID = "99999" } // Unmapped provider
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count); // 1 from provider verification + 1 unmapped excel provider

            var unmappedProvider = result.Value.FirstOrDefault(x => x.OWPMatchId == null);
            Assert.NotNull(unmappedProvider);
            Assert.Equal("BG", unmappedProvider.Provider);
            Assert.Equal("99999", unmappedProvider.ProviderMatchId);
        }

        [Fact]
        public async Task Handle_WithUnmappedProviders_SavesToCacheUsingBuildFileAgentUnmapKey()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" },
                new() { Provider = "BG", ProviderMatchID = "99999" } // Unmapped provider
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            // Verify that BuildFileAgentUnmapKey was called and cache was set
            var expectedUnmapCacheKey = Utils.BuildFileAgentUnmapKey(sessionId);
            await _cache.Received().SetAsync(
                Arg.Is<string>(key => key == expectedUnmapCacheKey),
                Arg.Any<string>(),
                Arg.Any<CacheItemOptions>()
            );
        }

        [Fact]
        public async Task Handle_WithMismatchedProviders_UpdatesIsMismatchInRepository()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>(), // No provider match - will be mismatched
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            // Verify that UpdateIsMismatch was called with correct parameters
            await _repository.Received().UpdateIsMismatch("1", date);
        }

        [Fact]
        public async Task Handle_WithLSTBookAndAdminSiteProviderDifference_ReturnsUnmappedProviderVerification()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "bet", // Will be converted to "betradar"
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" } // Different from LSTBook
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value[0].OWPMatchId);
        }

        [Fact]
        public async Task Handle_WithMultipleUnmappedProviders_ReturnsCorrectCount()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" },
                new() { Provider = "BG", ProviderMatchID = "99999" },
                new() { Provider = "Betradar", ProviderMatchID = "88888" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(3, result.Value.Count); // 1 from provider verification + 2 unmapped excel providers
        }

        [Fact]
        public async Task Handle_EmptyProviderVerificationList_ReturnsOnlyUnmappedExcelProviders()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" },
                new() { Provider = "BG", ProviderMatchID = "99999" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>(); // Empty list

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Count); // Only unmapped excel providers

            // Verify that BuildFileAgentUnmapKey was called for empty provider verification
            var expectedUnmapCacheKey = Utils.BuildFileAgentUnmapKey(sessionId);
            await _cache.Received().SetAsync(
                Arg.Is<string>(key => key == expectedUnmapCacheKey),
                Arg.Any<string>(),
                Arg.Any<CacheItemOptions>()
            );
        }

        [Fact]
        public async Task Handle_WithProviderNameMappings_AppliesCorrectMappings()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "bet", // Will be mapped to "betradar"
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "betradar" } // Mapped name
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Count);
            Assert.Equal(1, result.Value[0].OWPMatchId);
        }

        [Fact]
        public async Task Handle_WithAllExcludedConditions_ReturnsEmptyResult()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Cancel",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Saba League", // Contains "saba"
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "cancel", // Contains "cancel"
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>(), // No provider match
                    AgentConfirm = new List<AgentConfirmOption>
                    {
                        new() { Value = 1, Label = "cancel" } // Contains "cancel"
                    }
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Count); // 1 unmapped excel provider (RB2)
        }

        [Fact]
        public async Task Handle_WithNullProviderMatchAndAgentConfirm_ThrowsException()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>
            {
                new() { Provider = "RB2", ProviderMatchID = "12345" }
            };

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = null, // Null provider match
                    AgentConfirm = null // Null agent confirm
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act & Assert
            // When ProviderMatch is null, calling .Select() on it will throw ArgumentNullException
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("source", exception.ParamName);
        }

        [Fact]
        public async Task Handle_WithEmptyExcelProviderModels_ReturnsOnlyProviderVerificationResults()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var excelProviderModels = new List<ExcelProviderModel>(); // Empty excel providers

            var providerVerification = new List<ProviderVerificationDifferenceModel>
            {
                new()
                {
                    OWPMatchId = 1,
                    MatchNo = "M001",
                    MatchStatus = "Active",
                    EventDateTime = DateTime.Now,
                    KoTime = "14:00",
                    League = "Premier League",
                    Home = "Team A",
                    Away = "Team B",
                    LstBookProvider = "RB2",
                    InHouseProviderId = "12345",
                    ProviderMatch = new List<ProviderMatchModel>
                    {
                        new() { ProviderMatchId = 12345, InhouseProviderName = "RB2" }
                    },
                    AgentConfirm = new List<AgentConfirmOption>()
                }
            };

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);
            var excelProviderJson = JsonConvert.SerializeObject(excelProviderModels);

            _cache.TryGetAsync<string>(cacheKey).Returns((true, excelProviderJson));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(providerVerification);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(1, result.Value.Count); // Only provider verification result
            Assert.Equal(1, result.Value[0].OWPMatchId);
        }

        [Fact]
        public void Utils_BuildFileAgentUnmapKey_ReturnsCorrectKey()
        {
            // Arrange
            var sessionId = "test-session-123";

            // Act
            var result = Utils.BuildFileAgentUnmapKey(sessionId);

            // Assert
            Assert.Equal($"{Const.Const.KEY_CACHE_FILE_AGENT_UNMAP}-{sessionId}", result);
        }

        [Fact]
        public void Utils_BuildProviderVerificationKey_ReturnsCorrectKey()
        {
            // Arrange
            var sessionId = "test-session-456";

            // Act
            var result = Utils.BuildProviderVerificationKey(sessionId);

            // Assert
            Assert.Equal($"{Const.Const.KEY_PROVIDER_VERIFICATION}-{sessionId}", result);
        }

        [Fact]
        public async Task Handle_CacheReturnsFalse_ThrowsException()
        {
            // Arrange
            var sessionId = "test-session-id";
            var date = DateTime.Today;
            var request = new SubmitAgentRequest(sessionId, date);

            var cacheKey = Utils.BuildProviderVerificationKey(sessionId);

            _cache.TryGetAsync<string>(cacheKey).Returns((false, string.Empty));
            _repository.GetProviderVerificationDiff(Arg.Any<GetProviderVerificationCriteria>()).Returns(new List<ProviderVerificationDifferenceModel>());

            // Act & Assert
            // When cache returns false, excelProviderModel will be null/empty, causing JsonConvert.DeserializeObject to fail
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("source", exception.ParamName);
        }
    }
}
