using Google.Apis.Drive.v3.Data;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;

namespace WebApplication1.Services
{
    public interface IGoogleDriveService
    {
        Task<IEnumerable<GoogleDriveFileResponse>> GetFilesAsync();
        Task<string> UploadFileAsync(string filePath, string mimeType, string folderId, string originalFileName);
        Task<(Stream stream, string fileName, string mimeType)> DownloadFileAsync(string fileId);

        Task<IEnumerable<GoogleDriveFileResponse>> SearchFilesAsync(GoogleDriveSearchOptionsRequest request);
    }
}
