using Microsoft.AspNetCore.Http;

namespace E_Commerce.Application.Interfaces
{
    public interface IImageService
    {
        Task<string> SaveImageAsync(IFormFile imageFile, string folder = "products");
        Task<bool> DeleteImageAsync(string imagePath);
        string? GetImageUrl(string? imagePath);
        bool IsValidImageFile(IFormFile file);
    }
}