using OWP.EventPicking.Models;
using OWP.EventPicking.Services;
using Xunit;

namespace OWP.EventPicking.UnitTests.Services;

public class BuildLengthPropertyUnitTests
{
    private readonly BuildLengthProperty service;
    public BuildLengthPropertyUnitTests()
    {
        service = new BuildLengthPropertyImp();
    }

    [Fact]
    public void BuildRowColor_MapRowColor()
    {
        var eventPickings = new List<EventPickingModel>
        {
            new() { 
                TV = "K+",
                Channel = "K+NS",
                Wb = "WB(streaming)"
            },
            new() { 
                Provider = "RSD/GB",
                OtherProvider = "Other"
            },
            new() { 
                Division = "division"
            },
            new() { 
                LstNote = "test sa"
            }
        };
        service.Build(eventPickings);

        Assert.Equal(19, eventPickings[0].LengthStreamingCell);
        Assert.Equal(11, eventPickings[1].LengthProviderCell);
        Assert.Equal(8, eventPickings[2].LengthRoundCell);
        Assert.Equal(7, eventPickings[3].LengthRemarkCell);
    }
}