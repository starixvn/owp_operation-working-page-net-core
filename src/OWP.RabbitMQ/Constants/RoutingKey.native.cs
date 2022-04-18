using System.Collections.Immutable;
using Newtonsoft.Json;
using OWP.Models;
using OWP.NotificationRequests;

namespace OWP.RabbitMQ.Constants;

public static class RoutingKey
{
  public static readonly IImmutableList<string> Keys = new List<string>{
    "linked-lga-match",
    "prematch-new-league",
    "infosite-match-info"
  }.ToImmutableList();

  public static readonly IImmutableDictionary<string, Func<string, NotificationCommandBaseModel>> KeyMappingDict = new Dictionary<string, Func<string, NotificationCommandBaseModel>>{
            {"linked-lga-match", JsonConvert.DeserializeObject<LinkedMatchNotifyRequest>},
            {"prematch-new-league", JsonConvert.DeserializeObject<PrematchNewLeagueNotifyRequest>},
            {"infosite-match-info", JsonConvert.DeserializeObject<MatchInfoNotifyRequest>}
        }.ToImmutableDictionary();
}