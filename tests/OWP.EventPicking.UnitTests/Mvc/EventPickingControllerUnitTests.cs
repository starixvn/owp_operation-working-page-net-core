using Microsoft.AspNetCore.Mvc;
using OWP.EventPicking.Mvc;
using Xunit;

namespace OWP.EventPicking.UnitTests.Mvc;

public class EventPickingControllerUnitTests
{
    [Fact]
    public void Constructor_ShouldCreateControllerInstance()
    {
        // Act
        var controller = new EventPickingController();

        // Assert
        Assert.NotNull(controller);
    }

    [Fact]
    public void Index_ShouldReturnViewResult()
    {
        // Arrange
        var controller = new EventPickingController();

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", viewResult.ViewName); // Controller explicitly returns "Index"
    }

    [Fact]
    public void Index_ShouldReturnCorrectViewData()
    {
        // Arrange
        var controller = new EventPickingController();

        // Act
        var result = controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.NotNull(viewResult.ViewData);
    }

    [Fact]
    public void Controller_ShouldInheritFromControllerBase()
    {
        // Arrange
        var controller = new EventPickingController();

        // Act & Assert
        Assert.IsAssignableFrom<ControllerBase>(controller);
    }

    [Fact]
    public void Controller_ShouldHaveCorrectNamespace()
    {
        // Arrange
        var controller = new EventPickingController();

        // Act & Assert
        Assert.Equal("OWP.EventPicking.Mvc", controller.GetType().Namespace);
    }
}
