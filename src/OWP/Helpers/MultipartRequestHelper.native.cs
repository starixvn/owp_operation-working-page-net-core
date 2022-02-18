namespace OWP.Helpers
{
    public static class MultipartRequestHelper
    {
        public static bool IsMultipartContentType(string contentType)
        {
            return !string.IsNullOrEmpty(contentType)
                   && contentType.Contains("multipart/", StringComparison.OrdinalIgnoreCase);
        }
    }
}