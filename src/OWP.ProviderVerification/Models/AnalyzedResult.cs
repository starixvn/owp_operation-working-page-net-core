namespace OWP.ProviderVerification.Models;

public record AnalyzedResult(string SessionId, int OriginalRecordCount);

public record AnalyzedProviderResult(string SessionId, IList<ProviderVerificationModel> ProviderVerification);

public class Analyzer:  List<ExcelProviderModel>
{
    public Result Validate()
    {
        var errors = new List<Error>();
        for (var index = 0; index < this.Count; index++)
        {
            if (string.IsNullOrWhiteSpace(this[index].Provider))
            {
                errors.Add(Error.Validation(ErrorCode.PROVIDER_IS_REQUIRED, "Provider is required", this[index].Row));
            }
            if (string.IsNullOrWhiteSpace(this[index].ProviderMatchID))
            {
                errors.Add(Error.Validation(ErrorCode.PROVIDER_MATCH_ID_IS_REQUIRED, "ProviderMatchID is required", this[index].Row));
            }
        }
        return errors.Any()
            ? Result.Failure(errors)
                : Result.Success();
    }
}