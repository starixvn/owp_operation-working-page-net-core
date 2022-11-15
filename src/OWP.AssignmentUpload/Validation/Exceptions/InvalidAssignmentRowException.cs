namespace OWP.AssignmentUpload.Validation.Exceptions
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class InvalidAssignmentRowException : Exception
    {
        public InvalidAssignmentRowException(List<AssignmentValidationRowResult> validationResults) : base("Some rows have invalid data")
        {
            ValidationResults = validationResults;
        }

        public List<AssignmentValidationRowResult> ValidationResults { get; set; }
    }
}
