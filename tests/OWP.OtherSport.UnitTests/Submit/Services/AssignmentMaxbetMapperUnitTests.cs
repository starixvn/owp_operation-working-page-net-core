namespace OWP.OtherSport.UnitTests.Submit.Services
{
    using OWP.OtherSport.Submit.Contants;
    using OWP.OtherSport.Submit.Services;
    using OWP.OtherSport.Validation.Models;
    using Xunit;

    public class AssignmentMaxbetMapperUnitTests
    {
        private readonly AssignmentMaxbetMapperImp services;

        public AssignmentMaxbetMapperUnitTests()
        {
            services = new AssignmentMaxbetMapperImp();
        }

        [Fact]
        public void MapBySport_()
        {
            var assignment = GetAssignmentModel(Sport.Football);
            var actual = services.MapBySport(assignment);
        }

        private AssignmentModel GetAssignmentModel(string sport)
        {
            return new AssignmentModel
            {
                Sport = sport,

            };
        }
    }
}
