using System.ComponentModel.DataAnnotations;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;
using Xunit;
using Xunit.Asserts.Compare;

namespace OWP.AssignmentUpload.UnitTests.Validation.Services;

public class InfoSiteValidationUnitTests
{
    private readonly InfoSiteValidation infoSiteValidator;

    public InfoSiteValidationUnitTests()
    {
        infoSiteValidator = new InfoSiteValidationImp();
    }

    [Fact]
    public void ValidateDuplicatedMatches_TwoModelIsDuplicate_ErrorDuplicatedMessage()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27"
            },
            new()
            {
                Away = "awaymar271 Last Corner",
                Home = "homemar271 Last Corner",
                League = "leaguemar27 - CORNERS"
            },
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27"
            }
        };

        var actual = new List<ValidationResult>();
        var validationResult = new ValidationResult("There is duplicated match(es) in the list. Please check again");
        var expected = validationResult;


        infoSiteValidator.ValidateDuplicatedMatches(owpMatches[0], owpMatches, actual);
        DeepAssert.Equal(expected, actual[0]);
    }

    [Fact]
    public void ValidateDuplicatedMatches_OnePropertyIsNotSpecified_ErrorDuplicatedMessage()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                Home = "homemar27"
            },
            new()
            {
                Away = "awaymar27",
                Home = "homemar27"
            }
        };

        var actual = new List<ValidationResult>();
        var expected = new ValidationResult("There is duplicated match(es) in the list. Please check again");

        infoSiteValidator.ValidateDuplicatedMatches(owpMatches[0], owpMatches, actual);
        DeepAssert.Equal(expected, actual[0]);
    }

    [Fact]
    public void ValidateDuplicatedMatches_TwoModelIsNotSameAllProperty_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27"
            },
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27 - CORNERS"
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedMatches(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateDuplicatedMatches_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27"
            },
            new()
            {
                Away = "awaymar271 Last Corner",
                Home = "homemar271 Last Corner",
                League = "leaguemar27 - CORNERS"
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedMatches(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateDuplicatedMatches_SabaIsDuplicate_ErrorDuplicatedMessage()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27",
                KickOffTime = new TimeSpan(10, 20, 0),
                IsSaba = true
            },
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27",
                KickOffTime = new TimeSpan(10, 20, 0),
                IsSaba = true
            }
        };

        var actual = new List<ValidationResult>();
        var expected = new ValidationResult("There is duplicated match(es) in the list. Please check again");

        infoSiteValidator.ValidateDuplicatedMatches(owpMatches[0], owpMatches, actual);
        DeepAssert.Equal(expected, actual[0]);
    }

    [Fact]
    public void ValidateDuplicatedMatches_SabaIsNotDuplicate_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27",
                KickOffTime = new TimeSpan(10, 20, 0),
                IsSaba = true
            },
            new()
            {
                Away = "awaymar27",
                Home = "homemar27",
                League = "leaguemar27",
                KickOffTime = new TimeSpan(3, 20, 0),
                IsSaba = true
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedMatches(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateDuplicatedInfoSiteId_TwoModelIsDuplicate_ErrorDuplicatedMessage()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                InfoSiteMatchId = 10
            },
            new()
            {

                InfoSiteMatchId = 10
            }
        };

        var actual = new List<ValidationResult>();
        var expected = new ValidationResult($"Duplicate Infosite Id: '{owpMatches[0].InfoSiteMatchId}' of matches");

        infoSiteValidator.ValidateDuplicatedInfoSiteId(owpMatches[0], owpMatches, actual);
        DeepAssert.Equal(expected, actual[0]);
    }

    [Fact]
    public void ValidateDuplicatedInfoSiteId_InfoSiteMatchIdIsNotSpecified_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27"
            },
            new()
            {

                InfoSiteMatchId = 1
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedInfoSiteId(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateDuplicatedInfoSiteId_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                InfoSiteMatchId = 1
            },
            new()
            {

                InfoSiteMatchId = 2
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedInfoSiteId(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateDuplicatedAdminSiteId_TwoModelIsDuplicate_ErrorDuplicatedMessage()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                AdminSiteId = 10
            },
            new()
            {

                AdminSiteId = 10
            }
        };

        var actual = new List<ValidationResult>();
        var expected = new ValidationResult($"Duplicate AdminSite Id: '{owpMatches[0].AdminSiteId}' of matches");

        infoSiteValidator.ValidateDuplicatedAdminSiteId(owpMatches[0], owpMatches, actual);
        DeepAssert.Equal(expected, actual[0]);
    }

    [Fact]
    public void ValidateDuplicatedAdminSiteId_InfoSiteMatchIdIsNotSpecified_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27"
            },
            new()
            {

                AdminSiteId = 1
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedAdminSiteId(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void ValidateDuplicatedAdminSiteId_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                AdminSiteId = 1
            },
            new()
            {

                AdminSiteId = 2
            }
        };

        var actual = new List<ValidationResult>();

        infoSiteValidator.ValidateDuplicatedAdminSiteId(owpMatches[0], owpMatches, actual);
        Assert.Empty(actual);
    }

    [Fact]
    public void CustomValidateInfoSiteMatch_ModelIsDuplicate_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                InfoSiteMatchId = 1,
                MatchNo = "match1"
            },
            new()
            {
                InfoSiteMatchId = 1,
                MatchNo = "match1"
            }
        };

        var expected = new ValidationResult("Duplicate Infosite Id: '1' of matches");

        var actual = infoSiteValidator.CustomValidateInfoSiteMatch(owpMatches[0], owpMatches);
        DeepAssert.Equal(expected, actual[0]);
    }

    [Fact]
    public void CustomValidateInfoSiteMatch_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27"
            }
        };
        var exception = Record.Exception(() => infoSiteValidator.CustomValidateInfoSiteMatch(owpMatches[0], owpMatches));
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_ValidModel_NotThrowException()
    {
        var owpMatches = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27"
            }
        };

        var exception = Record.Exception(() => infoSiteValidator.Validate(owpMatches));
        Assert.Null(exception);
    }

    [Fact]
    public void Validate_InValidModel_ThrowException()
    {
        var assignmentModelList = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                League = "league",
                Home = "home"
            },
            new()
            {
                Away = "awaymar27",
                League = "league",
                Home = "home"
            }
        };

        var expected = new List<AssignmentValidationRowResult>
        {
            new()
            {
                Row = 1,
                Message = "There is duplicated match(es) in the list. Please check again"
            },
            new()
            {
                Row = 2,
                Message = "There is duplicated match(es) in the list. Please check again"
            }
        };

        var actual = Assert.Throws<InvalidAssignmentRowException>(() => infoSiteValidator.Validate(assignmentModelList));
        DeepAssert.Equal(expected, actual.ValidationResults);
    }

    [Fact]
    public void Validate_MatchValidatorIsNull_NotValidate_NotThrowException()
    {
        var assignmentModelList = new List<AssignmentModel>
        {
            new()
            {
                Away = "awaymar27",
                League = "league",
                Home = "home",
                MinorBettypeId = -89
            }
        };

        var exception = Record.Exception(() => infoSiteValidator.Validate(assignmentModelList));
        Assert.Null(exception);
    }
}