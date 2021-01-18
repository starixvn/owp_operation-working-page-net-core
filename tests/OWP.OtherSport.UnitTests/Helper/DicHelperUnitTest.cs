namespace OWP.OtherSport.UnitTests.Helper
{
    using System;
    using System.Collections.Generic;
    using OWP.OtherSport.Submit.Models;
    using Xunit;
    using Xunit.Asserts.Compare;

    public class TestBaseFixture : IDisposable
    {
        public Dictionary<int, List<MatchAssignment>> DicMatchAssignment;
        public TestBaseFixture()
        {
            Setup();
        }

        public void Setup()
        {
            DicMatchAssignment = new Dictionary<int, List<MatchAssignment>>()
            {
                {123, new List<MatchAssignment>
                {
                    new MatchAssignment
                    {
                        OSMatchId= 123,
                        BetTypeName="1Q & 3Q (OU/OE)"
                    }
                }},
                 {124, new List<MatchAssignment>
                {
                    new MatchAssignment
                    {
                        OSMatchId= 124,
                        BetTypeName="FT Run Line (HDP)"
                    }
                }}
            };
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }

    public class DicHelperUnitTest : IClassFixture<TestBaseFixture>
    {
        readonly TestBaseFixture setup;
        public DicHelperUnitTest(TestBaseFixture setup)
        {
            this.setup = setup;
        }

        [Fact]
        public void ShouldReturnNewObject_WhenKeyNotExistedInDic()
        {
            var actual = DicHelper.GetValueFromDicBykey(setup.DicMatchAssignment, 125);
            var expect = new List<MatchAssignment>();
            DeepAssert.Equal(expect, actual);
        }

        [Fact]
        public void ShouldReturnValue_WhenKeyBelongToDic()
        {
            var actual = DicHelper.GetValueFromDicBykey(setup.DicMatchAssignment, 123);
            var expect = new List<MatchAssignment>
            {
                new MatchAssignment
                {
                    OSMatchId = 123,
                    BetTypeName = "1Q & 3Q (OU/OE)"
                }
            };
            DeepAssert.Equal(expect, actual);
        }
    }
}
