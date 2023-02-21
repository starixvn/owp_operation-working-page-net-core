using System.Text;
using Fanex.Logging;
using Newtonsoft.Json;

namespace OWP.Services;

public interface LogServiceWrapper
{
    Task Log(string header, params object[] parameters);
}

public class LogServiceWrapperImp : LogServiceWrapper
{
    public async Task Log(string header, params object[] parameters)
    {
        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine(header);

        var index = 0;
        foreach (var parameter in parameters)
        {
            stringBuilder.AppendFormat("Parameter {0}-{1}: {2}\r\n", index, parameter != null ? parameter.GetType() : string.Empty, JsonConvert.SerializeObject(parameter));
            index++;
        }

        await Logger.Log.InfoAsync(stringBuilder.ToString()).ConfigureAwait(false);
    }
}