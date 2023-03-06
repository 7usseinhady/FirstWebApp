using WebApp.SharedKernel.Dtos;
using Microsoft.AspNetCore.Http;

namespace WebApp.SharedKernel.Interfaces
{
    public interface IFileUtils
    {
        Task<byte[]> ToBytesAsync(IFormFile file);
        Task<byte[]> ToBytesAsync(string base64String);
        Task<IFormFile> ToIFormFileAsync(byte[] byteArray, string fileName);
        Task<string> ToBase64StringAsync(byte[] byteArray);
        Task<string> ToBase64StringAsync(IFormFile file);
        Task<IFormFile> ToIFormFileAsync(string base64String, string fileName);

        Task<Dictionary<string, object>> UploadImageAsync(FileDto fileDto);
        Task<Dictionary<string, object>> UploadFileAsync(FileDto fileDto, string fileType);
        Task<bool> DeleteFileAsync(string filePath);
        Task<bool> DeleteFilesNameInPathAsync(string folderPath, string fileName);
    }
}
