using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

using Microsoft.Extensions.Options;

using RunningGroupsWebApp.Helpers;
using RunningGroupsWebApp.Interfaces;

namespace RunningGroupsWebApp.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _Cloudinary;

        public PhotoService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );
                
            _Cloudinary = new Cloudinary(account);
        }

        public async Task<ImageUploadResult> AddPhotoAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };

                uploadResult = await _Cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<DeletionResult> DeletionPhotoAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId);
            var result = await _Cloudinary.DestroyAsync(deleteParams);

            return result;
        }
    }
}
