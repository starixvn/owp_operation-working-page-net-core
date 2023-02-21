using OWP.Enums;

namespace OWP.LiveDetail.Common;

public class ValidateResult

{
    public IEnumerable<ValidateEntity> Results { get; set; }

    public IEnumerable<ValidateEntity> FailedResults => Results.Where(x => x.ResultType == (int)RuleReturnType.Failed);

    public IEnumerable<ValidateEntity> WarningResults => Results.Where(x => x.ResultType == (int)RuleReturnType.WarningMinor
                                                                            || x.ResultType == (int)RuleReturnType.WarningMajor);

    public bool IsFailed => FailedResults.Any();

    public bool IsHasWarning => WarningResults.Any();

    public string FailedMessage => string.Join("\n", FailedResults.Select(x => x.Message));
}