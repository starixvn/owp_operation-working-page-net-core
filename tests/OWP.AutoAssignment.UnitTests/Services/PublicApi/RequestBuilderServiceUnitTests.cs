namespace OWP.AutoAssignment.UnitTests.Services.PublicApi
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoAssignment.Services.PublicApi;
    using Models.PublicApi;
    using NSubstitute;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class RequestBuilderServiceUnitTests
    {
        private readonly Repository repository;
        private readonly SchedulingMatchBuilderService matchService;
        private readonly LAUserService laService;
        private readonly TraderUserService traderService;
        public RequestBuilderServiceUnitTests()
        {
            repository = Substitute.For<Repository>();
            matchService = Substitute.For<SchedulingMatchBuilderServiceImp>();
            laService = Substitute.For<LAUserServiceImp>();
            traderService = Substitute.For<TraderUserServiceImp>();
        }

        [Fact]
        public async Task BuildRequestSenderModel_Always_ReturnListSender()
        {

            var service = new RequestBuilderImp();
        }

    }
}
