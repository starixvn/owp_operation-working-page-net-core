using System;
using Fanex.Data.Repository;
using OWP.EventPicking.Api.Criteria;
using Xunit;

namespace OWP.EventPicking.UnitTests.Criteria;

public class CriteriaUnitTests
{
    [Fact]
    public void QueryMainMatchCriteria_ShouldSetAndGetDate()
    {
        // Arrange
        var criteria = new QueryMainMatchCriteria();
        var testDate = DateTime.Now;

        // Act
        criteria.Date = testDate;

        // Assert
        Assert.Equal(testDate, criteria.Date);
    }

    [Fact]
    public void QueryMainMatchCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new QueryMainMatchCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void QueryChildMatchCriteria_ShouldSetAndGetDate()
    {
        // Arrange
        var criteria = new QueryChildMatchCriteria();
        var testDate = DateTime.Now;

        // Act
        criteria.Date = testDate;

        // Assert
        Assert.Equal(testDate, criteria.Date);
    }

    [Fact]
    public void QueryChildMatchCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new QueryChildMatchCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void QueryMoveEventDateMatchCriteria_ShouldSetAndGetDate()
    {
        // Arrange
        var criteria = new QueryMoveEventDateMatchCriteria();
        var testDate = DateTime.Now;

        // Act
        criteria.Date = testDate;

        // Assert
        Assert.Equal(testDate, criteria.Date);
    }

    [Fact]
    public void QueryMoveEventDateMatchCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new QueryMoveEventDateMatchCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void GetLeagueSettingCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new GetLeagueSettingCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void GetTVListCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new GetTVListCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void InsertOrDeleteNoOfferMatchCriteria_ShouldSetAndGetProperties()
    {
        // Arrange
        var criteria = new InsertOrDeleteNoOfferMatchCriteria();
        var testJson = "{\"matchId\":123,\"reason\":\"test\"}";
        var testActorId = 456;

        // Act
        criteria.MatchInfoJson = testJson;
        criteria.ActorId = testActorId;

        // Assert
        Assert.Equal(testJson, criteria.MatchInfoJson);
        Assert.Equal(testActorId, criteria.ActorId);
    }

    [Fact]
    public void InsertOrDeleteNoOfferMatchCriteria_ShouldImplementINonQueryCommand()
    {
        // Arrange
        var criteria = new InsertOrDeleteNoOfferMatchCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<INonQueryCommand>(criteria);
    }

    [Fact]
    public void GetProviderCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new GetProviderCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void QueryRAndDInfoCriteria_ShouldSetAndGetDate()
    {
        // Arrange
        var criteria = new QueryRAndDInfoCriteria();
        var testDate = DateTime.Now;

        // Act
        criteria.Date = testDate;

        // Assert
        Assert.Equal(testDate, criteria.Date);
    }

    [Fact]
    public void QueryRAndDInfoCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new QueryRAndDInfoCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void QueryUpdateTypeCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new QueryUpdateTypeCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void GetLogCriteria_ShouldSetAndGetInfoSiteMatchId()
    {
        // Arrange
        var criteria = new GetLogCriteria();
        var testId = 123;

        // Act
        criteria.InfoSiteMatchId = testId;

        // Assert
        Assert.Equal(testId, criteria.InfoSiteMatchId);
    }

    [Fact]
    public void GetLogCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new GetLogCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void UpdateConfirmedMatchCriteria_ShouldSetAndGetProperties()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria();
        var testIds = "123,456";
        var testConfirmed = true;
        var testModifiedBy = "testuser";
        var testConfirmedAt = DateTime.Now;

        // Act
        criteria.InfoSiteMatchIds = testIds;
        criteria.Confirmed = testConfirmed;
        criteria.ModifiedBy = testModifiedBy;
        criteria.ConfirmedAt = testConfirmedAt;

        // Assert
        Assert.Equal(testIds, criteria.InfoSiteMatchIds);
        Assert.Equal(testConfirmed, criteria.Confirmed);
        Assert.Equal(testModifiedBy, criteria.ModifiedBy);
        Assert.Equal(testConfirmedAt, criteria.ConfirmedAt);
    }

    [Fact]
    public void UpdateConfirmedMatchCriteria_ShouldImplementINonQueryCommand()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<INonQueryCommand>(criteria);
    }

    [Fact]
    public void GetNewLeagueSettingCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new GetNewLeagueSettingCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void UpdateMaxbetSpreadCriteria_ShouldSetAndGetProperties()
    {
        // Arrange
        var criteria = new UpdateMaxbetSpreadCriteria();
        var testTargetJson = "{\"test\": \"data\"}";
        var testActorId = 123;

        // Act
        criteria.TargetJson = testTargetJson;
        criteria.ActorId = testActorId;

        // Assert
        Assert.Equal(testTargetJson, criteria.TargetJson);
        Assert.Equal(testActorId, criteria.ActorId);
    }

    [Fact]
    public void UpdateMaxbetSpreadCriteria_ShouldImplementINonQueryCommand()
    {
        // Arrange
        var criteria = new UpdateMaxbetSpreadCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<INonQueryCommand>(criteria);
    }

    [Fact]
    public void QueryEventPickingChildMatchCriteria_ShouldSetAndGetDate()
    {
        // Arrange
        var criteria = new QueryEventPickingChildMatchCriteria();
        var testDate = DateTime.Now;

        // Act
        criteria.Date = testDate;

        // Assert
        Assert.Equal(testDate, criteria.Date);
    }

    [Fact]
    public void QueryEventPickingChildMatchCriteria_ShouldImplementICriteria()
    {
        // Arrange
        var criteria = new QueryEventPickingChildMatchCriteria();

        // Act & Assert
        Assert.IsAssignableFrom<ICriteria>(criteria);
    }

    [Fact]
    public void Criteria_WithDateTimeEdgeCases_ShouldHandleCorrectly()
    {
        // Arrange
        var criteria = new QueryMainMatchCriteria();
        var minDate = DateTime.MinValue;
        var maxDate = DateTime.MaxValue;

        // Act
        criteria.Date = minDate;

        // Assert
        Assert.Equal(minDate, criteria.Date);

        // Act
        criteria.Date = maxDate;

        // Assert
        Assert.Equal(maxDate, criteria.Date);
    }

    [Fact]
    public void Criteria_WithNullStringValues_ShouldHandleCorrectly()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria();

        // Act
        criteria.InfoSiteMatchIds = null;
        criteria.ModifiedBy = null;

        // Assert
        Assert.Null(criteria.InfoSiteMatchIds);
        Assert.Null(criteria.ModifiedBy);
    }

    [Fact]
    public void Criteria_WithEmptyStringValues_ShouldHandleCorrectly()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria();

        // Act
        criteria.InfoSiteMatchIds = "";
        criteria.ModifiedBy = "";

        // Assert
        Assert.Equal("", criteria.InfoSiteMatchIds);
        Assert.Equal("", criteria.ModifiedBy);
    }

    [Fact]
    public void Criteria_WithSpecialCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria();
        var specialChars = "!@#$%^&*()_+-=[]{}|;':\",./<>?";

        // Act
        criteria.ModifiedBy = specialChars;

        // Assert
        Assert.Equal(specialChars, criteria.ModifiedBy);
    }

    [Fact]
    public void Criteria_WithUnicodeCharacters_ShouldHandleCorrectly()
    {
        // Arrange
        var criteria = new UpdateConfirmedMatchCriteria();
        var unicodeString = "测试文本 with unicode 🚀";

        // Act
        criteria.ModifiedBy = unicodeString;

        // Assert
        Assert.Equal(unicodeString, criteria.ModifiedBy);
    }
}
