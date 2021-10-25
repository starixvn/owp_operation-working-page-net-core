namespace OWP.FileUpload.Models
{
    public class File
    {
        public int FileId { get; set; }

        public string Title { get; set; }

        public byte[] FileContent { get; set; }
    }
}
