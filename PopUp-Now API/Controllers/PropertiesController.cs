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

        /**
         * Get a list of all properties
         */
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _propertiesService.GetAll());
        }

        /**
         * Get a property by id
         */
        [HttpGet("/id/{propertyId}")]
        public async Task<IActionResult> Get(int propertyId)
        {
            return Ok(await _propertiesService.Get(propertyId));
        }

        /**
         * Get all properties by category
         */
        [HttpGet("Category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            return Ok(await _propertiesService.GetByCategory(categoryId));
        }


        /**
         * Performs a search on all the properties with the given query
         */
        [Authorize]
        [HttpGet("{query}")]
        public async Task<IActionResult> GetSearch(string query)
        {
            return Ok(await _propertiesService.GetAll(query));
        }

        /**
         * Delete a property
         */
        [Authorize(Roles = "Landlord")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int propertyId)
        {
            return Ok(await _propertiesService.Delete(propertyId));
        }

        /**
         * Add a property
         * Landlord specific action
         * Takes in a propertyRequest 
         */
        [Authorize(Roles = "Landlord")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PropertyRequest propertyRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var email = User.FindFirst(ClaimTypes.Email)?.Value!;
            var property = await _propertiesService.Add(propertyRequest, email);
            return Created(property.GetURl(), property);
        }

        /**
         * Update a property
         */
        [Authorize(Roles = "Landlord")]
        public async Task<IActionResult> Update([FromBody] PropertyRequest propertyRequest)
        {
            var result = await _propertiesService.Update(propertyRequest);

            return result ? Ok("Property updated") : BadRequest();
        }

        /**
         * Get a list of all properties of a landlord
         */
        [Authorize(Roles = "Landlord")]
        [HttpGet("Landlord")]
        public async Task<IActionResult> GetByLandlord()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value!;
            return Ok(await _propertiesService.GetByLandlord(email));
        }
    }
}