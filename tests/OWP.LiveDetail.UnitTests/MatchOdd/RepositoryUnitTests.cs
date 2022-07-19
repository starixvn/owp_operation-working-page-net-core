namespace OWP.LiveDetail.UnitTests.MatchOdd
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Fanex.Data.Repository;
    using Fanex.NAP.Client.Helpers.Web;
    using KellermanSoftware.CompareNetObjects;
    using LiveDetail.MatchOdd;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class RepositoryUnitTests
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly INapSession currentSession;
        private readonly MatchOddRepository repository;

        public RepositoryUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = config.CreateMapper();
            dynamicRepository = Substitute.For<IDynamicRepository>();
            currentSession = Substitute.For<INapSession>();
            currentSession.UserId = 1;
            currentSession.UserName = "us";
            repository = new MatchOddRepositoryImp(mapper, dynamicRepository, currentSession);
        }

        [Fact]
        public async Task Query_Always_ReturnMatchOddList()
        {
            // Arrange
            var queryModel = new QueryModel { OWPMatchId = 22 };
            var matchOdds = new List<MatchOdd>
            {
                new MatchOdd { Id = 1, Bettype = 1, TeamChoice = 1, HDP = 1, Choice = 1, Odds = 1 }
            };

            dynamicRepository.FetchAsync<MatchOdd>(Arg.Compat.Any<QueryCriteria>()).Returns(matchOdds);

            // Act
            var actual = await repository.Query(queryModel);

            // Assert
            Assert.Equal(matchOdds, actual);
            await dynamicRepository.Received().FetchAsync<MatchOdd>(Arg.Compat.Is<QueryCriteria>(x => x.OWPMatchId == queryModel.OWPMatchId));
        }

        [Fact]
        public async Task Create_Always_ReturnResult()
        {
            // Arrange
            var model = new CreateModel
            {
                OWPMatchId = 1,
                Bettype = 1,
                TeamChoice = 1,
                Handicap = 1,
                RecommendOdds = new List<MatchOddUpdateRow>
                {
                    new MatchOddUpdateRow
                    {
                        Choice = 1,
                        Odds = 1
                    }
                }
            };
            const int commandResult = 0;
            var sessionId = string.Empty;
            dynamicRepository.ExecuteAsync(Arg.Compat.Any<CreateCriteria>()).Returns(returnThis: x =>
            {
                var criteria = x.Arg<CreateCriteria>();
                criteria.ErrMsg = "123";
                criteria.Id = 1;
                sessionId = criteria.SessionId;
                return Task.FromResult(commandResult);
            });

            // Act
            var result = await repository.Create(model);

            // Assert
            var expected = new CreateResult
            {
                Id = 1,
                Code = 0,
                Message = "123",
                SessionId = sessionId
            };
            DeepAssert.Equal(expected, result);
            var compareLogic = new CompareLogic();
            await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<CreateCriteria>(p =>
            compareLogic.Compare(
            new
            {
                p.UserId,
                p.OWPMatchId,
                p.Bettype,
                p.TeamChoice,
                p.Handicap,
                p.SessionId,
                p.Id,
                p.RecommendOdds
            },
            new
            {
                currentSession.UserId,
                model.OWPMatchId,
                model.Bettype,
                model.TeamChoice,
                model.Handicap,
                SessionId = sessionId,
                Id = 1,
                RecommendOdds = "<Root><Row Choice=\"1\" Odds=\"1\" /></Root>"
            }).AreEqual));
        }

        [Fact]
        public async Task Update_Always_ReturnResult()
        {
            // Arrange
            var model = new UpdateModel
            {
                Id = 1,
                OWPMatchId = 1,
                Bettype = 1,
                TeamChoice = 1,
                Handicap = 1,
                RecommendOdds = new List<MatchOddUpdateRow>
                {
                    new MatchOddUpdateRow
                    {
                        Choice = 1,
                        Odds = 1
                    }
                }
            };
            const int commandResult = 0;
            var sessionId = string.Empty;
            dynamicRepository.ExecuteAsync(Arg.Compat.Any<UpdateCriteria>()).Returns(returnThis: x =>
            {
                var criteria = x.Arg<UpdateCriteria>();
                sessionId = criteria.SessionId;
                criteria.ErrMsg = "123";
                return Task.FromResult(commandResult);
            });

            // Act
            var actual = await repository.Update(model);

            // Assert
            var expected = new ResponseResult
            {
                Code = 0,
                Message = "123",
                SessionId = sessionId
            };
            DeepAssert.Equal(expected, actual);
            var compareLogic = new CompareLogic();
            await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<UpdateCriteria>(p =>
                compareLogic.Compare(
            new
            {
                p.UserId,
                p.Id,
                p.TeamChoice,
                p.Handicap,
                p.SessionId,
                p.RecommendOdds
            },
            new
            {
                currentSession.UserId,
                model.Id,
                model.TeamChoice,
                model.Handicap,
                SessionId = sessionId,
                RecommendOdds = "<Root><Row Choice=\"1\" Odds=\"1\" /></Root>"
            }).AreEqual
            ));
        }

        [Fact]
        public async Task Delete_Always_ReturnResult()
        {
            // Arrange
            var model = new DeleteModel
            {
                Id = 1
            };
            const int commandResult = 0;
            var sessionId = string.Empty;
            dynamicRepository.ExecuteAsync(Arg.Compat.Any<DeleteCriteria>()).Returns(returnThis: x =>
            {
                var criteria = x.Arg<DeleteCriteria>();
                sessionId = criteria.SessionId;
                return Task.FromResult(commandResult);
            });

            // Act
            var actual = await repository.Delete(model);

            // Assert
            var expected = new ResponseResult
            {
                Code = 0,
                Message = "Ok",
                SessionId = sessionId
            };
            DeepAssert.Equal(expected, actual);
            var compareLogic = new CompareLogic();
            await dynamicRepository.Received().ExecuteAsync(Arg.Compat.Is<DeleteCriteria>(p =>
                compareLogic.Compare(
            new
            {
                p.UserId,
                p.Id,
                p.SessionId
            },
            new
            {
                currentSession.UserId,
                model.Id,
                SessionId = sessionId
            }).AreEqual
            ));
        }
    }
}
