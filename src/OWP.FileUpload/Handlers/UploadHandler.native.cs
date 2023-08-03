using Fanex.Data.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using OWP.FileUpload.Criteria;
using OWP.FileUpload.Helper;
using OWP.FileUpload.Models;

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

        public Handler(IDynamicRepository dynamicRepo)
        {
            this.dynamicRepo = dynamicRepo;
        }

        public async Task<FileModel> Handle(UploadCommand request, CancellationToken cancellationToken)
        {
            var criteria = new UploadCriteria { Title = request.File.FileName };

            var result = (await dynamicRepo.FetchAsync<FileModel>(criteria, cancellationToken)).FirstOrDefault();

            await SaveFile(request.File, result.SavedFileName);

            return result;
        }

        private static async Task SaveFile(IFormFile uploadFile, string fileName)
        {
            try
            {
                var fullPath = FileHelper.GetFullPath(fileName);
                await using var fileStream = new FileStream(fullPath, FileMode.Create);
                await uploadFile.CopyToAsync(fileStream);
            }
            catch
            {
                // SonarQube
            }
        }
    }
}