using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OWP.FileUpload.Models;
using static OWP.FileUpload.Handlers.DownloadHandler;
using static OWP.FileUpload.Handlers.UploadHandler;

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
        var request = new UploadCommand { File = file };
        var response = await mediator.Send(request);

        return response.FileId;
    }

    [Route("multiple-upload")]
    [HttpPost]
    public async Task<IList<FileModel>> Upload(IList<IFormFile> files)
    {
        var result = new List<FileModel>();

        foreach (var file in files)
        {
            var request = new UploadCommand { File = file };
            var response = await mediator.Send(request);

            result.Add(response);
        }

        return result;
    }

    [Route("download")]
    [HttpGet]
    public async Task<IActionResult> Download(int fileId)
    {
        var request = new DownloadRequest { FileId = fileId };
        var file = await mediator.Send(request);

        return PhysicalFile(file.FilePath, file.MediaType, file.Title);
    }
}