using System.ComponentModel.DataAnnotations;
using OWP.OtherSport.Validation.Models;
using OWP.OtherSport.Validation.Services;
using Xunit;

namespace OWP.OtherSport.UnitTests.Validation.Services;

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
            Assignments = GetAssignmentModels()
        };

        var traders = new List<Trader>
        {
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"},
            new() { UserId = 0, Username = "Curry"},
            new() { UserId = 11, Username = "Jack"},
            new() { UserId = 12, Username = "Tyler"},
            new() { UserId = 14, Username = "Carlos"},
            new() { UserId = 17, Username = "Jacob"}
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
                new()
                {
                    Away = "OKC",
                    Home = "Toronto Raptor",
                    League = "NBA",
                    Sport = "Basketball",
                    TraderBadmintonAllProductPointWinner = "starix3",
                    OtherTraderBadmintonRaceToPoints7911 = "#starix3"
                }
            }
        };
        var traders = new List<Trader>
        {
            new() { UserId = 1, Username = "starix1" },
            new() { UserId = 2, Username = "starix2" },
            new() { UserId = 0, Username = "starix3" }
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
            OtherTraderBadmintonRaceToPoints7911 = "#starix3"
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
            OtherTraderBadmintonRaceToPoints7911 = "#starix3"
        };
        var expected = new List<ValidationResult>
        {
            new("Match No must be number")
        };
        assignmentValidator.ValidateMatchNo(assignmentModelInExcel, actual);
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void ValidateInvalidTraders_ValidTrader_ResultIsEmpty()
    {
        var actual = new List<ValidationResult>();
        var invalidTrader = new List<Trader> {
            new() { UserId = 0, Username = "Bill" }
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
            new() { UserId = 0, Username = "Esther" },
            new() { UserId = 0, Username = "Lita" }
        };
        var assignmentModelList = GetAssignmentModels();
        var expected = new List<ValidationResult>
        {
            new("Invalid Trader Baseball FT Run Line (HDP): Esther; Other Trader Basketball 1H (OU/OE): Esther; Other Trader Badminton All Product (Set Winner): Lita")
        };

        assignmentValidator.ValidateTraders(assignmentModelList[0], actual, invalidTrader);
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void ValidateMaxbet_ValidMaxbet_ResultsIsEmpty()
    {
        var actual = new List<ValidationResult>();
        var assignmentModelList = new List<AssignmentModelInExcel>
        {
            GetAssignmentModel(string.Empty)
        };

        var expected = new List<ValidationResult>();

        assignmentValidator.ValidateMaxbetAndSpreadInString(assignmentModelList[0], actual);
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void ValidateMaxbet_InValidMaxbetAndSpread_ResultsIsEmpty()
    {
        var actual = new List<ValidationResult>();
        var assignmentModelList = new List<AssignmentModelInExcel>
        {
            GetAssignmentModel("esther")
        };

        var expected = new List<ValidationResult>
        {
            new("[Column: Max Bet Football FT (HDP)] - Error: Invalid MaxBet format"),
            new("[Column: Spread Basketball FT (HDP)] - Error: Invalid Spread format")
        };

        assignmentValidator.ValidateMaxbetAndSpreadInString(assignmentModelList[0], actual);
        DeepAssert.Equal(expected, actual);
    }

    [Fact]
    public void CustomValidateAssignment_ValidTrader_NotThrowsException()
    {
        var uploadingDataFromExcel = new UploadingDataFromExcel
        {
            Assignments = new List<AssignmentModelInExcel>
            {
                new()
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
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"},
            new() { UserId = 2, Username = "Lita"}
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
                new()
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
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"},
            new() { UserId = 0, Username = "Curry"}
        };
        var expected = new List<ValidationResult>
        {
            new("Invalid Trader Badminton All Product (Set Winner): Curry")
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
                new()
                {
                    Away = "OKC",
                    Home = "Toronto Raptor",
                    League = "NBA",
                    Sport = "Basketball",
                    TraderBadmintonAllProductPointWinner = "Esther",
                    DateTime = "43831.645833333336"
                },
                new()
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
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"}
        };
        var expected = new List<ValidationResult>
        {
            new("Duplicate Match: NBA | Toronto Raptor -vs- OKC")
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
                new()
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
            new() { UserId = 1, Username = "Esther"},
            new() { UserId = 10, Username = "Mira"}
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

        var actual = AssignmentValidatorImp.GroupByDuplicateMatchMessage(assignments, "Duplicate Match");

        DeepAssert.Equal(expected, actual);
    }

    private static List<AssignmentModelInExcel> GetAssignmentModels()
    {
        return new List<AssignmentModelInExcel>
        {
            new()
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
                TraderBaseballFTRunLineHDP = "#Esther",
                TraderBasketball1HHDP = "Carlos",
                TraderBaseball1H1st5inningsOU = "Carlos",
                TraderBaseballInningHitsRuns = "Jacob",
                TraderBasketball1HOUOE = "Mira",
                TraderBadmintonRaceToPoints7911 = "Jacob",
                TraderBasketball1Q3QHDP = "#Tyler",
                TraderBasketballFTHDP = "#Tyler",
                OtherTraderBadmintonAllProductSetWinner = "#Lita",
                OtherTraderBaseball1H1st5inningsHDP = "#Tyler",
                OtherTraderBadmintonRaceToPoints7911 = "#Carlos",
                OtherTraderBaseball1H1st5inningsOU = "#Carlos",
                OtherTraderBasketball1HOUOE = "#Esther",
                OtherTraderBasketballFTOUOE = "#Jack",
                OtherTraderCricketNextOverOUPlayerOU = "#Jack",
                OtherTraderFieldHockeyFTHDP = "#Carlos",
                OtherTraderFieldHockey1HOU = "#Jack",
                MaxBetBasketballFTHDP = "",
                MaxBetBasketballFTOUOE = "",
                MaxBetBasketballFTOE = "",
                MaxBetFootballFTHDP = "",
                MaxBetFootball1HHDP = "",
                MaxBetFootball1Q3QHDP = "",
                MaxBetFootballFTOU = "50000",
                MaxBetFootball1HOU = "",
                MaxBetFootball1Q3QOU = "",
                MaxBetFieldHockeyFTHDP = "",
                MaxBetFieldHockey1HHDP = "",
                MaxBetFieldHockey1QHDP = "",
                MaxBetFieldHockeyFTOU = "",
                MaxBetFieldHockey1HOU = "",
                MaxBetFieldHockey1QOU = "",
                MaxBetIceHockeyFTHDP = "",
                MaxBetIceHockey1st2ndPeriodsHDP = "",
                MaxBetIceHockeyFTOU = "",
                MaxBetIceHockey1st2ndPeriodsOU = "",
                MaxBetOtherSportsMatchWinner = "",
                MaxBetOtherSportsFTHDP = "",
                MaxBetOtherSports1HHDP = "",
                MaxBetOtherSportsFTOU = "",
                MaxBetOtherSports1HOU = "",
                MaxBetDartsLegsHDP = "",
                MaxBetDartsMost180sHDP = "",
                MaxBetPoolMatchWinnerHDP = "",
                MaxBetPoolRacksWinnerHDP = "",
                MaxBetTableTennisSetWinnerHDP = "",
                MaxBetTableTennisSetXWinnerHDP = "",
                MaxBetVolleyballSetWinnerHDPOE = "",
                MaxBetVolleyballSetWinnerOE = "",
                MaxBetVolleyballSetsXPointWinnerHDP = "",
                MaxBetHandballFTHDP = "",
                MaxBetHandball1HHDP = "",
                MaxBetHandballFTOU = "",
                MaxBetHandball1HOU = "",
                MaxBetNetballFTHDP = "",
                MaxBetNetball1HHDP = "",
                MaxBetNetball1Q3QHDP = "",
                MaxBetNetballFTOUOE = "",
                MaxBetNetballFTOE = "",
                MaxBetNetball1HOUOE = "",
                MaxBetNetball1HOE = "",
                MaxBetNetball1Q3QOUOE = "",
                MaxBetNetball1Q3QOE = "",
                MaxBetCricketMatchWinnerInningsOU = "",
                MaxBetCricketNextOverOUPlayerOU = "",
                MaxBetCricketFT1X2 = "",
                MaxBetSnookerMatchWinnerHDPOU = "",
                MaxBetSnookerMatchWinnerOU = "",
                MaxBetSnookerFramesWinnerHDPOUOE = "",
                MaxBetSnookerFramesWinnerOU = "",
                MaxBetSnookerFramesWinnerOE = "",
                MaxBetSnookerFirst5FramesHDP = "",
                MaxBetSnookerFirst8FramesHDP = "",
                MaxBetSnookerFirst9FramesHDP = "",
                MaxBetBaseballFTRunLineHDP = "",
                MaxBetBaseball1H1st5inningsHDP = "",
                MaxBetBaseballFTTotalRunOU = "",
                MaxBetBaseball1H1st5inningsOU = "",
                MaxBetBaseballInningHitsRuns = "",
                MaxBetBadmintonAllProductSetWinner = "",
                MaxBetBadmintonAllProductPointWinner = "",
                MaxBetBadmintonRaceToPoints7911 = "",
                MaxBetBadmintonNextPointWinner = "",
                MaxBetBadmintonMatchHandicap = "",
                MaxBetTennisAllProductLS = "",
                MaxBetTennisNextGame = "",
                MaxBetTennisMostAces = "",
                MaxBetTennisMostDoubleFaults = "",
                SpreadBasketballFTHDP = "",
                SpreadBasketballFTOUOE = "",
                SpreadBasketballFTOE = "",
                SpreadFootballFTHDP = "",
                SpreadFootball1HHDP = "",
                SpreadFootball1Q3QHDP = "",
                SpreadFootballFTOU = "",
                SpreadFootball1HOU = "",
                SpreadFootball1Q3QOU = "",
                SpreadFieldHockeyFTHDP = "",
                SpreadFieldHockey1HHDP = "",
                SpreadFieldHockey1QHDP = "",
                SpreadFieldHockeyFTOU = "",
                SpreadFieldHockey1HOU = "",
                SpreadFieldHockey1QOU = "",
                SpreadIceHockeyFTHDP = "",
                SpreadIceHockey1st2ndPeriodsHDP = "",
                SpreadIceHockeyFTOU = "",
                SpreadIceHockey1st2ndPeriodsOU = "",
                SpreadOtherSportsMatchWinner = "",
                SpreadOtherSportsFTHDP = "",
                SpreadOtherSports1HHDP = "",
                SpreadOtherSportsFTOU = "",
                SpreadOtherSports1HOU = "",
                SpreadDartsLegsHDP = "",
                SpreadDartsMost180sHDP = "",
                SpreadPoolMatchWinnerHDP = "",
                SpreadPoolRacksWinnerHDP = "",
                SpreadTableTennisSetWinnerHDP = "",
                SpreadTableTennisSetXWinnerHDP = "",
                SpreadVolleyballSetWinnerHDPOE = "",
                SpreadVolleyballSetWinnerOE = "",
                SpreadVolleyballSetsXPointWinnerHDP = "",
                SpreadHandballFTHDP = "",
                SpreadHandball1HHDP = "",
                SpreadHandballFTOU = "",
                SpreadHandball1HOU = "",
                SpreadNetballFTHDP = "",
                SpreadNetball1HHDP = "",
                SpreadNetball1Q3QHDP = "",
                SpreadNetballFTOUOE = "",
                SpreadNetballFTOE = "",
                SpreadNetball1HOUOE = "",
                SpreadNetball1HOE = "",
                SpreadNetball1Q3QOUOE = "",
                SpreadNetball1Q3QOE = "",
                SpreadCricketMatchWinnerInningsOU = "",
                SpreadCricketNextOverOUPlayerOU = "",
                SpreadCricketFT1X2 = "",
                SpreadSnookerMatchWinnerHDPOU = "",
                SpreadSnookerMatchWinnerOU = "",
                SpreadSnookerFramesWinnerHDPOUOE = "",
                SpreadSnookerFramesWinnerOU = "",
                SpreadSnookerFramesWinnerOE = "",
                SpreadSnookerFirst5FramesHDP = "",
                SpreadSnookerFirst8FramesHDP = "",
                SpreadSnookerFirst9FramesHDP = "",
                SpreadBaseballFTRunLineHDP = "",
                SpreadBaseball1H1st5inningsHDP = "",
                SpreadBaseballFTTotalRunOU = "",
                SpreadBaseball1H1st5inningsOU = "",
                SpreadBaseballInningHitsRuns = "",
                SpreadBadmintonAllProductSetWinner = "",
                SpreadBadmintonAllProductPointWinner = "",
                SpreadBadmintonRaceToPoints7911 = "",
                SpreadBadmintonNextPointWinner = "",
                SpreadBadmintonMatchHandicap = "",
                SpreadTennisAllProductLS = "",
                SpreadTennisNextGame = "",
                SpreadTennisMostAces = "",
                SpreadTennisMostDoubleFaults = ""
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
            TraderBadmintonAllProductSetWinner = "#Curry abc",
            TraderBadmintonMatchHandicap = "Mira",
            TraderBadmintonNextPointWinner = "Mira",
            TraderBaseball1H1st5inningsHDP = "Carlos",
            TraderBaseballFTRunLineHDP = "Esther",
            TraderBasketball1HHDP = "Carlos",
            TraderBaseball1H1st5inningsOU = "Carlos",
            TraderBaseballInningHitsRuns = "Jacob",
            TraderBasketball1HOUOE = "Mira",
            TraderBadmintonRaceToPoints7911 = "Jacob",
            TraderBasketball1Q3QHDP = "#Tyler",
            TraderBasketballFTHDP = "#Tyler",
            OtherTraderBadmintonAllProductSetWinner = "#Lita",
            OtherTraderBaseball1H1st5inningsHDP = "#Tyler",
            OtherTraderBadmintonRaceToPoints7911 = "#Carlos",
            OtherTraderBaseball1H1st5inningsOU = "#Carlos",
            OtherTraderBasketball1HOUOE = "#Esther",
            OtherTraderBasketballFTOUOE = "#Jack",
            OtherTraderCricketNextOverOUPlayerOU = "#Jack",
            OtherTraderFieldHockeyFTHDP = "#Carlos",
            OtherTraderFieldHockey1HOU = "#Jack",
            MaxBetBasketballFTHDP = "",
            MaxBetBasketballFTOUOE = "",
            MaxBetBasketballFTOE = "",
            MaxBetFootballFTHDP = maxbet,
            MaxBetFootball1HHDP = "",
            MaxBetFootball1Q3QHDP = "",
            MaxBetFootballFTOU = "",
            MaxBetFootball1HOU = "",
            MaxBetFootball1Q3QOU = "",
            MaxBetFieldHockeyFTHDP = "",
            MaxBetFieldHockey1HHDP = "",
            MaxBetFieldHockey1QHDP = "",
            MaxBetFieldHockeyFTOU = "",
            MaxBetFieldHockey1HOU = "",
            MaxBetFieldHockey1QOU = "",
            MaxBetIceHockeyFTHDP = "",
            MaxBetIceHockey1st2ndPeriodsHDP = "",
            MaxBetIceHockeyFTOU = "",
            MaxBetIceHockey1st2ndPeriodsOU = "",
            MaxBetOtherSportsMatchWinner = "",
            MaxBetOtherSportsFTHDP = "",
            MaxBetOtherSports1HHDP = "",
            MaxBetOtherSportsFTOU = "",
            MaxBetOtherSports1HOU = "",
            MaxBetDartsLegsHDP = "",
            MaxBetDartsMost180sHDP = "",
            MaxBetPoolMatchWinnerHDP = "",
            MaxBetPoolRacksWinnerHDP = "",
            MaxBetTableTennisSetWinnerHDP = "",
            MaxBetTableTennisSetXWinnerHDP = "",
            MaxBetVolleyballSetWinnerHDPOE = "",
            MaxBetVolleyballSetWinnerOE = "",
            MaxBetVolleyballSetsXPointWinnerHDP = "",
            MaxBetHandballFTHDP = "",
            MaxBetHandball1HHDP = "",
            MaxBetHandballFTOU = "",
            MaxBetHandball1HOU = "",
            MaxBetNetballFTHDP = "",
            MaxBetNetball1HHDP = "",
            MaxBetNetball1Q3QHDP = "",
            MaxBetNetballFTOUOE = "",
            MaxBetNetballFTOE = "",
            MaxBetNetball1HOUOE = "",
            MaxBetNetball1HOE = "",
            MaxBetNetball1Q3QOUOE = "",
            MaxBetNetball1Q3QOE = "",
            MaxBetCricketMatchWinnerInningsOU = "",
            MaxBetCricketNextOverOUPlayerOU = "",
            MaxBetCricketFT1X2 = "",
            MaxBetSnookerMatchWinnerHDPOU = "",
            MaxBetSnookerMatchWinnerOU = "",
            MaxBetSnookerFramesWinnerHDPOUOE = "",
            MaxBetSnookerFramesWinnerOU = "",
            MaxBetSnookerFramesWinnerOE = "",
            MaxBetSnookerFirst5FramesHDP = "",
            MaxBetSnookerFirst8FramesHDP = "",
            MaxBetSnookerFirst9FramesHDP = "",
            MaxBetBaseballFTRunLineHDP = "",
            MaxBetBaseball1H1st5inningsHDP = "",
            MaxBetBaseballFTTotalRunOU = "",
            MaxBetBaseball1H1st5inningsOU = "",
            MaxBetBaseballInningHitsRuns = "",
            MaxBetBadmintonAllProductSetWinner = "",
            MaxBetBadmintonAllProductPointWinner = "",
            MaxBetBadmintonRaceToPoints7911 = "",
            MaxBetBadmintonNextPointWinner = "",
            MaxBetBadmintonMatchHandicap = "",
            MaxBetTennisAllProductLS = "",
            MaxBetTennisNextGame = "",
            MaxBetTennisMostAces = "",
            MaxBetTennisMostDoubleFaults = "",
            SpreadBasketballFTHDP = maxbet,
            SpreadBasketballFTOUOE = "",
            SpreadBasketballFTOE = "",
            SpreadFootballFTHDP = "",
            SpreadFootball1HHDP = "",
            SpreadFootball1Q3QHDP = "",
            SpreadFootballFTOU = "",
            SpreadFootball1HOU = "",
            SpreadFootball1Q3QOU = "",
            SpreadFieldHockeyFTHDP = "",
            SpreadFieldHockey1HHDP = "",
            SpreadFieldHockey1QHDP = "",
            SpreadFieldHockeyFTOU = "",
            SpreadFieldHockey1HOU = "",
            SpreadFieldHockey1QOU = "",
            SpreadIceHockeyFTHDP = "",
            SpreadIceHockey1st2ndPeriodsHDP = "",
            SpreadIceHockeyFTOU = "",
            SpreadIceHockey1st2ndPeriodsOU = "",
            SpreadOtherSportsMatchWinner = "",
            SpreadOtherSportsFTHDP = "",
            SpreadOtherSports1HHDP = "",
            SpreadOtherSportsFTOU = "",
            SpreadOtherSports1HOU = "",
            SpreadDartsLegsHDP = "",
            SpreadDartsMost180sHDP = "",
            SpreadPoolMatchWinnerHDP = "",
            SpreadPoolRacksWinnerHDP = "",
            SpreadTableTennisSetWinnerHDP = "",
            SpreadTableTennisSetXWinnerHDP = "",
            SpreadVolleyballSetWinnerHDPOE = "",
            SpreadVolleyballSetWinnerOE = "",
            SpreadVolleyballSetsXPointWinnerHDP = "",
            SpreadHandballFTHDP = "",
            SpreadHandball1HHDP = "",
            SpreadHandballFTOU = "",
            SpreadHandball1HOU = "",
            SpreadNetballFTHDP = "",
            SpreadNetball1HHDP = "",
            SpreadNetball1Q3QHDP = "",
            SpreadNetballFTOUOE = "",
            SpreadNetballFTOE = "",
            SpreadNetball1HOUOE = "",
            SpreadNetball1HOE = "",
            SpreadNetball1Q3QOUOE = "",
            SpreadNetball1Q3QOE = "",
            SpreadCricketMatchWinnerInningsOU = "",
            SpreadCricketNextOverOUPlayerOU = "",
            SpreadCricketFT1X2 = "",
            SpreadSnookerMatchWinnerHDPOU = "",
            SpreadSnookerMatchWinnerOU = "",
            SpreadSnookerFramesWinnerHDPOUOE = "",
            SpreadSnookerFramesWinnerOU = "",
            SpreadSnookerFramesWinnerOE = "",
            SpreadSnookerFirst5FramesHDP = "",
            SpreadSnookerFirst8FramesHDP = "",
            SpreadSnookerFirst9FramesHDP = "",
            SpreadBaseballFTRunLineHDP = "",
            SpreadBaseball1H1st5inningsHDP = "",
            SpreadBaseballFTTotalRunOU = "",
            SpreadBaseball1H1st5inningsOU = "",
            SpreadBaseballInningHitsRuns = "",
            SpreadBadmintonAllProductSetWinner = "",
            SpreadBadmintonAllProductPointWinner = "",
            SpreadBadmintonRaceToPoints7911 = "",
            SpreadBadmintonNextPointWinner = "",
            SpreadBadmintonMatchHandicap = "",
            SpreadTennisAllProductLS = "",
            SpreadTennisNextGame = "",
            SpreadTennisMostAces = "",
            SpreadTennisMostDoubleFaults = ""
        };
    }
}