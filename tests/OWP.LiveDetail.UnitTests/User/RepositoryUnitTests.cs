namespace OWP.LiveDetail.UnitTests.User
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using LiveDetail.User;
    using NSubstitute;
    using Xunit;

    public class RepositoryUnitTests
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly UserRepository repository;

        public RepositoryUnitTests()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            repository = new UserRepositoryImp(dynamicRepository);
        }

        [Fact]
        public async Task GetAll_Always_ReturnsData()
        {
            // Arrange
            var userInfos = new List<UserInfoModel>();

            dynamicRepository.FetchAsync<UserInfoModel>(Arg.Compat.Any<GetUserListCriteria>()).Returns(userInfos);

            // Act
            var result = await repository.GetAll();

            // Assert
            Assert.Equal(userInfos, result);
        }

        [Fact]
        public async Task GetSuggestion_Always_ReturnsData()
        {
            // Arrange
            var userSuggestions = new List<UserSuggestionEntity>
            {
                 new UserSuggestionEntity
                 {
                      IsAssigned=false,
                      NickName="superman",
                      UserId=8,
                      UserName="sa"
                 }
            };

            dynamicRepository.FetchAsync<UserSuggestionEntity>(Arg.Compat.Any<GetSuggestionCriteria>()).Returns(userSuggestions);

            // Act
            var model = new GetSuggestionModel
            {
                OWPMatchId = 1,
                Keyword = "sa"
            };
            var result = await repository.GetSuggestion(model);

            // Assert
            Assert.Equal(userSuggestions, result);
        }

        [Fact]
        public async Task GetUserInfo_Name_ReturnUserInfo()
        {
            var userInfo = new UserInfoModel
            {
                NickName = "superman",
                UserName = "sa",
                UserId = 8,
                Phone = "123456789",
                Skype = "super",
                Extension = "123"
            };
            const string name = "sa";

            dynamicRepository.GetAsync<UserInfoModel>(Arg.Compat.Is<GetInfoByNameCriteria>(x => x.Name == name)).Returns(Task.FromResult(userInfo));

            // Act
            var result = await repository.GetUserInfo(name);

            // Assert
            Assert.Equal(userInfo, result);
        }
    }
}