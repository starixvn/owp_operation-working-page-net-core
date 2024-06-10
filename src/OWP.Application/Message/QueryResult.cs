
namespace OWP.Application.Message;
public class QueryResult
{
    public IEnumerable<MessageModel> Messages { get; set; }

    public int TotalNotViewMessage { get; set; }
}