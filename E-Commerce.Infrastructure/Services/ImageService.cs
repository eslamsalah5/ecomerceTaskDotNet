using E_Commerce.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace E_Commerce.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly string _webRootPath;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public ImageService(IConfiguration configuration)
        {
            // Use a configurable path or default to wwwroot
            _webRootPath = configuration["FileUpload:WebRootPath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }

        public async Task<string> SaveImageAsync(IFormFile imageFile, string folder = "products")
        {
            if (!IsValidImageFile(imageFile))
            {
                throw new ArgumentException("Invalid image file");
            }

            // Create uploads directory if it doesn't exist
            var uploadsPath = Path.Combine(_webRootPath, "uploads", folder);
            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";
            var filePath = Path.Combine(uploadsPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return relative path for database storage
            return Path.Combine("uploads", folder, fileName).Replace("\\", "/");
        }

        public async Task<bool> DeleteImageAsync(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath))
                    return true;

                var fullPath = Path.Combine(_webRootPath, imagePath.Replace("/", "\\"));
                
                if (File.Exists(fullPath))
                {
                    await Task.Run(() => File.Delete(fullPath));
                    return true;
                }
                
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string? GetImageUrl(string? imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            return $"/{imagePath}";
        }

        public bool IsValidImageFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return false;

            if (file.Length > MaxFileSize)
                return false;

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _allowedExtensions.Contains(extension);
        }
    }
}