using Fanex.Data.Repository;
using Newtonsoft.Json;

namespace OWP.SimpleDataApi.Services;

public interface SimpleCriteriaFactory
{
    Type GetCriteriaType(string apiKey);

    Type GetCommandType(string apiKey);

    ICriteria CreateCriteria(Type criteriaType, Dictionary<string, object> queryModel);

    INonQueryCommand CreateNonQueryCommand(Type commandType, Dictionary<string, object> queryModel);
}

public abstract class SimpleCriteriaFactoryBase : SimpleCriteriaFactory
{
    protected static readonly Dictionary<string, Type> CriteriaTypes = new();

    protected static readonly Dictionary<string, Type> CommandTypes = new();

    public virtual ICriteria CreateCriteria(Type criteriaType, Dictionary<string, object> queryModel)
    {
        var json = JsonConvert.SerializeObject(queryModel);
        var criteria = JsonConvert.DeserializeObject(json, criteriaType);

        return (ICriteria)criteria;
    }

    public virtual INonQueryCommand CreateNonQueryCommand(Type commandType, Dictionary<string, object> queryModel)
    {
        var json = JsonConvert.SerializeObject(queryModel);
        var nonQueryCommand = JsonConvert.DeserializeObject(json, commandType);

        return (INonQueryCommand)nonQueryCommand;
    }

    public virtual Type GetCriteriaType(string apiKey) => CriteriaTypes.ContainsKey(apiKey) ? CriteriaTypes[apiKey] : null;

    public virtual Type GetCommandType(string apiKey) => CommandTypes.ContainsKey(apiKey) ? CommandTypes[apiKey] : null;
}