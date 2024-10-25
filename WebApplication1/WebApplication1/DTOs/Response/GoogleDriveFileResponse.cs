namespace WebApplication1.DTOs.Response
{
    public class GoogleDriveFileResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MimeType { get; set; }
        public long? Size { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public string WebViewLink { get; set; }
        public List<string> Parents { get; set; }
        public string ThumbnailLink { get; set; }
        public bool? Shared { get; set; }
        public long? Version { get; set; }
    }
}
