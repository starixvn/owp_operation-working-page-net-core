using System;
using System.Collections.Generic;
using OWP.OtherSport.Models;

namespace OWP.OtherSport.fake
{
    public static class FakeData
    {
        public static Assignment FakeAssignment()
        {
            return new Assignment
            {
                No = 1,
                LeagueId = 101,
                PreMatchLeagueId = 17038,
                LeagueName = "*SPAIN PRIMERA LALIGA",
                Status = "Live",
                KOTime = DateTime.Now.ToString(),
                HomeName = "CD Universidad Catolica(V)",
                AwayName = "AEK Athens (V)",
                Remark = "TA ROBOT_Draw will PK",
                Traders = new List<Trader>
                {
                    new Trader
                    {
                          Bettype= "FT HDP",
                          Assignee= "SA",
                          OtherAssignee= "starix14"
                    },
                     new Trader
                    {
                         Bettype= "1h HDP",
                         Assignee= "SA2"
                    },
                      new Trader
                    {
                        Bettype= "1Q & 3Q HDP",
                        Assignee= "SA3"
                    },
                       new Trader
                    {
                       Bettype= "FT OU/OE",
                       Assignee= "SA4"
                    },
                        new Trader
                    {
                        Bettype= "1H OUOE",
                        Assignee= "SA5"
                    }
                },
                MaxBets= new List<Maxbet>
                {
                    new Maxbet
                    {
                        Bettype= "FT HDP",
                        Value= "3200"
                    },
                     new Maxbet
                    {
                         Bettype= "1h HDP",
                        Value= "1500"
                    },
                      new Maxbet
                    {
                        Bettype= "1Q & 3Q HDP",
                        Value= "1500"
                    },
                       new Maxbet
                    {
                         Bettype= "1H OUOE",
                        Value= "1300"
                    }
                }
            };
        }
    }
}
