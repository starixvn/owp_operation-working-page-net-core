namespace OWP.AssignmentUpload.Validation.Models;

using System.Collections.Generic;

public class AnalyzedResult
{
    public List<AssignmentModel> Assignments { get; set; }

    public int OriginalRecordCount { get; set; }
}