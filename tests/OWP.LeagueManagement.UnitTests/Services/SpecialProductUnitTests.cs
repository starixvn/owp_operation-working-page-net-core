using Fanex.NAP.Client;
using NSubstitute;
using OWP.LeagueManagement.Models;
using OWP.LeagueManagement.Services;
using Xunit;

namespace OWP.LeagueManagement.UnitTests.Services;

public class SpecialProductUnitTests
{
    private readonly SpecialProductService service;
    private readonly Repository repository;

    public SpecialProductUnitTests()
    {
        repository = Substitute.For<Repository>();
        var session = Substitute.For<INapSession>();
        service = new SpecialProductServiceImp(repository, session);
    }
    [Fact]
    public async Task UpdateSpecialProduct_Always_ReturnData()
    {
        var query = new SpecialProduct();
        // Arrange
        var model = new ProductInfo
        {
            ErrorMessage = null,
            ProductId = 288
        };
        repository.UpdateSpecialProduct(Arg.Compat.Any<SpecialProduct>()).Returns(model);

        // Act
        var actual = await service.UpdateSpecialProduct(query);

        // Assert
        Assert.Equal(model.ErrorMessage, actual.ErrorMessage);
        Assert.Equal(model.ProductId, actual.ProductId);
    }
    [Fact]
    public async Task ViewLog_Always_ReturnData()
    {
        // Arrange
        var model = new QueryLogModel();
        var mockReturnLogModel = new List<UserLogModel>
        {
            new()
            {
                Action = "Update",
                DataOld = "<r><z p=\"Maxbet No.Of Corner\" vl=\"5000.0000\"/></r>",
                Data = "<r><z p=\"Maxbet No.Of Corner\" vl=\"2000.0000\"/></r>",
                ModifiedDate = new DateTime(2022,07,14,9,11,00),
                ModifiedBy = "sa"
            }
        };
        var expect = new List<LogDisplayModel>
        {
            new()
            {
                Action= "Update",
                ModifyBy= "sa",
                ModifyDate= "07/14/2022 10:11 AM",
                NewValue= "2000.0000",
                OldValue= "5000.0000",
                Product= "Maxbet No.Of Corner"
            }
        };

        repository.SearchUserLog(Arg.Compat.Any<QueryLogModel>()).Returns(mockReturnLogModel);

        // Act
        var actual = await service.BuildLog(model);

        // Assert
        Assert.Equal(expect.First().NewValue, actual.First().NewValue);
    }

}