namespace OWP.AssignmentUpload.Validation.Models
{
    using System.Collections.Generic;

    public class AssignmentValidationResult
    {
        public AssignmentValidationResult()
        {
            ValidationResults = new List<AssignmentValidationRowResult>();
            Exception = string.Empty;
        }

        public string SessionId { get; set; }

        public string Exception { get; set; }

        public int RecordCount { get; set; }

        public List<AssignmentValidationRowResult> ValidationResults { get; set; }

        public bool IsSuccessful => string.IsNullOrEmpty(Exception)
                                    && ValidationResults.Count == 0;
    }
}