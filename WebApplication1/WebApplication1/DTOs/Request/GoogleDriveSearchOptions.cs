namespace WebApplication1.DTOs.Request
{
    public class GoogleDriveSearchOptionsRequest
    {
        public string? NameContains { get; set; }
        public string ParentId { get; set; }
        public string MimeType { get; set; }
        public bool? IsTrashed { get; set; }
        public DateTime? ModifiedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }

        public string ContentContains { get; set; }  // Thêm trường tìm nội dung
    }
}
