namespace OWP.AssignmentUpload.UnitTests.Validation.Services
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using AssignmentUpload.Validation.Exceptions;
    using AssignmentUpload.Validation.Models;
    using AssignmentUpload.Validation.Services;
    using Xunit;
    using Xunit.Asserts.Compare;

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
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27"
                },
                new AssignmentModel
                {
                    Away = "awaymar271 Last Corner",
                    Home = "homemar271 Last Corner",
                    League = "leaguemar27 - CORNERS",
                },
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                }
            };

            var actual = new List<ValidationResult>();
            var validationResult = new ValidationResult("There is duplicated match(es) in the list. Please check again");
            var expected = validationResult;


            infoSiteValidator.ValidateDuplicatedMatches(owpMatchs[0], owpMatchs, actual);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateDuplicatedMatches_OnePropertyIsNotSpecified_ErrorDuplicatedMessage()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27"
                },
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27"
                }
            };

            var actual = new List<ValidationResult>();
            var expected = new ValidationResult("There is duplicated match(es) in the list. Please check again");

            infoSiteValidator.ValidateDuplicatedMatches(owpMatchs[0], owpMatchs, actual);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateDuplicatedMatches_TwoModelIsNotSameAllProperty_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27"
                },
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27 - CORNERS",
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedMatches(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedMatches_NotModelIsDuplicate_ResultMessageShpuldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27"
                },
                new AssignmentModel
                {
                    Away = "awaymar271 Last Corner",
                    Home = "homemar271 Last Corner",
                    League = "leaguemar27 - CORNERS",
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedMatches(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedMatches_SabaIsDuplicate_ErrorDuplicatedMessage()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                    KickOffTime = new System.TimeSpan(10, 20, 0),
                    IsSaba = true
                },
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                    KickOffTime = new System.TimeSpan(10, 20, 0),
                    IsSaba = true
                }
            };

            var actual = new List<ValidationResult>();
            var expected = new ValidationResult("There is duplicated match(es) in the list. Please check again");

            infoSiteValidator.ValidateDuplicatedMatches(owpMatchs[0], owpMatchs, actual);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateDuplicatedMatches_SabaIsNotDuplicate_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                    KickOffTime = new System.TimeSpan(10, 20, 0),
                    IsSaba = true
                },
                new AssignmentModel
                {
                    Away = "awaymar27",
                    Home = "homemar27",
                    League = "leaguemar27",
                    KickOffTime = new System.TimeSpan(3, 20, 0),
                    IsSaba = true
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedMatches(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedInfoSiteId_TwoModelIsDuplicate_ErrorDuplicatedMessage()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    InfoSiteMatchId = 10
                },
                new AssignmentModel
                {

                    InfoSiteMatchId = 10
                }
            };

            var actual = new List<ValidationResult>();
            var expected = new ValidationResult($"Duplicate Infosite Id: '{owpMatchs[0].InfoSiteMatchId}' of matches");

            infoSiteValidator.ValidateDuplicatedInfoSiteId(owpMatchs[0], owpMatchs, actual);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateDuplicatedInfoSiteId_InfoSiteMatchIdIsNotSpecified_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27"
                },
                new AssignmentModel
                {

                    InfoSiteMatchId = 1
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedInfoSiteId(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedInfoSiteId_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    InfoSiteMatchId = 1
                },
                new AssignmentModel
                {

                    InfoSiteMatchId = 2
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedInfoSiteId(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedAdminSiteId_TwoModelIsDuplicate_ErrorDuplicatedMessage()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    AdminSiteId = 10
                },
                new AssignmentModel
                {

                    AdminSiteId = 10
                }
            };

            var actual = new List<ValidationResult>();
            var expected = new ValidationResult($"Duplicate AdminSite Id: '{owpMatchs[0].AdminSiteId}' of matches");

            infoSiteValidator.ValidateDuplicatedAdminSiteId(owpMatchs[0], owpMatchs, actual);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateDuplicatedAdminSiteId_InfoSiteMatchIdIsNotSpecified_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27"
                },
                new AssignmentModel
                {

                    AdminSiteId = 1
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedAdminSiteId(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedAdminSiteId_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    AdminSiteId = 1
                },
                new AssignmentModel
                {

                    AdminSiteId = 2
                }
            };

            var actual = new List<ValidationResult>();

            infoSiteValidator.ValidateDuplicatedAdminSiteId(owpMatchs[0], owpMatchs, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void CustomValidateInfoSiteMatch_ModelIsDuplicate_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    InfoSiteMatchId = 1,
                    MatchNo = "match1",
                },
                new AssignmentModel
                {
                    InfoSiteMatchId = 1,
                    MatchNo = "match1"
                }
            };

            var expected = new ValidationResult("Duplicate Infosite Id: '1' of matches");

            var actual = infoSiteValidator.CustomValidateInfoSiteMatch(owpMatchs[0], owpMatchs);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void CustomValidateInfoSiteMatch_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
        {
            var owpMatchs = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                }
            };
            var exception = Record.Exception(() => infoSiteValidator.CustomValidateInfoSiteMatch(owpMatchs[0], owpMatchs));
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_ValidModel_NotThrowException()
        {
            var owpMatchs = new List<AssignmentModel>
                {
                    new AssignmentModel
                    {
                        Away = "awaymar27"
                    }
                };

            var exception = Record.Exception(() => infoSiteValidator.Validate(owpMatchs));
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_InValidModel_ThrowException()
        {
            var assignmentModelList = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    Away = "awaymar27",
                    League = "league",
                    Home = "home"
                },
                new AssignmentModel
                {
                    Away = "awaymar27",
                    League = "league",
                    Home = "home"
                }
            };

            var expected = new List<AssignmentValidationRowResult>
            {
                new AssignmentValidationRowResult
                {
                    Row = 1,
                    Message = "There is duplicated match(es) in the list. Please check again"
                },
                new AssignmentValidationRowResult
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
                new AssignmentModel
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
}