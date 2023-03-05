using WebApp.SharedKernel.Consts;
using WebApp.SharedKernel.DTOs;
using WebApp.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace WebApp.SharedKernel.Helpers
{
    public class FileUtils : IFileUtils
    {
        private readonly IHostEnvironment _environment;
        public FileUtils(IHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<byte[]?> ToBytesAsync(IFormFile file)
        {
            byte[]? data = null;
            if (file is not null && file.Length > 0)
            {
                var stream = file.OpenReadStream();
                using (var br = new BinaryReader(stream))
                    data = br.ReadBytes((int) stream.Length);
            }
            if (data is not null && data.Length > 0)
                return data;
            else
                return null;
        }
        public async Task<byte[]?> ToBytesAsync(string base64String)
        {
            if(!string.IsNullOrEmpty(base64String))
                return Convert.FromBase64String(base64String);
            return null;
        }
        public async Task<IFormFile?> ToIFormFileAsync(byte[] byteArray, string fileName)
        {
            if (byteArray is not null && byteArray.Length > 0)
            {
                var stream = new MemoryStream(byteArray);
                return new FormFile(stream, 0, byteArray.Length, "file", fileName);
            }
            return null;
        }
        public async Task<string?> ToBase64StringAsync(byte[] byteArray)
        {
            if (byteArray is not null && byteArray.Length > 0)
            {
                return Convert.ToBase64String(byteArray, 0, byteArray.Length);
            }
            return null;
        }
        public async Task<string> ToBase64StringAsync(IFormFile file) => await ToBase64StringAsync(await ToBytesAsync(file));
        public async Task<IFormFile> ToIFormFileAsync(string base64String, string fileName) => await ToIFormFileAsync(await ToBytesAsync(base64String), fileName);

        public async Task<Dictionary<string, object>> UploadImageAsync(FileDTO fileDTO)
        {
            var holder = new Dictionary<string, object>();
            List<bool> lIndicator = new List<bool>();
            try
            {
                if (fileDTO.File is not null && fileDTO.File.Length > 0)
                {
                    string FileName = fileDTO.File.FileName;

                    using (var image = Image.Load(fileDTO.File.OpenReadStream()))
                    {

                        string newSize = ImageResize(image, 1200, 1200);
                        string[] sizeArray = newSize.Split(',');
                        image.Mutate(x => x.Resize(Convert.ToInt32(sizeArray[1]), Convert.ToInt32(sizeArray[0])));

                        List<string> FileExtentions = FileExtention(FileTypes.Image);
                        string extention = Path.GetExtension(FileName).ToLower();
                        if (FileExtentions.Count > 0 && FileExtentions.Contains(extention))
                        {
                            if (!string.IsNullOrEmpty(fileDTO.FilePath))
                            {
                                string dir = @"wwwroot\" + fileDTO.FilePath;
                                if (!Directory.Exists(dir))
                                    Directory.CreateDirectory(dir);

                                string fileName = $"{fileDTO.Id}{extention}";
                                string path = Path.Combine(fileDTO.FilePath, fileName);
                                var filePath = Path.Combine(_environment.ContentRootPath, dir, fileName);
                                await DeleteFilesNameInPathAsync(fileDTO.FilePath, fileDTO.Id!);
                                image.Save(filePath);
                                holder.Add(Res.filePath, path);
                                lIndicator.Add(true);
                            }
                            else
                                lIndicator.Add(false);
                        }
                        else
                            lIndicator.Add(false);
                    }
                }
                else
                    lIndicator.Add(false);
            }
            catch (Exception ex)
            {
                holder.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }
            holder.Add(Res.state, lIndicator.All(x => x));
            return holder;
        }

        //upload file with path i wwwroot in api and in the computer folder.
        public async Task<Dictionary<string, object>> UploadFileAsync(FileDTO fileDTO, string fileType)
        {
            var holder = new Dictionary<string, object>();
            List<bool> lIndicator = new List<bool>();
            try
            {
                if (fileDTO.File is not null && fileDTO.File.Length > 0)
                {
                    List<string> FileExtentions = FileExtention(fileType);
                    string extention = Path.GetExtension(fileDTO.File.FileName).ToLower();
                    if (FileExtentions.Count > 0 && FileExtentions.Contains(extention))
                    {
                        if (!string.IsNullOrEmpty(fileDTO.FilePath))
                        {
                            string dir = @"wwwroot\" + fileDTO.FilePath;
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                            string fileName = $"{fileDTO.Id}{extention}";
                            string path = Path.Combine(fileDTO.FilePath, fileName);
                            var filePath = Path.Combine(_environment.ContentRootPath, dir, fileName);
                            using var fileStream = new FileStream(filePath, FileMode.Create);
                            await DeleteFilesNameInPathAsync(fileDTO.FilePath, fileDTO.Id!);
                            await fileDTO.File.CopyToAsync(fileStream);
                            holder.Add(Res.filePath, path);
                            lIndicator.Add(true);
                        }
                        else
                            lIndicator.Add(false);
                    }
                    else
                        lIndicator.Add(false);
                }
                else
                    lIndicator.Add(false);
            }
            catch (Exception ex)
            {
                holder.Add(Res.message, ex.Message);
                lIndicator.Add(false);
            }
            holder.Add(Res.state, lIndicator.All(x => x));
            return holder;
        }
        public async Task<bool> DeleteFileAsync(string filePath)
        {
            List<bool> lIndicator = new List<bool>();
            try
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    var fileDir = @"wwwroot" + filePath;
                    if (File.Exists(fileDir))
                        File.Delete(fileDir);
                }
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                lIndicator.Add(false);
            }
            return lIndicator.All(x => x);
        }
        public async Task<bool> DeleteFilesNameInPathAsync(string folderPath, string fileName)
        {
            var fileDir = @"wwwroot" + folderPath;
            List<bool> lIndicator = new List<bool>();
            try
            {
                string[] files = Directory.GetFiles(fileDir, $"{fileName}.*");
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                        lIndicator.Add(true);
                    }
                    catch { }
                }
                lIndicator.Add(true);
            }
            catch (Exception ex)
            {
                lIndicator.Add(false);
            }
            return lIndicator.All(x => x);
        }

        private List<string> FileExtention(string fileType)
        {
            switch (fileType)
            {
                case FileTypes.Image:
                    return FileTypes.ImageExtensions.ToList();

                case FileTypes.Document:
                    return FileTypes.DocumentExtensions.ToList();

                case FileTypes.ReportDocument:
                    return FileTypes.ReportDocumentExtensions.ToList();

                default:
                    return new List<string>();
            }
        }
        private string ImageResize(Image img, int MaxWidth, int MaxHeight)
        {
            if (img.Width > MaxWidth || img.Height > MaxHeight)
            {
                double widthRatio = (double)img.Width / (double)MaxWidth;
                double heightRatio = (double)img.Height / (double)MaxHeight;
                double ratio = Math.Max(widthRatio, heightRatio);
                int newWidth = (int)(img.Width / ratio);
                int newHeght = (int)(img.Height / ratio);
                return newHeght.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() + "," + img.Width.ToString();

            }
        }
    }
}
