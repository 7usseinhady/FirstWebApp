using WebApp.SharedKernel.Dtos;
using Microsoft.AspNetCore.Http;

namespace WebApp.SharedKernel.Interfaces
{
    public interface IFileUtils
    {
        byte[]? ToBytes(IFormFile file);
        byte[]? ToBytes(string base64String);
        string? ToBase64String(byte[] byteArray);
        string? ToBase64String(IFormFile file);
        IFormFile? ToIFormFile(string base64String, string fileName);
        IFormFile? ToIFormFile(byte[] byteArray, string fileName);

        Dictionary<string, object> UploadImage(FileDto fileDto);
        Task<Dictionary<string, object>> UploadFileAsync(FileDto fileDto, string fileType);
        bool DeleteFile(string? filePath);
        bool DeleteFilesNameInPath(string folderPath, string fileName);
    }
}
