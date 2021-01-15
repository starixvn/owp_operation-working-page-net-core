using System;
using System.Collections.Generic;
using OWP.OtherSport.EventAssignment.Models;
using OWP.OtherSport.Submit.Models;

namespace OWP.OtherSport.fake
{
    public static class FakeData
    {
        public static List<EventAssignmentModel> FakeAssignment()
        {
            return new List<EventAssignmentModel>
            {
                 new EventAssignmentModel
                 {
                     OSMatchId= 123,
                      League= "*SPAIN PRIMERA LALIGA",
                      MatchStatus= "Live",
                      DateTime= DateTime.Now,
                      Home= "CD Universidad Catolica(V)_Home",
                      Away= "AEK Athens (V)_Away",
                      Remark= "TA ROBOT_Draw will PK",
                      TraderMaxbetModels= new List<TraderMaxBetModel>
                        {
                          new TraderMaxBetModel
                          {
                               BetTypeName=  "FT HDP",
                               TraderAssignment=new TraderAssignment
                               {
                                   OSMatchId=123,
                                    BetTypeName=  "FT HDP",
                                    MainTrader="sa",
                                    AssignmentNote= "abc #sa abc"
                                },
                               Maxbet= new MatchMaxBet
                               {
                                    OSMatchId=123,
                                    BetTypeName=  "FT HDP",
                                    MaxBet= 123m
                               }
                          }
                      },

                       IPTV= "ipTv aaaaa",
                       WB= "Streaming bbbbbbb",
                       Provider= "Provider JAck",
                       AutoLink="Yes",
                       Category="Category",
                       NoHDP="NoHDP",
                       MatchOffer="MatchOffer",
                       OtherProvider="OtherProvider",
                       Service="Medium"
                 }
            };
        }
    }
}
