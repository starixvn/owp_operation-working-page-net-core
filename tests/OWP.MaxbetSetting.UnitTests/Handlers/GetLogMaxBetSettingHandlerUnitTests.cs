namespace OWP.MaxbetSetting.UnitTests.Handlers;

using AutoMapper;
using Criteria;
using Fanex.Data.Repository;
using MaxbetSetting.Handlers;
using Models;
using NSubstitute;
using Xunit;

public class GetLogMaxbetSettingHandlerUnitTests
{
    private readonly IDynamicRepository dynamicRepository;

    private readonly IMapper mapper;

    private readonly GetLogMaxbetSettingHandler handler;

    public GetLogMaxbetSettingHandlerUnitTests()
    {
        dynamicRepository = Substitute.For<IDynamicRepository>();
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<LogModelDeserialize, MaxBetLogSettingModel>();
        });
        mapper = configuration.CreateMapper();

        handler = new GetLogMaxbetSettingHandler(dynamicRepository, mapper);
    }

    [Fact]
    public async Task GetLogMaxBetSetting_Always_Return()
    {
        // Arrange
        var request = new GetLogMaxbetSettingRequest();
        var log = new List<UserLogModel>
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
        var expected = new List<LogDisplayModel>
        {
            new()
            {
                Action = 128,
                OldValue = new MaxBetLogSettingModel
                {
                    SecondAccept_Corner = "4"
                },
                NewValue = new MaxBetLogSettingModel
                {
                    SecondAccept_Corner = "7"
                },
                ModifyDate= "2022-09-05 23:52:09",
                ModifyBy="sa"
            }
        };

        var actual = await handler.Handle(request, new CancellationToken(false));
        Equals(expected[0].OldValue.SecondAccept_Corner, actual[0].OldValue.SecondAccept_Corner);
    }
}