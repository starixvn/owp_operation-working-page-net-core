using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OWP.OtherSport.Notification.Services;
using OWP.OtherSport.Submit.Models;
using Xunit;

namespace OWP.OtherSport.UnitTests.Notification.Services
{
    public class ChangedMatchSettingGeneratorUnitTests
    {
        [Fact]
        public void GetListValueChangedOfMatchSetting_OldvalueHasMaxbetChange_NewvalueHasMaxbetChange()
        {
            var oldMatchSetting = new MatchMaxBet
            {
                MaxBet= 120000,
            };
            var newMatchSetting = new MatchMaxBet
            {
                MaxBet = 110000,
            };
            var actual = ChangedMatchSettingGeneratorImp.GetListValueChangedOfMatchSetting(oldMatchSetting, newMatchSetting, "FT (HDP)");
            Assert.Equal(1, 2);
        }
    }
}
