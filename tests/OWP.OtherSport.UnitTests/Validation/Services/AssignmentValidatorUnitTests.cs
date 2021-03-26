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
        public void Validate_InvValidModel_ThrowException()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = GetAssignmentModels()
            };

            var traders = new List<Trader>
            {
                new Trader{ UserId = 0, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 0, Username = "Curry"},
                new Trader{ UserId = 11, Username = "Jack"},
                new Trader{ UserId = 12, Username = "Tyler"},
                new Trader{ UserId = 14, Username = "Carlos"},
                new Trader{ UserId = 17, Username = "Jacob"}
            };

            var exception = Record.Exception(() => assignmentValidator.Validate(uploadingDataFromExcel, traders));
            Assert.NotNull(exception);
        }

        [Fact]
        public void Validate_ValidModel_NotThrowException()
        {
            var uploadingDataFromExcel = new UploadingDataFromExcel
            {
                Assignments = GetAssignmentModels()
            };

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 0, Username = "Curry"},
                new Trader{ UserId = 11, Username = "Jack"},
                new Trader{ UserId = 12, Username = "Tyler"},
                new Trader{ UserId = 14, Username = "Carlos"},
                new Trader{ UserId = 17, Username = "Jacob"}
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
        public void ValidateMatchNo_ValidMatchNo_ResultIsEmpty()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelInExcel = new AssignmentModelInExcel
            {
                MatchNo = "123.456",
                Away = "OKC",
                Home = "Toronto Raptor",
                League = "NBA",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = "starix3",
                OtherTraderBadmintonRacetoPoints7911 = "#starix3"
            };
            assignmentValidator.ValidateMatchNo(assignmentModelInExcel, actual);
            Assert.Empty(actual);
        }


        [Fact]
        public void ValidateMatchNo_InValidMatchNo_ErrorIsCatch()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelInExcel = new AssignmentModelInExcel
            {
                MatchNo = "123f.456",
                Away = "OKC",
                Home = "Toronto Raptor",
                League = "NBA",
                Sport = "Basketball",
                TraderBadmintonAllProductPointWinner = "starix3",
                OtherTraderBadmintonRacetoPoints7911 = "#starix3"
            };
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Match No must be number")
            };
            assignmentValidator.ValidateMatchNo(assignmentModelInExcel, actual);
            DeepAssert.Equal(expected, actual);
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
        public void ValidateDateTime_InValidDateTime_ReturnError()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "OKC",
                    Home = "Toronto Raptor",
                    League = "NBA",
                    Sport = "Basketball",
                    TraderBadmintonAllProductPointWinner = "Esther",
                    DateTime = "Feb 32, 2020 02:00 PM"
                }
            };

            var expected = new List<ValidationResult>
            {
                new ValidationResult("[Column: Date Time] - Error: Invalid date time format")
            };

            assignmentValidator.ValidateDateTime(assignmentModelList[0], actual);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateDateTime_InValidDateTimeFormat_ReturnError()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "OKC",
                    Home = "Toronto Raptor",
                    League = "NBA",
                    Sport = "Basketball",
                    TraderBadmintonAllProductPointWinner = "Esther",
                    DateTime = "02 32, 2020 02:00 PM"
                }
            };

            var expected = new List<ValidationResult>
            {
                new ValidationResult("[Column: Date Time] - Error: Invalid date time format")
            };

            assignmentValidator.ValidateDateTime(assignmentModelList[0], actual);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateDateTime_ValidDateTime_ResulstIsEmpt()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    Away = "OKC",
                    Home = "Toronto Raptor",
                    League = "NBA",
                    Sport = "Basketball",
                    TraderBadmintonAllProductPointWinner = "Esther",
                    DateTime = "43883.583333333336"
                }
            };

            var expected = new List<ValidationResult>();

            assignmentValidator.ValidateDateTime(assignmentModelList[0], actual);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateMaxbet_ValidMaxbet_ResulstIsEmpt()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                GetAssignmentModel(string.Empty)
            };

            var expected = new List<ValidationResult>();

            assignmentValidator.ValidateMaxbetInString(assignmentModelList[0], actual);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateMaxbet_InValidMaxbet_ResulstIsEmpt()
        {
            var actual = new List<ValidationResult>();
            var assignmentModelList = new List<AssignmentModelInExcel>
            {
                GetAssignmentModel("esther")
            };

            var expected = new List<ValidationResult>
            {
                new ValidationResult("[Column: Max Bet Football FT (HDP)] - Error: Invalid Maxbet format")
            };

            assignmentValidator.ValidateMaxbetInString(assignmentModelList[0], actual);
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
                        DateTime = "43881.583333333336"
                    }
                }
            };

            var assignmentModelList = GetAssignmentModel(string.Empty);

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
                        TraderBadmintonAllProductSetWinner = "Curry",
                        DateTime = "43883"
                    }
                }
            };

            var assignmentModel = GetAssignmentModel(string.Empty);

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 0, Username = "Curry"}
            };
            var expected = new List<ValidationResult>
            {
                new ValidationResult("Invalid Trader Badminton All Product (Set Winner): Curry")
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
                        DateTime = "43831.645833333336"
                    },
                    new AssignmentModelInExcel
                    {
                        Away = "OKC",
                        Home = "Toronto Raptor",
                        League = "NBA",
                        Sport = "Basketball",
                        TraderBadmintonAllProductPointWinner = "Esther",
                        DateTime = "43831.645833333336"
                    }
                }
            };

            var assignmentModel = GetAssignmentModel(string.Empty);

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

        [Fact]
        public void CustomValidateAssignment_NotDuplicateMatches()
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
                        DateTime = "43832.645833333336"
                    }
                }
            };

            var assignmentModel = GetAssignmentModel(string.Empty);

            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"}
            };
            var expected = new List<ValidationResult>();

            var actual = assignmentValidator.CustomValidateAssignment(assignmentModel, uploadingDataFromExcel.Assignments, traders);
            DeepAssert.Equal(expected, actual);
        }

        [Fact]
        public void GroupDuplicateMessage()
        {
            var assignments = new List<AssignmentValidationRowResult>
            {
                new(){ Row = 2,  Message = "Duplicate Match ABC XYZ"},
                new(){ Row = 10, Message = "Duplicate Match ABC XYZ"},
                new(){ Row = 12, Message = "Duplicate Match ABC XYZ"},
                new(){ Row = 15, Message = "Duplicate Match XYZ"},
                new(){ Row = 15, Message = "invalid trader HDP"}
            };

            var expected = new List<AssignmentValidationRowResult>
            {
                new(){ Row = 2,  Message = "Duplicate Match ABC XYZ (with row 10,12)"},
                new(){ Row = 15, Message = "Duplicate Match XYZ"},
                new(){ Row = 15, Message = "invalid trader HDP"}
            };

            var actual = AssignmentValidatorImp.GroupByDuplicateMatchMessage(assignments);

            DeepAssert.Equal(expected, actual);
        }

        private static List<AssignmentModelInExcel> GetAssignmentModels()
        {
            return new List<AssignmentModelInExcel>
            {
                new AssignmentModelInExcel
                {
                    MatchNo="123",
                    Away = "OKC",
                    Home = "Toronto Raptor",
                    League = "NBA",
                    Sport = "tennis",
                    DateTime = "44247",
                    TraderBadmintonAllProductPointWinner = "Mira",
                    TraderBadmintonAllProductSetWinner = "Mira",
                    TraderBadmintonMatchHandicap = "Mira",
                    TraderBadmintonNextPointWinner = "Mira",
                    TraderBaseball1H1st5inningsHDP = "Carlos",
                    TraderBaseballFTRunLineHDP = "Esther",
                    TraderBasketball1HHDP = "Carlos",
                    TraderBaseball1H1st5inningsOU = "Carlos",
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
                    OtherTraderFieldHockey1HOU = "#Jack",
                    MaxBetBadmintonAllProductPointWinner = "",
                    MaxBetBadmintonNextPointWinner = "",
                    MaxBetBadmintonMatchHandicap = "",
                    MaxBetBaseball1H1st5inningsOU = "",
                    MaxBetBadmintonAllProductSetWinner = "",
                    MaxBetBaseball1H1st5inningsHDP = "",
                    MaxBetBadmintonRacetoPoints7911 = "",
                    MaxBetBaseballFTRunLineHDP = "",
                    MaxBetBaseballFTTotalRunOU = "",
                    MaxBetBaseballInningHitsRuns = "",
                    MaxBetBasketballFTHDP = "",
                    MaxBetBasketballFTOUOE = "",
                    MaxBetCricketFT1X2 = "",
                    MaxBetCricketMatchWinnerInningsOU = "",
                    MaxBetCricketNextOverOUPlayerOU = "",
                    MaxBetDartsLegsHDP = "",
                    MaxBetDartsMost180sHDP = "",
                    MaxBetFieldHockey1HHDP = "",
                    MaxBetFieldHockey1HOU = "",
                    MaxBetFieldHockey1QHDP = "",
                    MaxBetFieldHockey1QOU = "",
                    MaxBetFieldHockeyFTHDP = "",
                    MaxBetFieldHockeyFTOU = "",
                    MaxBetFootball1HHDP = "",
                    MaxBetFootball1HOU = "",
                    MaxBetFootball1Q3QHDP = "",
                    MaxBetFootball1Q3QOU = "",
                    MaxBetFootballFTHDP = "",
                    MaxBetFootballFTOU = "",
                    MaxBetHandball1HHDP = "",
                    MaxBetHandball1HOU = "",
                    MaxBetHandballFTHDP = "",
                    MaxBetHandballFTOU = "",
                    MaxBetIceHockey1st2ndPeriodsHDP = "",
                    MaxBetIceHockey1st2ndPeriodsOU = "",
                    MaxBetIceHockeyFTHDP = "",
                    MaxBetIceHockeyFTOU = "",
                    MaxBetNetball1HHDP = "",
                    MaxBetNetball1HOUOE = "",
                    MaxBetNetball1Q3QHDP = "",
                    MaxBetNetball1Q3QOUOE = "",
                    MaxBetNetballFTHDP = "",
                    MaxBetNetballFTOUOE = "",
                    MaxBetOtherSportsMatchWinner = "",
                    MaxBetPoolMatchWinnerHDP = "",
                    MaxBetPoolRacksWinnerHDP = "",
                    MaxBetSnookerFirst5FramesHDP = "",
                    MaxBetSnookerFirst8FramesHDP = "",
                    MaxBetSnookerFirst9FramesHDP = "",
                    MaxBetSnookerFramesWinnerHDPOUOE = "",
                    MaxBetSnookerMatchWinnerHDPOU = "",
                    MaxBetTableTennisSetWinnerHDP = "",
                    MaxBetTableTennisSetXWinnerHDP = "",
                    MaxBetTennisAllProductLS = "",
                    MaxBetTennisMostAces = "",
                    MaxBetTennisMostDoubleFaults = "",
                    MaxBetTennisNextGame = "",
                    MaxBetVolleyballSetsXPointWinnerHDP = "",
                    MaxBetVolleyballSetWinnerHDPOE = ""
                }
            };
        }

        private static AssignmentModelInExcel GetAssignmentModel(string maxbet)
        {
            return new AssignmentModelInExcel
            {
                MatchNo = "123",
                Away = "OKC",
                Home = "Toronto Raptor",
                League = "NBA",
                Sport = "Basketball",
                DateTime = "43831.645833333336",
                TraderBadmintonAllProductPointWinner = "Esther",
                TraderBadmintonAllProductSetWinner = "Curry",
                TraderBadmintonMatchHandicap = "Mira",
                TraderBadmintonNextPointWinner = "Mira",
                TraderBaseball1H1st5inningsHDP = "Carlos",
                TraderBaseballFTRunLineHDP = "Esther",
                TraderBasketball1HHDP = "Carlos",
                TraderBaseball1H1st5inningsOU = "Carlos",
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
                OtherTraderFieldHockey1HOU = "#Jack",
                MaxBetBadmintonAllProductPointWinner = "",
                MaxBetBadmintonNextPointWinner = "",
                MaxBetBadmintonMatchHandicap = "",
                MaxBetBaseball1H1st5inningsOU = "",
                MaxBetBadmintonAllProductSetWinner = "",
                MaxBetBaseball1H1st5inningsHDP = "",
                MaxBetBadmintonRacetoPoints7911 = "",
                MaxBetBaseballFTRunLineHDP = "",
                MaxBetBaseballFTTotalRunOU = "",
                MaxBetBaseballInningHitsRuns = "",
                MaxBetBasketballFTHDP = "",
                MaxBetBasketballFTOUOE = "",
                MaxBetCricketFT1X2 = "",
                MaxBetCricketMatchWinnerInningsOU = "",
                MaxBetCricketNextOverOUPlayerOU = "",
                MaxBetDartsLegsHDP = "",
                MaxBetDartsMost180sHDP = "",
                MaxBetFieldHockey1HHDP = "",
                MaxBetFieldHockey1HOU = "",
                MaxBetFieldHockey1QHDP = "",
                MaxBetFieldHockey1QOU = "",
                MaxBetFieldHockeyFTHDP = "",
                MaxBetFieldHockeyFTOU = "",
                MaxBetFootball1HHDP = "",
                MaxBetFootball1HOU = "",
                MaxBetFootball1Q3QHDP = "",
                MaxBetFootball1Q3QOU = "",
                MaxBetFootballFTHDP = maxbet,
                MaxBetFootballFTOU = "",
                MaxBetHandball1HHDP = "",
                MaxBetHandball1HOU = "",
                MaxBetHandballFTHDP = "",
                MaxBetHandballFTOU = "",
                MaxBetIceHockey1st2ndPeriodsHDP = "",
                MaxBetIceHockey1st2ndPeriodsOU = "",
                MaxBetIceHockeyFTHDP = "",
                MaxBetIceHockeyFTOU = "",
                MaxBetNetball1HHDP = "",
                MaxBetNetball1HOUOE = "",
                MaxBetNetball1Q3QHDP = "",
                MaxBetNetball1Q3QOUOE = "",
                MaxBetNetballFTHDP = "",
                MaxBetNetballFTOUOE = "",
                MaxBetOtherSportsMatchWinner = "",
                MaxBetPoolMatchWinnerHDP = "",
                MaxBetPoolRacksWinnerHDP = "",
                MaxBetSnookerFirst5FramesHDP = "",
                MaxBetSnookerFirst8FramesHDP = "",
                MaxBetSnookerFirst9FramesHDP = "",
                MaxBetSnookerFramesWinnerHDPOUOE = "",
                MaxBetSnookerMatchWinnerHDPOU = "",
                MaxBetTableTennisSetWinnerHDP = "",
                MaxBetTableTennisSetXWinnerHDP = "",
                MaxBetTennisAllProductLS = "",
                MaxBetTennisMostAces = "",
                MaxBetTennisMostDoubleFaults = "",
                MaxBetTennisNextGame = "",
                MaxBetVolleyballSetsXPointWinnerHDP = "",
                MaxBetVolleyballSetWinnerHDPOE = ""
            };
        }
    }
}
