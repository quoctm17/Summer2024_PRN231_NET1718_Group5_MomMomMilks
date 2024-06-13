using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace Service.Interfaces
{
    public interface IImageService
    {
        Task<DeletionResult> DeletePhotoAsync(string publicId);
        string GetImageUploadUrl(string publicId);
        Task<ImageUploadResult> UploadAsync(IFormFile file);
    }
}