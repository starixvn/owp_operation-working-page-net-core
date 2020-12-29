namespace OWP.OtherSport.UnitTests.Validation.Services
{
    using OWP.OtherSport.Validation.Models;
    using OWP.OtherSport.Validation.Services;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentValidatorUnitTests
    {
        private readonly AssignmentValidatorImp assignmentValidator;

        public AssignmentValidatorUnitTests()
        {
            assignmentValidator = new AssignmentValidatorImp();
        }

        [Fact]
        public void Validate_ValidModel_NotThrowException()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = new List<AssignmentModelInExcel>
                {
                    new AssignmentModelInExcel
                    {
                        Away = "OKC",
                        Home = "Toronto Raptor",
                        League = "NBA",
                        Sport = "tennis"
                    }
                }
            };
            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "starix1" },
                new Trader{ UserId = 2, Username = "starix2" },
                new Trader{ UserId = 3, Username = "starix3" }
            };

            var exception = Record.Exception(() => assignmentValidator.Validate(uploadingDataFromExcel, traders));
            Assert.Null(exception);
        }

        [Fact]
        public void Validate_InValidTrader_ThrowException()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = new List<AssignmentModelInExcel>
                {
                    new AssignmentModelInExcel
                    {
                        Away = "OKC",
                        Home = "Toronto Raptor",
                        League = "NBA",
                        Sport = "Basketball",
                        TraderBadmintonAllProductPointWinner = "starix3",
                        OtherTraderBadmintonRacetoPoints7911 = "#starix3",
                        DateTime = "12/02/2020 03:30 PM"
                    }
                }
            };
            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "starix1" },
                new Trader{ UserId = 2, Username = "starix2" },
                new Trader{ UserId = 0, Username = "starix3" }
            };

            var exception = Record.Exception(() => assignmentValidator.Validate(uploadingDataFromExcel, traders));
            Assert.NotNull(exception);
        }

        [Fact]
        public void ValidateInvalidTraders_ValidTrader_ResulstIsEmpty()
        {
            var actual = new List<ValidationResult>();
            var invalidTrader = new List<Trader> {
                new Trader { UserId = 0, Username = "Bill" }
            };
            var assignmentModelList = GetAssignmentModels();

            assignmentValidator.ValidateTraders(assignmentModelList[0], actual, invalidTrader);
            Assert.Empty(actual);
        }

        [Fact]
        public void ValidateInvalidTraders_InvalidTrader_ReturnError()
        {
            var actual = new List<ValidationResult>();
            var invalidTrader = new List<Trader>
            {
                new Trader { UserId = 0, Username = "Esther" },
                new Trader { UserId = 0, Username = "Lita" }
            };
            var assignmentModelList = GetAssignmentModels();
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Invalid Trader Baseball FT Run Line (HDP): Esther; Other Trader Basketball 1H (OU/OE): Esther; Other Trader Badminton All Product (Set Winner): Lita")
            };

            assignmentValidator.ValidateTraders(assignmentModelList[0], actual, invalidTrader);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CustomValidateAssignment_ValidTrader_NotThrowsException()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = new List<AssignmentModelInExcel>
                {
                    new AssignmentModelInExcel
                    {
                        Away = "GSW",
                        Sport = "Basketball",
                        DateTime = "2020-29-12"
                    }
                }
            };

            var assignmentModelList = new AssignmentModelInExcel
            {
                Away = "OKC",
                Home = "Toronto Raptor",
                League = "NBA",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = "Esther"
            };

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 2, Username = "Lita"}
            };
            var expected = new List<ValidationResult>();

            var actual = assignmentValidator.CustomValidateAssignment(assignmentModelList, uploadingDataFromExcel.Assignments, traders);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CustomValidateAssignment_InvalidTrader_NotThrowsInvalidAssignmentRowException()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = new List<AssignmentModelInExcel>
                {
                    new AssignmentModelInExcel
                    {
                        Away = "OKC",
                        Home = "Toronto Raptor",
                        League = "NBA",
                        Sport = "Basketball",
                        TraderBadmintonAllProductPointWinner = "Curry",
                        DateTime = "2020-29-12"
                    }
                }
            };

            var assignmentModel = new AssignmentModelInExcel
            {
                Away = "OKC",
                Home = "Toronto Raptor",
                League = "NBA",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = "Curry"
            };

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 0, Username = "Curry"}
            };
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Invalid Trader Badminton All Product (Point Winner): Curry")
            };

            var actual = assignmentValidator.CustomValidateAssignment(assignmentModel, uploadingDataFromExcel.Assignments, traders);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void CustomValidateAssignment_DuplicateMatches_AddsDuplicateMatchToResult()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = new List<AssignmentModelInExcel>
                {
                    new AssignmentModelInExcel
                    {
                        Away = "OKC",
                        Home = "Toronto Raptor",
                        League = "NBA",
                        Sport = "Basketball",
                        TraderBadmintonAllProductPointWinner = "Esther",
                        DateTime = "12/02/2020 03:30 PM"
                    },
                    new AssignmentModelInExcel
                    {
                        Away = "OKC",
                        Home = "Toronto Raptor",
                        League = "NBA",
                        Sport = "Basketball",
                        TraderBadmintonAllProductPointWinner = "Esther",
                        DateTime = "12/02/2020 03:30 PM"
                    }
                }
            };

            var assignmentModel = new AssignmentModelInExcel
            {
                Away = "OKC",
                Home = "Toronto Raptor",
                League = "NBA",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = "Esther",
                DateTime = "12/02/2020 03:30 PM"
            };

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"}
            };
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Duplicate Match: NBA | Toronto Raptor -vs- OKC")
            };

            var actual = assignmentValidator.CustomValidateAssignment(assignmentModel, uploadingDataFromExcel.Assignments, traders);
            DeepAssert.Equal(expected, actual);
        }

        private static List<AssignmentModelInExcel> GetAssignmentModels()
        {
            return new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    TraderBadmintonAllProductPointWinner = "Mira",
                    TraderBadmintonAllProductSetWinner = "Mira",
                    TraderBadmintonMatchHandicap = "Mira",
                    TraderBadmintonNextPointWinner = "Mira",
                    TraderBaseball1H1st5InningsHDP = "Carlos",
                    TraderBaseballFTRunLineHDP = "Esther",
                    TraderBasketball1HHDP = "Carlos",
                    TraderBaseball1H1st5InningsOU = "Carlos",
                    TraderBaseballInningHitsRuns = "Jacob",
                    TraderBasketball1HOUOE = "Mira",
                    TraderBadmintonRacetoPoints7911 = "Jacob",
                    TraderBasketball1Q3QHDP = "#Tyler",
                    TraderBasketballFTHDP = "#Tyler",
                    OtherTraderBadmintonAllProductSetWinner = "#Lita",
                    OtherTraderBaseball1H1st5inningsHDP = "#Tyler",
                    OtherTraderBadmintonRacetoPoints7911 = "#Carlos",
                    OtherTraderBaseball1H1st5inningsOU = "#Carlos",
                    OtherTraderBasketball1HOUOE = "#Esther",
                    OtherTraderBasketballFTOUOE = "#Jack",
                    OtherTraderCricketNextOverOUPlayerOU = "#Jack",
                    OtherTraderFieldHockeyFTHDP = "#Carlos",
                    OtherTraderFieldHockey1HOU = "#Jack"
                }
            };
        }
    }
}
