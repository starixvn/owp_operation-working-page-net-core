namespace OWP.OtherSport.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Models;

    public class ValidationHandler
    {
        public class Request : IRequest<Result>
        {
            public Request()
            {
                SessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            }

            public string SessionId { get; private set; }

            public Stream Stream { get; set; }
        }

        public class Result
        {
            public Result()
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

        public async Task<Result> Handle(Request request, CancellationToken cancellationToken)
        {
            var result = new Result
            {
                SessionId = request.SessionId
            };
            result.RecordCount = await ProcessAssignmentUploading(result.SessionId, request);

            return result;
        }

        private async Task<int> ProcessAssignmentUploading(string sessionId, Request request)
        {
            //excelValidator.Validate(request.Stream);

            //return analyzedResult.OriginalRecordCount;

            return 1;
        }
    }
}