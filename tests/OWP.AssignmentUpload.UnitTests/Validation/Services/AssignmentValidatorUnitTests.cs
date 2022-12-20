namespace OWP.AssignmentUpload.UnitTests.Validation.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using OWP.AssignmentUpload.Validation.Exceptions;
    using OWP.AssignmentUpload.Validation.Models;
    using OWP.AssignmentUpload.Validation.Services;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentValidatorImpUnitTests
    {
        private readonly AssignmentValidatorImp assignmentValidator;

        public AssignmentValidatorImpUnitTests()
        {
            assignmentValidator = new AssignmentValidatorImp();
        }

        [Fact]
        public void ValidateEventDate_TimeInvalid_ReturnResultHasMessageInvalidTime()
        {
            var validationResult = new List<ValidationResult>();
            AssignmentValidatorImp.ValidateInvalidTime(new AssignmentModelInExcel
            {
                Time = null
            }, validationResult);
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Invalid time")
            };

            DeepAssert.Equal(expected, validationResult);
        }

        [Fact]
        public void ValidateEventDate_EventDateEqualDateTime_ValidateEventDate()
        {
            var dateTime = new DateTime(2020, 03, 30);
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                EventDate = new DateTime(2020, 03, 30)
            };
            var exception = Record.Exception(() => assignmentValidator.ValidateEventDate(dateTime, uploadingDataFromExcel));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateEventDate_EventDateNotEqualDateTime_ThrowEventDateMismatchException()
        {
            var dateTime = new DateTime(2020, 03, 20);
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                EventDate = new DateTime(2020, 03, 30)
            };
            Assert.Throws<EventDateMismatchException>(() => assignmentValidator.ValidateEventDate(dateTime, uploadingDataFromExcel));
        }

        [Fact]
        public void ValidateDuplicatedTraders_IsDuplicateIsTrue_ThrowDuplicatedTraderException()
        {
            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "starix1", IsDuplicate = true },
                new Trader{ UserId = 2, Username = "starix3", IsDuplicate = false }
            };
            Assert.Throws<DuplicatedTraderException>(() => assignmentValidator.ValidateDuplicatedTraders(traders));
        }

        [Fact]
        public void ValidateDuplicatedTraders_IsDuplicateIsFalse_Valid()
        {
            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "starix1", IsDuplicate = false },
                new Trader{ UserId = 2, Username = "starix2", IsDuplicate = false },
                new Trader{ UserId = 3, Username = "starix3", IsDuplicate = false }
            };
            var exception = Record.Exception(() => assignmentValidator.ValidateDuplicatedTraders(traders));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateDuplicatedMatchNo_TwoModelIsDuplicate_ErrorDuplicatedMessage()
        {
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "awaymar27",
                    MatchNo = "Match1"
                },
                new AssignmentModelInExcel
                {
                    Time = "01:00 PM",
                    MatchNo = "Match1"
                }
            };

            var actual = new List<ValidationResult>();
            var expected = new ValidationResult($"Duplicate Match No: '{assignmentModelList[0].MatchNo}' of matches");

            assignmentValidator.ValidateDuplicatedMatchNo(assignmentModelList[0], assignmentModelList, actual);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateDuplicatedMatchNo_MatchNoIsNotSpecified_ResultMessageShouldBeEmpty()
        {
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "awaymar27"
                },
                new AssignmentModelInExcel
                {
                    Time = "01:00 PM",
                    MatchNo = "Match2"
                }
            };

            var actual = new List<ValidationResult>();

            assignmentValidator.ValidateDuplicatedMatchNo(assignmentModelList[0], assignmentModelList, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateDuplicatedMatchNo_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
        {
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "awaymar27",
                    MatchNo = "Match1"
                },
                new AssignmentModelInExcel
                {
                    Time = "01:00 PM",
                    MatchNo = "Match2"
                }
            };

            var actual = new List<ValidationResult>();

            assignmentValidator.ValidateDuplicatedMatchNo(assignmentModelList[0], assignmentModelList, actual);
            Assert.Empty(actual);
        }

        [Fact]
        public void CustomValidateAssignment_ModelIsDuplicate_ThrowsInvalidAssignmentRowException()
        {
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "awaymar27",
                    InfoSiteMatchId = 1,
                    MatchNo = "match1",
                    Time = "01:00 PM"
                },
                new AssignmentModelInExcel
                {
                    Time = "01:00 PM",
                    InfoSiteMatchId = 1,
                    MatchNo = "match1"
                }
            };

            var expected = new List<AssignmentValidationRowResult>
            {
                new AssignmentValidationRowResult
                {
                    Row = 1,
                    Message = "Duplicate Match No: 'match1' of matches"
                },
                new AssignmentValidationRowResult
                {
                    Row = 2,
                    Message = "Duplicate Match No: 'match1' of matches"
                }
            };

            var actual = Assert.Throws<InvalidAssignmentRowException>(() => assignmentValidator.CustomValidateAssignment(assignmentModelList, false, new List<Trader>()));
            DeepAssert.Equal(expected, actual.ValidationResults);
        }

        [Fact]
        public void CustomValidateAssignment_NotModelIsDuplicate_ResultMessageShouldBeEmpty()
        {
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "awaymar27",
                    Time = "01:00 PM"
                },
                new AssignmentModelInExcel
                {
                    Time = "01:00 PM"
                }
            };
            var exception = Record.Exception(() => assignmentValidator.CustomValidateAssignment(assignmentModelList, false, new List<Trader>()));
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_ValidModel_NotThrowException()
        {
            var dateTime = new DateTime(2020, 03, 30);
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                EventDate = new DateTime(2020, 03, 30),
                Assignments = new List<AssignmentModelInExcel>
                {
                    new AssignmentModelInExcel
                    {
                        Away = "awaymar27",
                        Time = "01:00 PM"
                    },
                    new AssignmentModelInExcel
                    {
                        Time = "01:00 PM"
                    }
                }
            };
            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "starix1" },
                new Trader{ UserId = 2, Username = "starix2" },
                new Trader{ UserId = 3, Username = "starix3" }
            };

            var exception = Record.Exception(() => assignmentValidator.Validate(dateTime, uploadingDataFromExcel, traders, false));
            Assert.Null(exception);
        }

        [Fact]
        public void ValidateOtherAssignment_IsUploadFMAndAIOIsTrue_CheckFMandAIO()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    OtherTraders10Mins = "a",
                    OtherTraders15Mins = "a",
                    OtherTraders1H1X2 = "#Mira",
                    OtherTraders1HHDP = "v",
                    OtherTraders1HOU = "u",
                    OtherTraders1X2 = "i",
                    OtherTradersAIO = "i",
                    OtherTradersFM = "#Lina",
                    OtherTradersHDP = "o",
                    OtherTradersLA = "p",
                    OtherTradersOU = "p"
                }
            };

            var expected = new ValidationResult("Missing main assignment of 10 Mins; 15 Mins; 1H 1X2; 1H HDP; 1H O/U; 1X2; AIO; FM; HDP; LA; O/U");

            assignmentValidator.ValidateOtherAssignment(assignmentModelList[0], actual, true);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateOtherAssignment_IsUploadFMAndAIOIsFalse_NotCheckFMandAIO()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    OtherTraders10Mins = "a",
                    OtherTraders15Mins = "a",
                    OtherTraders1H1X2 = "#Mira",
                    OtherTraders1HHDP = "v",
                    OtherTraders1HOU = "u",
                    OtherTraders1X2 = "i",
                    OtherTradersAIO = "i",
                    OtherTradersFM = "#Lina",
                    OtherTradersHDP = "o",
                    OtherTradersLA = "p",
                    OtherTradersOU = "p"
                }
            };

            var expected = new ValidationResult("Missing main assignment of 10 Mins; 15 Mins; 1H 1X2; 1H HDP; 1H O/U; 1X2; HDP; LA; O/U");

            assignmentValidator.ValidateOtherAssignment(assignmentModelList[0], actual, false);
            DeepAssert.Equal(expected, actual[0]);
        }

        [Fact]
        public void ValidateOtherAssignment_Valid_ResultIsEmpty()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Trader10MinsName = "a",
                    Trader15MinsName = "a",
                    Trader1H1X2Name = "#Mira",
                    Trader1HHDPName = "v",
                    Trader1HOUName = "u",
                    Trader1X2Name = "i",
                    TraderAIOName = "i",
                    TraderFMName = "#Lina",
                    TraderHDPName = "o",
                    TraderLAName = "p",
                    TraderOUName = "p",
                    OtherTraders10Mins = "a",
                    OtherTraders15Mins = "a",
                    OtherTraders1H1X2 = "#Mira",
                    OtherTraders1HHDP = "v",
                    OtherTraders1HOU = "u",
                    OtherTraders1X2 = "i",
                    OtherTradersAIO = "i",
                    OtherTradersFM = "#Lina",
                    OtherTradersLA = "p",
                    OtherTradersOU = "p"
                }
            };

            assignmentValidator.ValidateOtherAssignment(assignmentModelList[0], actual, true);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateOtherAssignment2_Valid_ResultIsEmpty()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Trader10MinsName = "a"
                }
            };

            assignmentValidator.ValidateOtherAssignment(assignmentModelList[0], actual, true);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateInvalidTraders_InvalidTrader_ReturnError()
        {
            var actual = new List<ValidationResult>();
            var invalidTrader = new List<Trader>
            {
                new Trader { UserId = 0, Username = "a" },
                new Trader { UserId = 0, Username = "p" },
                new Trader { UserId = 1, Username = "u" },
                new Trader { UserId = 0, Username = "o" },
                new Trader { UserId = 0, Username = "v" },
                new Trader { UserId = 0, Username = "Mira" },
                new Trader { UserId = 0, Username = "Carlos" }
            };
            var assignmentModelList = GetAssignmentModels();
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Invalid OtherLA: Carlos; OtherTraders (1H O/U): Carlos; OtherTraders (1X2): Carlos; OtherTraders (HDP): Carlos; LA: Mira; Trader (10mins): Mira; Trader (15mins): Mira; Trader (1H 1X2): Mira; Trader (1H HDP): Mira")
            };

            assignmentValidator.ValidateTraders(assignmentModelList[0], actual, invalidTrader);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateInvalidTraders_ValidTrader_ResulstIsEmpty()
        {
            var actual = new List<ValidationResult>();
            var invalidTrader = new List<Trader> {
                new Trader { UserId = 0, Username = "Lisa" }
            };
            var assignmentModelList = GetAssignmentModels();
            var expected = new List<ValidationResult>();

            assignmentValidator.ValidateTraders(assignmentModelList[0], actual, invalidTrader);
            Assert.Empty(actual);
        }

        private static List<AssignmentModelInExcel> GetAssignmentModels()
        {
            return new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Trader10MinsName = "Mira",
                    Trader15MinsName = "Mira",
                    Trader1H1X2Name = "Mira",
                    Trader1HHDPName = "Mira",
                    Trader1HOUName = "Esther",
                    Trader1X2Name = "Esther",
                    TraderAIOName = "Esther",
                    TraderFMName = "Esther",
                    TraderHDPName = "Jacob",
                    TraderLAName = "Mira",
                    TraderOUName = "Jacob",
                    OtherTraders10Mins = "#Tyler",
                    OtherTraders15Mins = "#Tyler",
                    OtherTraders1H1X2 = "#Lita",
                    OtherTraders1HHDP = "#Tyler",
                    OtherTradersHDP = "#Carlos",
                    OtherTraders1HOU = "#Carlos",
                    OtherTraders1X2 = "#Carlos",
                    OtherTradersAIO = "#Jack",
                    OtherTradersFM = "#Jack",
                    OtherTradersLA = "#Carlos",
                    OtherTradersOU = "#Jack"
                }
            };
        }

    }
}