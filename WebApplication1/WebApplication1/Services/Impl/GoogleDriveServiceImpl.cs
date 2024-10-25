using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Identity.Client;
using WebApplication1.Core.Utils;
using WebApplication1.DTOs.Request;
using WebApplication1.DTOs.Response;
using File = Google.Apis.Drive.v3.Data.File;


namespace WebApplication1.Services.Impl
{
    public class GoogleDriveServiceImpl : IGoogleDriveService
    {
        private readonly DriveService _driveService;
        private readonly string _applicationName;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _serviceAccount;

        public GoogleDriveServiceImpl(IConfiguration configuration)
        {
            _applicationName = configuration["GoogleDrive:ApplicationName"];
            _clientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
            _clientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
            _serviceAccount = configuration["Dialogflow:CredentialsFilePath"];
            _driveService = AuthenticateGoogleDrive();
        }

        // Xác thực Google Drive API
        private DriveService AuthenticateGoogleDrive()
        {

            if (string.IsNullOrEmpty(_clientId) || string.IsNullOrEmpty(_clientSecret))
            {
                throw new Exception("Google Client ID or Client Secret is not set in environment variables.");
            }

            var secrets = new ClientSecrets()
            {
                ClientId = _clientId,
                ClientSecret = _clientSecret
            };

            

            GoogleCredential credential;
            using (var stream = new FileStream(_serviceAccount, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    //.CreateScoped(DriveService.Scope.DriveReadonly);
                    .CreateScoped(DriveService.Scope.Drive);
            }

            var service = new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = _applicationName
            });

            return service;
        }


        public async Task<IEnumerable<GoogleDriveFileResponse>> SearchFilesAsync(GoogleDriveSearchOptionsRequest options)
        {
            var files = new List<GoogleDriveFileResponse>();


            var queryBuilder = new List<string>();

            if (!string.IsNullOrEmpty(options.NameContains))
            {
                queryBuilder.Add($"name contains '{options.NameContains}'");
            }
            if (!string.IsNullOrEmpty(options.ParentId))
            {
                queryBuilder.Add($"'{options.ParentId}' in parents");
            }
            if (!string.IsNullOrEmpty(options.MimeType))
            {
                queryBuilder.Add($"mimeType='{options.MimeType}'");
            }
            if (options.IsTrashed.HasValue)
            {
                queryBuilder.Add($"trashed = {options.IsTrashed.Value.ToString().ToLower()}");
            }
            if (options.ModifiedAfter.HasValue)
            {
                queryBuilder.Add($"modifiedTime > '{options.ModifiedAfter.Value:yyyy-MM-ddTHH:mm:ssZ}'");
            }
            if (options.CreatedBefore.HasValue)
            {
                queryBuilder.Add($"createdTime < '{options.CreatedBefore.Value:yyyy-MM-ddTHH:mm:ssZ}'");
            }
            if (!string.IsNullOrEmpty(options.ContentContains))
            {
                queryBuilder.Add($"fullText contains '{options.ContentContains}'"); // Thêm điều kiện tìm nội dung
            }

            // Kết hợp các điều kiện thành chuỗi truy vấn
            var query = string.Join(" and ", queryBuilder);

            var request = _driveService.Files.List();
            request.Q = query; // Query được truyền vào từ người dùng
            request.Fields = "nextPageToken, files(id, name, mimeType, size, createdTime)";
            request.PageSize = 100;

            do
            {
                var result = await request.ExecuteAsync();
                if (result.Files != null && result.Files.Any())
                {
                    files.AddRange(result.Files.Select(MapToGoogleDriveFile));
                }
                request.PageToken = result.NextPageToken;
            } while (!string.IsNullOrEmpty(request.PageToken));

            return files;
        }


        // Liệt kê các file trong Google Drive
        public async Task<IEnumerable<GoogleDriveFileResponse>> GetFilesAsync()
        {
            try
            {
                var files = new List<GoogleDriveFileResponse>();

                var request = _driveService.Files.List();

                string query = "trashed = false";

                request.Q = query;
                request.Fields = "nextPageToken, files(id, name, mimeType, size, createdTime, modifiedTime, parents, webViewLink, thumbnailLink, shared, version)";
                request.PageSize = 1000;
                request.OrderBy = "modifiedTime desc, name";

                do
                {
                    var result = await request.ExecuteAsync();
                    Console.WriteLine($"Fetched {result.Files?.Count} files.");
                    if (result.Files != null && result.Files.Any())
                    {
                        files.AddRange(result.Files.Select(MapToGoogleDriveFile));
                    }
                    request.PageToken = result.NextPageToken;
                } while (!string.IsNullOrEmpty(request.PageToken));

                return files;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error listing files: {ex.Message}", ex);
            }
        }

        // Tải file lên Google Drive
        public async Task<string> UploadFileAsync(string filePath, string mimeType, string folderId, string originalFileName)
        {

            var uniqueFileName = await GetUniqueFileNameAsync(originalFileName, folderId);

            var fileMetadata = new File()
            {
                Name = uniqueFileName,
                Parents = new List<string> { folderId } // Thêm folderId vào đây
            };

            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                var request = _driveService.Files.Create(fileMetadata, stream, mimeType);
                request.Fields = "id";
                var res = await request.UploadAsync();
                var file = request.ResponseBody;

                if (res.Status == Google.Apis.Upload.UploadStatus.Failed)
                {
                    throw new Exception($"File upload failed: {res.Exception.Message}");
                }

                return file.Id;
            }
        }

        // Tải file từ Google Drive về máy
        // Tải file từ Google Drive
        public async Task<(Stream stream, string fileName, string mimeType)> DownloadFileAsync(string fileId)
        {
            try
            {
                // Lấy thông tin tệp
                var fileRequest = _driveService.Files.Get(fileId);
                var file = await fileRequest.ExecuteAsync();

                Stream fileStream;
                string fileName = file.Name;
                string mimeType = file.MimeType;

                // Xử lý theo kiểu MIME
                switch (mimeType)
                {
                    // Google Docs types
                    case GoogleDriveMimeType.Document:
                        fileStream = await ExportFileAsync(fileId, GoogleDriveMimeType.Word);
                        fileName += ".docx";
                        mimeType = GoogleDriveMimeType.Word;
                        break;

                    case GoogleDriveMimeType.Spreadsheet:
                        fileStream = await ExportFileAsync(fileId, GoogleDriveMimeType.Excel);
                        fileName += ".xlsx";
                        mimeType = GoogleDriveMimeType.Excel;
                        break;

                    case GoogleDriveMimeType.Presentation:
                        fileStream = await ExportFileAsync(fileId, GoogleDriveMimeType.Pdf);
                        fileName += ".pdf";
                        mimeType = GoogleDriveMimeType.Pdf;
                        break;

                    case GoogleDriveMimeType.Drawing:
                        fileStream = await ExportFileAsync(fileId, GoogleDriveMimeType.Png);
                        fileName += ".png";
                        mimeType = GoogleDriveMimeType.Png;
                        break;

                    case GoogleDriveMimeType.Form:
                        fileStream = await ExportFileAsync(fileId, GoogleDriveMimeType.Pdf);
                        fileName += ".pdf";
                        mimeType = GoogleDriveMimeType.Pdf;
                        break;

                    // Common file types
                    case GoogleDriveMimeType.Pdf:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".pdf";
                        break;

                    case GoogleDriveMimeType.PlainText:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".txt";
                        break;

                    case GoogleDriveMimeType.Word:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".docx";
                        break;

                    case GoogleDriveMimeType.Excel:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".xlsx";
                        break;

                    case GoogleDriveMimeType.PowerPoint:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".pptx";
                        break;

                    case GoogleDriveMimeType.Jpeg:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".jpeg";
                        break;

                    case GoogleDriveMimeType.Png:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".png";
                        break;

                    case GoogleDriveMimeType.Gif:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".gif";
                        break;

                    case GoogleDriveMimeType.Mp3:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".mp3";
                        break;

                    case GoogleDriveMimeType.Mp4:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".mp4";
                        break;

                    case GoogleDriveMimeType.Zip:
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += ".zip";
                        break;

                    // Các trường hợp khác
                    case GoogleDriveMimeType.Folder:
                        throw new InvalidOperationException("Cannot download a folder.");

                    default:
                        // Tải tệp thông thường nếu không khớp với bất kỳ kiểu nào ở trên
                        fileStream = await DownloadNormalFile(fileId);
                        fileName += Path.GetExtension(mimeType); // Sử dụng đuôi tệp từ MIME
                        break;
                }

                return (fileStream, fileName, mimeType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error downloading file: {ex.Message}", ex);
            }
        }

        // Phương thức tải tệp thông thường
        private async Task<Stream> DownloadNormalFile(string fileId)
        {
            var getRequest = _driveService.Files.Get(fileId);
            var fileStream = new MemoryStream();
            await getRequest.DownloadAsync(fileStream);
            fileStream.Position = 0; // Đặt lại vị trí cho stream
            return fileStream;
        }

        // Phương thức xuất tệp
        private async Task<Stream> ExportFileAsync(string fileId, string mimeType)
        {
            var exportRequest = _driveService.Files.Export(fileId, mimeType);
            var stream = new MemoryStream();
            await exportRequest.DownloadAsync(stream);
            stream.Position = 0; // Đặt lại vị trí cho stream
            return stream;
        }


        private async Task<string> GetUniqueFileNameAsync(string originalFileName, string folderId)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
            var fileExtension = Path.GetExtension(originalFileName);
            var newFileName = originalFileName;
            int count = 1;

            while (await FileExistsInFolderAsync(newFileName, folderId))
            {
                newFileName = $"{fileNameWithoutExtension} ({count++}){fileExtension}";
            }

            return newFileName;
        }

        private async Task<bool> FileExistsInFolderAsync(string fileName, string folderId)
        {
            var request = _driveService.Files.List();
            request.Q = $"name = '{fileName}' and '{folderId}' in parents";
            request.Fields = "files(id)";
            var result = await request.ExecuteAsync();

            return result.Files.Count > 0;
        }


        private GoogleDriveFileResponse MapToGoogleDriveFile(File file)
        {
            return new GoogleDriveFileResponse
            {
                Id = file.Id,
                Name = file.Name,
                MimeType = file.MimeType,
                Size = file.Size,
                CreatedTime = file.CreatedTime,
                ModifiedTime = file.ModifiedTime,
                Parents = file.Parents?.ToList(),
                WebViewLink = file.WebViewLink,
                ThumbnailLink = file.ThumbnailLink,
                Shared = file.Shared,
                Version = file.Version
            };
        }
    }

}
