using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using PopUp_Now_API.Database;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using SendGrid;
using static System.String;

namespace PopUp_Now_API.Services
{
    public class ImageService : IImagesService
    {
        private static readonly List<string> SupportedContentTypes = new()
        {
            new string("image/png"),
            new string("image/jpeg")
        };

        private readonly DataContext _dataContext;

        private readonly IWebHostEnvironment _environment;

        private readonly IUsersService _userService;

        public ImageService(DataContext dataContext, IWebHostEnvironment environment, IUsersService userService)
        {
            _dataContext = dataContext;
            _environment = environment;
            _userService = userService;
        }

        public async Task<Image> Upload(IFormFile formFile, string email)
        {
            var provider = new FileExtensionContentTypeProvider();
            
            if (!provider.TryGetContentType(formFile.FileName, out var contentType))
            {
                contentType = formFile.ContentType;
            }
            
            if (IsNullOrEmpty(formFile.FileName) || !SupportedContentTypes.Contains(contentType))
            {
                throw new Exception("Invalid file format");
            }

            var path = _environment.WebRootPath + Path.DirectorySeparatorChar + "Uploads" + Path.DirectorySeparatorChar;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filePath = path + formFile.FileName;
            await using var fileStream = File.Create(filePath);
            await formFile.CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            var image = new Image
            {
                Path = Path.DirectorySeparatorChar + "Uploads" + Path.DirectorySeparatorChar + formFile.FileName
            };
            await _dataContext.Images.AddAsync(image);
            return image;
        }

        public async Task<Image> Get(int imageId)
        {
            var result = await _dataContext.Images.FindAsync(imageId);
            if (result is null)
            {
                throw new Exception("Image with id not found");
            }

            return result;
        }
    }
}