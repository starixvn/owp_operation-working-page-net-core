using NSubstitute;
using OWP.TicketCenter.Models;

namespace OWP.TicketCenter.UnitTests.Services;

public class GetEventTicketsServiceUnitTests
{
    public GetEventTicketsServiceUnitTests()
    {
        Substitute.For<Repository>();
    }

    public List<int> MockListTicketIsSaved()
        => new()
        {
            1,2,3
        };
    public List<TicketLeagueInfo> MockListEventTicketLeague()
        => new()
        {
            new TicketLeagueInfo
            {
                TicketId = 1,
                OwpLeagueId=17,
                OwpLeagueName="NHA",
                LastCommentDate=new DateTime(2022,3,16)
            }
        };

    public List<TicketMatchInfo> MockListEventTicketMatch()
        => new()
        {
            new TicketMatchInfo
            {
                TicketId = 2,
                OwpLeagueId=17,
                OwpLeagueName="NHA",
                LastCommentDate=new DateTime(2022,3,17)
            }
        };

    public List<TicketOtherInfo> MockListEventTicketOther()
        => new()
        {
            new TicketOtherInfo
            {
                TicketId = 3,
                Subject="hi",
                LastCommentDate=new DateTime(2022,3,18)
            }
        };

    public List<TicketSupportInfo> MockListEventTicketSupport()
        => new()
        {
            new TicketSupportInfo
            {
                TicketId = 4,
                Subject ="hi",
                LastCommentDate=new DateTime(2022,3,15)
            }
        };

    public List<MessageInfoModel> MockLastMessageInfo()
        => new()
        {
            new MessageInfoModel
            {
                MessageId = 1,
                TicketId=1
            },
            new MessageInfoModel
            {
                MessageId = 2,
                TicketId=2
            },
            new MessageInfoModel
            {
                MessageId = 3,
                TicketId=3
            },
            new MessageInfoModel
            {
                MessageId = 4,
                TicketId=4
            }
        };

    public IList<TicketBaseInfo> GetExpected()
        => new List<TicketBaseInfo>
        {
            new TicketOtherInfo
            {
                TicketId = 3,
                Subject="hi",
                LastCommentDate=new DateTime(2022,3,18),
                Messages=new List<MessageInfoModel>
                {
                    new()
                    {
                        MessageId = 3,
                        TicketId=3
                    }
                }
            },
            new TicketMatchInfo
            {
                TicketId = 2,
                OwpLeagueId=17,
                OwpLeagueName="NHA",
                LastCommentDate=new DateTime(2022,3,17),
                Messages=new List<MessageInfoModel>
                {
                    new()
                    {
                        MessageId = 2,
                        TicketId=2
                    }
                }
            },
            new TicketLeagueInfo
            {
                TicketId = 1,
                OwpLeagueId=17,
                OwpLeagueName="NHA",
                LastCommentDate=new DateTime(2022,3,16),
                Messages=new List<MessageInfoModel>
                {
                    new()
                    {
                        MessageId = 1,
                        TicketId=1
                    }
                }
            }
        };
}