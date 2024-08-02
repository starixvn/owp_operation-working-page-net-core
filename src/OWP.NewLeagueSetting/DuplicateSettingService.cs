namespace OWP.NewLeagueSetting;

public interface DuplicateSettingService
{
    Task<bool> GetDuplicatedSettingId(LeagueSettingRequest model);
}

public class DuplicateSettingServiceImp(Repository repository) : DuplicateSettingService
{
    public async Task<bool> GetDuplicatedSettingId(LeagueSettingRequest model)
    {
        var leagueSettings = await repository.QueryLeagueSetting(model.League, false);

        return leagueSettings
            .Where(x => x.LeagueSettingId != model.LeagueSettingId)
            .Any(x =>
                x.Condition.HasIpTv == model.HasIpTv &&
                x.Condition.HasTv == model.HasTv &&
                IsConditionsEqual(x.Condition.Round, model.Round) &&
                IsConditionsEqual(x.Condition.Division, model.Division));
    }

    public static bool IsConditionsEqual(IList<Condition> first, IList<Condition> second)
    {
        return first.Count == second.Count && !first.Except(second, new ConditionComparer()).Any();
    }
}
