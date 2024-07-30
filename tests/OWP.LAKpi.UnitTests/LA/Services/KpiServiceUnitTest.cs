using AutoMapper;
using NSubstitute;
using OWP.Kpi.LA.Models;
using OWP.Kpi.LA.Services;
using OWP.Kpi.LA.ViewModels;
using Xunit;

namespace OWP.LAKpi.UnitTests.LA.Services;

public class KpiServiceUnitTest
{
    private readonly Repository repositoryMock;
    private readonly IMapper mapperMock;
    private readonly KpiServiceImp service;

    public KpiServiceUnitTest()
    {
        repositoryMock = Substitute.For<Repository>();
        mapperMock = Substitute.For<IMapper>();
        service = new KpiServiceImp(repositoryMock, mapperMock);
    }

    [Fact]
    public async Task BuildKpiInfo_ShouldBuildKpiInfoCorrectly_WhenIsGapIsFalse()
    {
        var queryModel = new QueryModel();
        var kpiData = new List<KpiInfo> { new() };
        var kpiInfoViewModels = new List<KpiInfoViewModel> { new() };

        repositoryMock.GetKpiInfo(queryModel).Returns(kpiData);
        mapperMock.Map<IList<KpiInfoViewModel>>(kpiData).Returns(kpiInfoViewModels);

        var result = await service.BuildKpiInfo(queryModel, isGap: false);

        Assert.NotNull(result);
        Assert.Equal(kpiInfoViewModels.Count, result.Count);
        await repositoryMock.Received(1).GetKpiInfo(queryModel);
        mapperMock.Received(1).Map<IList<KpiInfoViewModel>>(kpiData);
    }

    [Fact]
    public async Task BuildKpiInfo_ShouldBuildKpiInfoCorrectly_WhenIsGapIsTrue()
    {
        var queryModel = new QueryModel();
        var kpiData = new List<KpiInfo> { new() { /* Initialize properties */ } };
        var kpiInfoViewModels = new List<KpiInfoViewModel> { new() { /* Initialize properties */ } };

        repositoryMock.GetKpiInfo(queryModel).Returns(kpiData);
        mapperMock.Map<IList<KpiInfoViewModel>>(kpiData).Returns(kpiInfoViewModels);

        var result = await service.BuildKpiInfo(queryModel, isGap: true);

        Assert.NotNull(result);
        Assert.Equal(kpiInfoViewModels.Count, result.Count);
        await repositoryMock.Received(1).GetKpiInfo(queryModel);
        mapperMock.Received(1).Map<IList<KpiInfoViewModel>>(kpiData);
    }
}