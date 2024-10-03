using MediatR;
using Microsoft.AspNetCore.Hosting;
using OWP.AssignmentUpload.Validation.Exceptions;
using OWP.AssignmentUpload.Validation.Models;
using OWP.AssignmentUpload.Validation.Services;
using System.Globalization;
using Fanex.NAP.Client;

namespace OWP.AssignmentUpload.Validation;

public class ValidationRequest : IRequest<AssignmentValidationResult>
{
    public ValidationRequest()
    {
        SessionId = Guid.NewGuid().ToString("D", CultureInfo.InvariantCulture);
    }

    public string SessionId { get; }

    public DateTime EventDate { get; set; }

    public Stream Stream { get; set; }

    public bool IsUploadFMAndAIO { get; set; }
}

public class Handler : IRequestHandler<ValidationRequest, AssignmentValidationResult>
{
    private readonly Analyzer analyzer;

    private readonly CacheManipulator cacheManipulator;

    private readonly ExcelValidator excelValidator;

    private readonly INapSession currentSession;

    private readonly IWebHostEnvironment hostEnvironment;

    public Handler(
        Analyzer analyzer,
        ExcelValidator excelValidator,
        CacheManipulator cacheManipulator,
        INapSession currentSession,
        IWebHostEnvironment hostEnvironment)
    {
        this.analyzer = analyzer;
        this.cacheManipulator = cacheManipulator;
        this.excelValidator = excelValidator;
        this.currentSession = currentSession;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<AssignmentValidationResult> Handle(ValidationRequest request, CancellationToken cancellationToken)
    {
        var result = new AssignmentValidationResult
        {
            SessionId = request.SessionId
        };
        try
        {
            result.RecordCount = await ProcessAssignmentUploading(result.SessionId, request);
        }
        catch (CriticalValidationException e)
        {
            result.Exception = e.Message;
        }
        catch (InvalidAssignmentRowException e)
        {
            result.ValidationResults = e.ValidationResults;
        }

        await LogExcelFile(request.SessionId, request.Stream, request.EventDate);

        return result;
    }

    private async Task<int> ProcessAssignmentUploading(string sessionId, ValidationRequest request)
    {
        excelValidator.Validate(request.Stream);
        var analyzedResult = await analyzer.Analyze(request);
        await cacheManipulator.SaveAssignments(sessionId, analyzedResult.Assignments);

        return analyzedResult.OriginalRecordCount;
    }

    private async Task LogExcelFile(string sessionId, Stream file, DateTime eventDate)
    {
        try
        {
            var absoluteFolderPath = Path.Combine(
                hostEnvironment.ContentRootPath,
                "App_Data",
                "Files"
                );

            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);
            }

            var fileName = $"{sessionId}-{currentSession.UserName}-{eventDate:yyy-MM-dd}.xlsx";
            var fullPath = Path.Combine(absoluteFolderPath, fileName);
            await using var stream = File.Create(fullPath);
            file.Seek(0, SeekOrigin.Begin);
            await file.CopyToAsync(stream);
        }
        catch
        {
            // SonarQube
        }
    }
}