namespace OWP.OtherSport.Validation
{
    
    
    using System.Globalization;
    
    
    
    using Fanex.NAP.Client.Helpers.Web;
    using MediatR;
    using Models;
    using OWP.OtherSport.Validation.Exceptions;
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
                try
                {
                    result.RecordCount = await ProcessAssignmentUploading(result.SessionId, request);
                }
                catch (CriticalValidationException ex)
                {
                    var validationResults = new List<AssignmentValidationRowResult>
                    {
                        new AssignmentValidationRowResult
                        {
                            Row = 1,
                            Message = ex.Message
                        }
                    };

                    result.ValidationResults = validationResults;
                    result.Exception = ex.Message;
                }
                catch (InvalidAssignmentRowException ex)
                {
                    result.ValidationResults = ex.ValidationResults;
                    result.Exception = ex.Message;
                }

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
                var folderPath = @$"App_Data{Path.DirectorySeparatorChar}Files";
                var absolutedFolderPath = $"{AppDomain.CurrentDomain.BaseDirectory}{Path.DirectorySeparatorChar}{folderPath}";

                if (!Directory.Exists(absolutedFolderPath))
                {
                    Directory.CreateDirectory(absolutedFolderPath);
                }

                var fileName = $"{sessionId}-{session.UserName}-New-OS.xlsx";
                var fullPath = Path.Combine(absolutedFolderPath, fileName);

                using var stream = File.Create(fullPath);
                file.Seek(0, SeekOrigin.Begin);
                await file.CopyToAsync(stream);
            }
        }
    }
}