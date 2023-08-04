using Fanex.Data.Repository;
using Fanex.Logging;
using MediatR;
using OWP.Helpers;

namespace OWP.FileUpload.Handlers;

public class InitialOldDataHandler
{
    public class InitialOldDataRequest : IRequest<int> { }

    public class Handler : IRequestHandler<InitialOldDataRequest, int>
    {
        private readonly IDynamicRepository dynamicRepo;
        private readonly ILogger logger;

        public Handler(IDynamicRepository dynamicRepo, ILogger logger)
        {
            this.dynamicRepo = dynamicRepo;
            this.logger = logger;
        }

        public async Task<int> Handle(InitialOldDataRequest request, CancellationToken cancellationToken)
        {
            var criteria = new InitialOldDataCriteria();

            var result = (await dynamicRepo.FetchAsync<OldFileModel>(criteria, cancellationToken)).ToList();
            foreach (var file in result)
            {
                await SaveFile(file.FileContent, file.SavedFileName);
            }

            return result.Count;
        }

        private async Task SaveFile(byte[] fileContent, string fileName)
        {
            try
            {
                var fullPath = FileHelper.GetFullPath(fileName);
                await using var fileStream = new FileStream(fullPath, FileMode.Create);

                var stream = new MemoryStream(fileContent);
                await stream.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync("LogFile", ex.Message);
            }
        }
    }

    public class OldFileModel
    {
        public string SavedFileName { get; set; }

        public byte[] FileContent { get; set; }
    }

    public class InitialOldDataCriteria : CriteriaBase
    {
        public override string GetSettingKey() => "OWP_FileUpload_InitialOldData";

        public override bool IsValid() => true;
    }
}