namespace OWP.FileUpload.Models
{
    public class FileModel
    {
        public int FileId { get; set; }

        public string Title { get; set; }

        public byte[] FileContent { get; set; }
    }
}