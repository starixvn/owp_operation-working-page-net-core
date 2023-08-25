using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OWP.Kpi.LA.Models
{
    public class FakeData
    {
        public static IList<KpiInfo> FakeKpiInfos() => new List<KpiInfo>(){
            new KpiInfo{
                UserId=1,
                UserName="sa",

                Target="Level B",
                GapWithTarget= new TotalGameInfo{
                    VirtualMatches =-123,
                    MultipleMatches =123,
                    SpecialProducts =123,
                    FiveMajorLeagues=123,
                    MultipleProducts=-123,
                },
                AchievedGames=new TotalGameInfo{
                    VirtualMatches =-123,
                    MultipleMatches =123,
                    SpecialProducts =123,
                    FiveMajorLeagues=123,
                    MultipleProducts=-123,
                }
            },
            new KpiInfo{
                UserId=2,
                UserName="starix1",

                Target="Level B",
                GapWithTarget= new TotalGameInfo{
                    VirtualMatches =-123,
                    MultipleMatches =123,
                    SpecialProducts =123,
                    FiveMajorLeagues=123,
                    MultipleProducts=-123,
                },
                AchievedGames= new TotalGameInfo{
                    VirtualMatches =-123,
                    MultipleMatches =123,
                    SpecialProducts =123,
                    FiveMajorLeagues=123,
                    MultipleProducts=-123,
                }
            }
        };
    }
}