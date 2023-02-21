using System.Globalization;
using Fanex.Data.Repository;

namespace OWP.LiveDetail.MatchOdd;

public class DeleteCriteria : NonQueryCommand
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string SessionId { get; } = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);

    public override string GetSettingKey() => "OWP_LiveDetail_DeleteRecommendOdds";

    public override bool IsValid() => UserId > 0 && Id > 0;
}