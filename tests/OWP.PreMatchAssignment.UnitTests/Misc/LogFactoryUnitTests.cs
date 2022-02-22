namespace OWP.PreMatchAssignment.UnitTests.Misc
{
    using Models;
    using OWP.PreMatchAssignment.Misc;

    using Xunit;

    public class LogFactoryUnitTests
    {
        [Fact]
        public void BuildColumn_WithUpdateAssignmentAction_ShouldReturnData()
        {
            //Arrange
            var logFactory = new LogFactoryImp();
            var updateAssignments = new List<UpdateAssignmentLogModel>
            {
                new UpdateAssignmentLogModel
                {
                    Bettype = 1,
                    IsTakeOver = true,
                    UserId = 8,
                    Username = "sa",
                    AssignmentType = 1
                }
            };
            var data = "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"1\"/></r>";
            var expected = "HDP T.Over: #sa";

            //Act
            var actual = logFactory.BuildColumn(LogActionConst.UpdateAssignment, data);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildColumn_WithEmptyUpdateAssignmentAction_ShouldReturnEmpty()
        {
            //Arrange
            var logFactory = new LogFactoryImp();
            var data = string.Empty;
            var expected = string.Empty;

            //Act
            var actual = logFactory.BuildColumn(LogActionConst.UpdateAssignment, data);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildColumn_WithUpdateRemarkAction_ShouldReturnData()
        {
            //Arrange
            var logFactory = new LogFactoryImp();

            var data = "<r><z plid=\"2019\" rm=\"cdcd\"/></r>";
            var expected = "cdcd";

            //Act
            var actual = logFactory.BuildColumn(LogActionConst.UpdateRemark, data);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildColumn_WithUpdateStartTimeAction_ShouldReturnData()
        {
            //Arrange
            var logFactory = new LogFactoryImp();
            var data = "<r><z d=\"2020-08-20\" t=\"14:30:00\"/></r>";
            var expected = "Date: 08/20/2020, Start Time: 02:30 PM";

            //Act
            var actual = logFactory.BuildColumn(LogActionConst.ChangeStartTime, data);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildColumn_WithUpdateStartTimeAction_ShouldReturnEmpty()
        {
            //Arrange
            var logFactory = new LogFactoryImp();

            var data = string.Empty;

            //Act
            var actual = logFactory.BuildColumn(LogActionConst.ChangeStartTime, data);

            //Assert
            Assert.Equal(data, actual);
        }

        [Fact]
        public void BuildColumn_WithRecoverLeagueAction_ShouldReturnEmpty()
        {
            //Arrange
            var logFactory = new LogFactoryImp();
            var data = "<r><z plid=\"2007\"/></r>";
            var expected = string.Empty;

            //Act
            var actual = logFactory.BuildColumn("Recover League", data);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void BuildColumn_WithUpdateOtherAssignmentAction_ShouldReturnData()
        {
            //Arrange
            var logFactory = new LogFactoryImp();
            var data = "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"2\"/></r>";
            var expected = "HDP T.Over: #sa";

            //Act
            var actual = logFactory.BuildColumn(LogActionConst.UpdateAssignment, data);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(LogActionConst.UpdateAssignment, "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"2\"/></r>", LogActionConst.UpdateOtherAssignment)]
        public void GetLogType_WithUpdateOtherAssignmentAction_ShouldReturnUpdateOtherAssignment(string action, string data, string expectedAction)
        {
            var logFactory = new LogFactoryImp();

            //Act
            var actual = logFactory.GetLogType(action, data);

            //Assert
            Assert.Equal(expectedAction, actual);
        }

        [Theory]
        [InlineData(LogActionConst.UpdateAssignment, "<r><z plid=\"2047\" uid=\"8\" un=\"sa\" bt=\"1\" ito=\"1\" at=\"1\"/></r>", LogActionConst.UpdateAssignment)]
        public void GetLogType_WithUpdateAssignmentAction_ShouldReturnUpdateAssignment(string action, string data, string expectedAction)
        {
            var logFactory = new LogFactoryImp();

            //Act
            var actual = logFactory.GetLogType(action, data);

            //Assert
            Assert.Equal(expectedAction, actual);
        }

        [Theory]
        [InlineData(LogActionConst.UpdateRemark, "", LogActionConst.UpdateRemark)]
        public void GetLogType_WithoutUpdateAssignmentAction_ShouldReturnAction(string action, string data, string expectedAction)
        {
            var logFactory = new LogFactoryImp();

            //Act
            var actual = logFactory.GetLogType(action, data);

            //Assert
            Assert.Equal(expectedAction, actual);
        }
    }
}