namespace OWP.MaxbetSetting.UnitTests.Handlers
{
    using Fanex.Data.Repository;
    using NSubstitute;
    using OWP.MaxbetSetting.Criteria;
    using OWP.MaxbetSetting.Handlers;
    using OWP.MaxbetSetting.Models;
    using Xunit;
    using Xunit.Asserts.Compare;
    using AutoMapper;

    public class GetLogMaxbetSettingHandlerUnitTests
    {
        private IDynamicRepository dynamicRepository;

        private IMapper mapper;

        private GetLogMaxbetSettingHandler handler;

        public void Setup()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<LogModelDeserialize, MaxBetSettingModel>();
                cfg.CreateMap<LogModelDeserialize, MaxBetSettingModel>();
            });
            mapper = configuration.CreateMapper();
            handler = new GetLogMaxbetSettingHandler(dynamicRepository, mapper);
        }

        [Fact]
        public async Task GetLogMaxBetSetting_Always_Return()
        {
            Setup();
            // Arrange
            var request = new GetLogMaxbetSettingRequest();
            var log = new List<UserLogModel>()
            {
                new()
                {
                    Action = 128,
                    DataOld = "<r><z secondAccept_Corner='4.00'/></r>",
                    Data = "<r><z secondAccept_Corner='7'/></r>",
                    ModifiedDate = new DateTime(2022,09,05,23,52,09),
                    ModifiedBy = "sa"
                }
            };
            dynamicRepository.FetchAsync<UserLogModel>(Arg.Compat.Any<GetLogMaxBetSettingCriteria>()).Returns(log);
            var expected = new List<LogDisplayModel>()
            {
                new LogDisplayModel()
                {
                    Action = 128,
                    OldValue = new MaxBetLogSettingModel(){
                        SecondAccept_Corner = "4"
                    },
                    NewValue = new MaxBetLogSettingModel(){
                        SecondAccept_Corner = "7"
                    },
                    ModifyDate= "2022-09-05 23:52:09",
                    ModifyBy="sa"
                }
            };

            var actual = await handler.Handle(request, new CancellationToken(false));
            DeepAssert.Equals(expected[0].OldValue.SecondAccept_Corner, actual[0].OldValue.SecondAccept_Corner);
        }
    }
}