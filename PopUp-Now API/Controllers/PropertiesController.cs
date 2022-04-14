using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Exceptions;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Route("[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesService _propertiesService;

        public PropertiesController(IPropertiesService propertiesService)
        {
            _propertiesService = propertiesService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var properties = await _propertiesService.GetAll();

            return properties.Count > 0 ? Ok(properties) : NotFound();
        }

        [HttpGet("/id/{propertyId}")]
        public async Task<IActionResult> Get(int propertyId)
        {
            try
            {
                var property = await _propertiesService.Get(propertyId);
                return property != null ? Ok(property) : NotFound(propertyId);
            }
            catch (PropertiesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("Category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            try
            {
                var properties = await _propertiesService.GetByCategory(categoryId);
                return properties.Any() ? Ok(properties) : NotFound(categoryId);
            }
            catch (PropertiesException e)
            {
                return BadRequest(e.Message);
            }
        }


        [Authorize]
        [HttpGet("{query}")]
        public async Task<IActionResult> GetSearch(string query)
        {
            try
            {
                var properties = await _propertiesService.GetAll(query);
                return properties.Any() ? Ok(properties) : NotFound(query);
            }
            catch (PropertiesException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Landlord")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int propertyId)
        {
            try
            {
                var removedProperty = await _propertiesService.Delete(propertyId);
                return Ok(removedProperty);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "Landlord")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PropertyRequest propertyRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value!;
                var property = await _propertiesService.Add(propertyRequest, email);
                return Created(property.GetURl(), property);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "Landlord")]
        [HttpGet("{propertyId:int}")]
        public async Task<IActionResult> Update(int propertyId, [FromBody] PropertyRequest propertyRequest)
        {
            var result = await _propertiesService.Update(propertyRequest);

            return result ? Ok("Property updated") : BadRequest();
        }
    }
}