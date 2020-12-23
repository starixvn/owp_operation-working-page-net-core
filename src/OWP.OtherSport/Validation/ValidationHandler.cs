namespace OWP.OtherSport.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using Fanex.NAP.Client.Helpers.Web;
    using MediatR;
    using Models;
    using OWP.OtherSport.Validation.Services;

    public class ValidationHandler
    {
        public class Request : IRequest<Response>
        {
            public Request()
            {
                SessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
            }

            public string SessionId { get; private set; }

            public Stream Stream { get; set; }
        }

        public class Response
        {
            public Response()
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

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly CacheManipulator cacheManipulator;

            private readonly ExcelValidator excelValidator;

            private readonly Analyzer analyzer;

            private readonly INapSession session;

            public Handler(ExcelValidator excelValidator, Analyzer analyzer, INapSession session, CacheManipulator cacheManipulator)
            {
                this.excelValidator = excelValidator;
                this.analyzer = analyzer;
                this.session = session;
                this.cacheManipulator = cacheManipulator;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var result = new Response
                {
                    SessionId = request.SessionId
                };
                result.RecordCount = await ProcessAssignmentUploading(result.SessionId, request);

                await LogExcelFile(request.SessionId, request.Stream);

                return result;
            }

            private async Task<int> ProcessAssignmentUploading(string sessionId, Request request)
            {
                excelValidator.Validate(request.Stream);
                var analyzedResult = await analyzer.Analyze(request);
                await cacheManipulator.SaveAssignmentsSnapshot(sessionId, analyzedResult.Assignments);

                return analyzedResult.OriginalRecordCount;
            }

            private async Task LogExcelFile(string sessionId, Stream file)
            {
                const string folderPath = @"App_Data\Files";
                var absolutedFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}\\{folderPath}";
                var fileName = $"{sessionId}-{session.UserName}-New.xlsx";
                var fullPath = Path.Combine(absolutedFolderPath, fileName);
                using (var stream = File.Create(fullPath))
                {
                    file.Seek(0, SeekOrigin.Begin);
                    await file.CopyToAsync(stream);
                }
            }
        }
    }
}