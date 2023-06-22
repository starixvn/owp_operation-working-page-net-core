using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OWP.FileUpload.Handlers;
using OWP.FileUpload.Models;

namespace OWP.FileUpload;

[Route("api/[controller]")]
[ApiController]
[Authorize]
[EnableCors("AllowOrigin")]
public class FileUploadController : ControllerBase
{
    private readonly IMediator mediator;

    public FileUploadController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [Route("upload")]
    [HttpPost]
    public async Task<int> Upload(IFormFile file)
    {
        await using var memoryStream = new MemoryStream();

        await file.CopyToAsync(memoryStream);

        var request = new UploadHandler.UploadCommand
        {
            FileStream = memoryStream.ToArray(),
            FileName = file.FileName
        };

        var response = await mediator.Send(request);

        return response.FileId;
    }

    [Route("multiple-upload")]
    [HttpPost]
    public async Task<IList<FileModel>> Upload(IList<IFormFile> files)
    {
        var result = new List<FileModel>();
        await using var memoryStream = new MemoryStream();

        foreach (var file in files)
        {
            await file.CopyToAsync(memoryStream);
            var request = new UploadHandler.UploadCommand
            {
                FileStream = memoryStream.ToArray(),
                FileName = file.FileName
            };
            var response = await mediator.Send(request);

            memoryStream.SetLength(0);
            result.Add(response);
        }

        return result;
    }

    [Route("download")]
    [HttpGet]
    public async Task<IActionResult> Download(int fileId)
    {
        var request = new DownloadHandler.DownloadRequest
        {
            FileId = fileId
        };

        var file = await mediator.Send(request);

        var fileExtension = file.Title[(file.Title.LastIndexOf('.') + 1)..].ToLowerInvariant().Trim();
        var isImage = IsImage(fileExtension);
        var mediaType = GetMediaType(isImage, fileExtension);

        var stream = new MemoryStream(file.FileContent);

        return isImage
            ? File(stream, mediaType)
            : File(stream, mediaType, file.Title);
    }

    private static string GetMediaType(bool isImage, string fileExtension)
    {
        return isImage
            ? $"image/{fileExtension}"
            : "application/octet-stream";
    }

    private static bool IsImage(string fileExtension)
    {
        string[] imageExtension = { "jpg", "jpeg", "png", "gif", "bmp" };
        return imageExtension.Contains(fileExtension);
    }
}