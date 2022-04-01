namespace OWP.MatchCountLiveReport.Models
{
    public record FileInfo
    {
        public byte[] FileContents { get; set; }

        public string ContentType { get; set; }

        public string FileDownloadName { get; set; }
    }
}