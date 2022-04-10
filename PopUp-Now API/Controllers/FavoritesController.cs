using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Route("[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly IFavoritesService _favoritesService;
        private readonly IUsersService _userService;

        public FavoritesController(IFavoritesService favoritesService, IUsersService userService)
        {
            _favoritesService = favoritesService;
            _userService = userService;
        }

        [Authorize(Roles = "User,Landlord")]
        [HttpPost]
        public async Task<IActionResult> SaveToFavorite(Property property)
        {
            try
            {
                var favorite = new Favorite()
                {
                    User = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value) as User,
                    Property = property
                };
                _favoritesService.Create(favorite);
                return Created(favorite.GetURL(), favorite);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize(Roles = "User,Landlord")]
        [HttpGet("{favoriteId}")]
        public async Task<IActionResult> Get(int favoriteId)
        {
            try
            {
                var favorite = await _favoritesService.Get(favoriteId);
                return Ok(favorite);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        [Authorize(Roles = "User,Landlord")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var favorites = await _favoritesService.GetAll();

            return favorites is null ? NotFound() : Ok(favorites);
        }

        [Authorize(Roles = "User,Landlord")]
        [HttpDelete("{favoriteId}")]
        public IActionResult Delete(int favoriteId)
        {
            try
            {
                _favoritesService.Delete(favoriteId);
                return Ok("Favorite was deleted");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}