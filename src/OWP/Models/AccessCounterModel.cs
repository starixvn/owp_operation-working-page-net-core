using Microsoft.Extensions.Options;

namespace OWP.Models
{
    public class AccessCounterModel
    {
        public string EventUploadNewUpload { get; set; }

        public string ExportMiscMatch { get; set; }

        public string OsExportExcel { get; set; }

        public string OsUpload { get; set; }
        public IOptions<AccessCounterModel> Value { get; set; }

        public override string ToString()
        => $"EventUploadNewUpload: {EventUploadNewUpload} + ExportMiscMatch:{ExportMiscMatch} + OsExportExcel:{OsExportExcel} + OsUpload:{OsUpload}";
    }
}