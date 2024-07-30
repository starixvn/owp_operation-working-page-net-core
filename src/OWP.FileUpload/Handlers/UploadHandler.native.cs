using Fanex.Data.Repository;
using Fanex.Logging;
using MediatR;
using Microsoft.AspNetCore.Http;
using OWP.FileUpload.Criteria;
using OWP.FileUpload.Models;
using OWP.Helpers;

namespace OWP.FileUpload.Handlers;

public class UploadHandler
{
    public class UploadCommand : IRequest<FileModel>
    {
        public IFormFile File { get; set; }
    }

    public class Handler : IRequestHandler<UploadCommand, FileModel>
    {
        private readonly IDynamicRepository dynamicRepo;
        private readonly ILogger logger;

        public Handler(IDynamicRepository dynamicRepo, ILogger logger)
        {
            this.dynamicRepo = dynamicRepo;
            this.logger = logger;
        }

        public async Task<FileModel> Handle(UploadCommand request, CancellationToken cancellationToken)
        {
            var criteria = new UploadCriteria { Title = request.File.FileName };

            var result = (await dynamicRepo.FetchAsync<FileModel>(criteria)).FirstOrDefault();

            await SaveFile(request.File, result.SavedFileName);

            return result;
        }

        private async Task SaveFile(IFormFile uploadFile, string fileName)
        {
            try
            {
                var fullPath = FileHelper.GetFullPath(fileName);
                await using var fileStream = new FileStream(fullPath, FileMode.Create);
                await uploadFile.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync("LogFile", ex.Message);
            }
        }
    }
}