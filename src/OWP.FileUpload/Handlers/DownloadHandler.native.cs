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

            var fileModel = (await dynamicRepo.FetchAsync<FileModel>(criteria, cancellationToken)).FirstOrDefault();

            fileModel.MediaType = FileHelper.GetMediaType(fileModel.Title);
            fileModel.FilePath = FileHelper.GetFullPath(fileModel.SavedFileName);

            return fileModel;
        }
    }
}