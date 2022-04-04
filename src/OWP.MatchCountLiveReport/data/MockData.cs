using OWP.MatchCountLiveReport.Models;

namespace OWP.MatchCountLiveReport.data
{
  public static class MockData
  {
    public static IList<ReportInfo> MockTestData()
    => new List<ReportInfo>{
          new ReportInfo{
            Bettype="Real Soccer",
            Total=100,
            Mj=100,
            M1=100,
            M2=100,
            M3=100,
            M4=100,
            Others=100,
            Others1=100,
            Others2=100,
            Others3=100,
            Others4=100,
            Others5=100,
            Others6=100,
            Unknown=100,
            IsSaba= false
          },
          new ReportInfo{
            No=1,
            Bettype="Main",
            Total=99,
            Mj=99,
            M1=99,
            M2=99,
            M3=99,
            M4=99,
            Others=99,
            Others1=99,
            Others2=99,
            Others3=99,
            Others4=99,
            Others5=99,
            Others6=99,
            Unknown=99,
            IsSaba= false
          },
          new ReportInfo{
            No=2,
            Bettype="Corners",
            Total=88,
            Mj=88,
            M1=88,
            M2=88,
            M3=88,
            M4=88,
            Others=88,
            Others1=88,
            Others2=88,
            Others3=88,
            Others4=88,
            Others5=88,
            Others6=88,
            Unknown=88,
            IsSaba= false
          },
          new ReportInfo{
            Bettype="SABA",
            Total=100,
            Mj=100,
            M1=100,
            M2=100,
            M3=100,
            M4=100,
            Others=100,
            Others1=100,
            Others2=100,
            Others3=100,
            Others4=100,
            Others5=100,
            Others6=100,
            Unknown=100,
            IsSaba= true
          },
          new ReportInfo{
            No=1,
            Bettype="SABA CLUB FRIENDLY Virtual PES 21 - 15 Mins Play",
            Total=99,
            Mj=99,
            M1=99,
            M2=99,
            M3=99,
            M4=99,
            Others=99,
            Others1=99,
            Others2=99,
            Others3=99,
            Others4=99,
            Others5=99,
            Others6=99,
            Unknown=99,
            IsSaba= false
          },
          new ReportInfo{
            No=2,
            Bettype="SABA INTERNATIONAL FRIENDLY Virtual PES 21 - 20 Mins Play",
            Total=88,
            Mj=88,
            M1=88,
            M2=88,
            M3=88,
            M4=88,
            Others=88,
            Others1=88,
            Others2=88,
            Others3=88,
            Others4=88,
            Others5=88,
            Others6=88,
            Unknown=88,
            IsSaba= false
          }
    };
  }
}