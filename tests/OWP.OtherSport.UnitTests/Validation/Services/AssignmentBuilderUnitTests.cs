namespace OWP.OtherSport.UnitTests.Validation.Services
{
    using AutoMapper;
    using OWP.OtherSport.Validation.Models;
    using OWP.OtherSport.Validation.Services;
    using System;
    using System.Collections.Generic;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class AssignmentBuilderUnitTests
    {
        private readonly AssignmentBuilderImp assignmentBuilder;
        private readonly AssignmentMapper assignmentMapper;

        public AssignmentBuilderUnitTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssignmentModelInExcel, AssignmentModel>()
                   .ForMember(dest => dest.DateTime, opt => opt.Ignore());
            });
            var iMapper = config.CreateMapper();

            assignmentMapper = new AssignmentMapperImp(iMapper);
            assignmentBuilder = new AssignmentBuilderImp(assignmentMapper);
        }

        [Fact]
        public void BuildAssignment_Always_ReturnAssignmentModels()
        {
            var traders = new List<Trader>
            {
                new Trader{ UserId = 1, Username = "Esther"},
                new Trader{ UserId = 10, Username = "Mira"},
                new Trader{ UserId = 2, Username = "Lita"}
            };

            var assignmentModelInExcels = GetAssignmentModelInExcels();

            var expected = GetAssignmentModels();
            var actual = assignmentBuilder.BuildAssignment(traders, assignmentModelInExcels);

            DeepAssert.Equal(expected, actual, "HashKey");
        }

        private static List<AssignmentModel> GetAssignmentModels()
        {
            var model = new AssignmentModel
            {
                Reporter= null,
                Sport = "Basketball",
                Home = "Golden State Warrior",
                Away = "LA Laker",
                MatchStatus = "Completed",
                DateTime = DateTime.Parse("12/10/2020 12:00 AM"),
                League = "NBA",
                AutoLink = "YES"
            };

            var model2 = new AssignmentModel
            {
                Reporter = null,
                Sport = "Ice Hookey",
                Home = "Golden State Warrior",
                Away = "LA Laker",
                MatchStatus = "Completed",
                DateTime = DateTime.Parse("12/10/2020 12:00 AM"),
                League = "Hookey",
                AutoLink = "YES"
            };

            var assignmentModels = new List<AssignmentModel>
            {
                model, model2
            };

            return assignmentModels;
        }

        private static List<AssignmentModelInExcel> GetAssignmentModelInExcels()
        {
            var excelModel = new AssignmentModelInExcel
            {
                Sport = "Basketball",
                Home = "Golden State Warrior",
                Away = "LA Laker",
                MatchStatus = "Completed",
                DateTime = "44175",
                League = "NBA",
                AutoLink = "YES"
            };

            var excelModel2 = new AssignmentModelInExcel
            {
                Sport = "Ice Hookey",
                Home = "Golden State Warrior",
                Away = "LA Laker",
                MatchStatus = "Completed",
                DateTime = "44175",
                League = "Hookey",
                AutoLink = "YES"
            };

            var assignmentModelInExcels = new List<AssignmentModelInExcel>
            {
                excelModel, excelModel2
            };

            return assignmentModelInExcels;
        }
    }
}
