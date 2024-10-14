using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Core.Entities
{
    public class Image
    {
        // ID của hình ảnh
        public Guid ID { get; set; }

        // Tệp tin được tải lên
        [NotMapped]
        public IFormFile File { get; set; }

        // Tên tệp tin
        public string FileName { get; set; }

        // Mô tả tệp tin (có thể null)
        public string? FileDescription { get; set; }

        // Phần mở rộng tệp tin
        public string FileExtension { get; set; }

        // Kích thước tệp tin (tính bằng byte)
        public long FileSizeInBytes { get; set; }

        // Đường dẫn tệp tin
        public string FilePath { get; set; }
    }
}
