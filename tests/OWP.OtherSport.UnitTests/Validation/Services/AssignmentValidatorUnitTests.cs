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
                        Away = "GSW",
                        Sport = "Basketball"
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
                        Away = "GSW",
                        Sport = "Basketball",
                        TraderBadmintonAllProductPointWinner = "starix3",
                        OtherTraderBadmintonRacetoPoints7911 = "#starix3"
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
