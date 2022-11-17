namespace OWP.AssignmentUpload.UnitTests.Validation.Services
{
    using System.Collections.Generic;
    using AssignmentUpload.Validation.Exceptions;
    using AssignmentUpload.Validation.Models;
    using AssignmentUpload.Validation.Services;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class MatchStatusValidatorUnitTests
    {
        private readonly MatchStatusValidatorImp matchStatus;

        public MatchStatusValidatorUnitTests()
        {
            matchStatus = new MatchStatusValidatorImp();
        }

        [Fact]
        public void Validate_MainMatchStatusIsCancel_MinorStatusIsDifferentFromMainStatus_ThrowException()
        {
            var assignments = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                    Home = "Ha Noi FC U19 (N)",
                    Away = "Than Quang Ninh U19",
                    HashKey = "ahihi",
                    MatchStatus = "Cancel",
                    ParentHashKey= ""
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) Next Corners",
                    Away = "Than Quang Ninh U19 Next Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Live"
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) No.of Corners",
                    Away = "Than Quang Ninh U19 No.of Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Suspended"
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) No.of Corners",
                    Away = "Than Quang Ninh U19 No.of Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Postponed"
                }
            };
            var expected = new List<AssignmentValidationRowResult>
            {
                new AssignmentValidationRowResult
                {
                    Row = 2,
                    Message = "The status of special events must be same as main match if main match is Cancel, Suspended, or Postponed"
                },
                new AssignmentValidationRowResult
                {
                    Row = 3,
                    Message = "The status of special events must be same as main match if main match is Cancel, Suspended, or Postponed"
                },
                new AssignmentValidationRowResult
                {
                    Row = 4,
                    Message = "The status of special events must be same as main match if main match is Cancel, Suspended, or Postponed"
                }
            };

            var exception = Assert.Throws<InvalidAssignmentRowException>(() => matchStatus.Validate(assignments));
            DeepAssert.Equal(expected, exception.ValidationResults);
        }

        [Fact]
        public void Validate_MainMatchStatusIsSuspended_MinorStatusIsSimilarToMainStatus_Valid()
        {
            var assignments = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                    Home = "Ha Noi FC U19 (N)",
                    Away = "Than Quang Ninh U19",
                    HashKey = "ahihi",
                    MatchStatus = "Suspended",
                    ParentHashKey= ""
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) Next Corners",
                    Away = "Than Quang Ninh U19 Next Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Suspended"
                }
            };

            var exception = Record.Exception(() => matchStatus.Validate(assignments));
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_MainMatchStatusIsLive_AnyMinorStatus_Valid()
        {
            var assignments = new List<AssignmentModel>
            {
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS",
                    Home = "Ha Noi FC U19 (N)",
                    Away = "Than Quang Ninh U19",
                    HashKey = "ahihi",
                    MatchStatus = "Live",
                    ParentHashKey= ""
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) Next Corners",
                    Away = "Than Quang Ninh U19 Next Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Live"
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) No.of Corners",
                    Away = "Than Quang Ninh U19 No.of Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Suspended"
                },
                new AssignmentModel
                {
                    League = "VIETNAM CHAMPIONSHIP U19 QUALIFIERS - CORNERS",
                    Home = "Ha Noi FC U19 (N) No.of Corners",
                    Away = "Than Quang Ninh U19 No.of Corners",
                    ParentHashKey = "ahihi",
                    MatchStatus = "Postponed"
                }
            };

            var exception = Record.Exception(() => matchStatus.Validate(assignments));
            Assert.Null(exception);
        }

    }
}
