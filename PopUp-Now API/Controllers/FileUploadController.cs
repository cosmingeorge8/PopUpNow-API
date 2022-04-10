using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;

namespace PopUpStore.api.Controllers
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

        [Authorize(Roles = "User,Landlord")]
        [HttpGet("{imageId}")]
        public async Task<IActionResult> Get(int imageId)
        {
            try
            {
                return Ok(await _imagesService.Get(imageId));
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "User,Landlord")]
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile formFile)
        {
            try
            {
                var result = await _imagesService.Upload(formFile, User.FindFirst(ClaimTypes.Email)?.Value!);
                return Created(result.Path, result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}