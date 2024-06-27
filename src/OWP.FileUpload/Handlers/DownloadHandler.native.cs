using Fanex.Data.Repository;
using MediatR;
using OWP.FileUpload.Criteria;
using OWP.FileUpload.Models;
using OWP.Helpers;

namespace OWP.FileUpload.Handlers;

public class DownloadHandler
{
    public class DownloadRequest : IRequest<FileModel>
    {
        public int FileId { get; set; }
    }

    public class Handler : IRequestHandler<DownloadRequest, FileModel>
    {
        private readonly IDynamicRepository dynamicRepo;

        public Handler(IDynamicRepository dynamicRepo)
        {
            this.dynamicRepo = dynamicRepo;
        }

        public async Task<FileModel> Handle(DownloadRequest request, CancellationToken cancellationToken)
        {
            var criteria = new DownloadCriteria { FileId = request.FileId };

            var fileModel = (await dynamicRepo.FetchAsync<FileModel>(criteria)).FirstOrDefault();

            var fileExtension = fileModel.Title[(fileModel.Title.LastIndexOf('.') + 1)..].ToLowerInvariant().Trim();
            var isImage = IsImage(fileExtension);
            fileModel.MediaType = GetMediaType(fileExtension, isImage);
            fileModel.FilePath = FileHelper.GetFullPath(fileModel.SavedFileName);
            fileModel.Title = isImage ? null : fileModel.Title;

            return fileModel;
        }

        private static string GetMediaType(string fileExtension, bool isImage)
            => isImage
                ? $"image/{fileExtension}"
                : "application/octet-stream";

        private static bool IsImage(string fileExtension)
        {
            string[] imageExtension = ["jpg", "jpeg", "png", "gif", "bmp"];
            return imageExtension.Contains(fileExtension);
        }
    }
}