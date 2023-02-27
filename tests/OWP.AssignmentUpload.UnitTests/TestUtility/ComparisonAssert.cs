using Newtonsoft.Json;
using Xunit;

namespace OWP.AssignmentUpload.UnitTests.TestUtility;

internal static class ComparisonAssert
{
    internal static void AreEqual(object expected, object actual)
    {
        var expectedJson = JsonConvert.SerializeObject(expected);
        var actualJson = JsonConvert.SerializeObject(actual);
        Assert.Equal(expectedJson, actualJson);
    }
}