using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Exceptions;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class FileUploadController : ControllerBase
    {
        private readonly IImagesService _imagesService;

        public FileUploadController(IImagesService imagesService)
        {
            _imagesService = imagesService;
        }

        /**
         * Get the image object by the imageId
         * Usefull for getting the imagePath
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpGet("{imageId}")]
        public async Task<IActionResult> Get(int imageId)
        {
            return Ok(await _imagesService.Get(imageId));
        }

        /**
         * Upload images
         * Files are stored on the server as static files
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpPost]
        public async Task<IActionResult> Upload(FormFileCollection formFiles)
        {
            var images = new List<Image>();
            foreach (var formFile in formFiles)
            {
                var result = await _imagesService.Upload(formFile, User.FindFirst(ClaimTypes.Email)?.Value!);
                images.Add(result);
            }

            if (images.Count == 0)
            {
                throw new PopUpNowException("No file uploaded");
            }

            return Ok(images);
        }
    }
}