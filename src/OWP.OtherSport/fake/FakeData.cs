//using System;
//using System.Collections.Generic;
//using OWP.OtherSport.EventAssignment.Models;
//using OWP.OtherSport.Submit.Models;

//namespace OWP.OtherSport.fake
//{
//    public static class FakeData
//    {
//        public static List<EventAssignmentModel> FakeAssignment()
//        {
//            return new List<EventAssignmentModel>
//            {
//                 new EventAssignmentModel
//                 {
//                     OSMatchId= 123,
//                      League= "*SPAIN PRIMERA LALIGA",
//                      MatchStatus= "Live",
//                      DateTime= DateTime.Now,
//                      Home= "CD Universidad Catolica(V)_Home",
//                      Away= "AEK Athens (V)_Away",
//                      Remark= "TA ROBOT_Draw will PK",
//                      Assignments= new List<Assignment>
//                        {
//                          new Assignment
//                          {

//                          },
//                          new Assignment
//                          {

//                          }
//                        },
//                      MaxBets = new List<MatchMaxBet>
//                        {
//                              new MatchMaxBet
//                              {
//                                  OSMatchId= 123,
//                                  BetTypeName=  "FT HDP",
//                                  MaxBet= 3200000
//                              },
//                              new MatchMaxBet
//                              {
//                                  OSMatchId= 123,
//                                  BetTypeName=  "1h HDP",
//                                  MaxBet= 3200000
//                              },
//                              new MatchMaxBet
//                              {
//                                  OSMatchId= 123,
//                                  BetTypeName=  "1Q & 3Q HDP",
//                                  MaxBet= 3200000
//                              },
//                              new MatchMaxBet
//                              {
//                                  OSMatchId= 123,
//                                  BetTypeName=  "FT OU/OE",
//                                  MaxBet= 3200000
//                              },
//                               new MatchMaxBet
//                              {
//                                  OSMatchId= 123,
//                                  BetTypeName=  "1H OUOE",
//                                  MaxBet= 3200000
//                              }
//                      },
//                       IPTV= "ipTv aaaaa",
//                        WB= "Streaming bbbbbbb",
//                        Provider= "Provider JAck",
//                        AutoLink="Yes",
//                        Category="Category",
//                        NoHDP="NoHDP",
//                        MatchOffer="MatchOffer",
//                        OtherProvider="OtherProvider",
//                        Service="Medium"
//                 }
//            };
//        }
//    }
//}
