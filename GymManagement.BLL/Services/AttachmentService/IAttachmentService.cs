using Microsoft.AspNetCore.Http;

namespace GymManagement.BLL.Services.AttachmentService
{
    public interface IAttachmentService
    {
        string? Upload(string folderName, IFormFile file);
        bool Delete(string fileName, string folderName);
    }
}
