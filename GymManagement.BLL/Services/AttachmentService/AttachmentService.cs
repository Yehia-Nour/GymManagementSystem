using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] _AllowedExtenstion = { ".jpg", ".jpeg", ".png" };
        private readonly long _maxFileSize = 5 * 1024 * 1024;
        private readonly IWebHostEnvironment _webHost;
        public AttachmentService(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file is null || file.Length == 0)
                    return null;

                if (file.Length > _maxFileSize)
                    return null;

                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!_AllowedExtenstion.Contains(extension))
                    return null;

                var folderPath = Path.Combine(_webHost.WebRootPath, "images", folderName);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = Guid.NewGuid().ToString() + extension;

                var filePath = Path.Combine(folderPath, fileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                file.CopyTo(fileStream);

                return fileName;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild to Upload File to Folder = {folderName} : {ex}");
                return null;
            }
        }

        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return false;

                var fullPath = Path.Combine(_webHost.WebRootPath, "images", folderName, fileName);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild to Delete File with Name = {fileName} : {ex}");
                return false;
            }
        }
    }
}
