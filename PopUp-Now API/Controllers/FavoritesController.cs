using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

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
        private readonly IPropertiesService _propertiesService;

        public FavoritesController(IFavoritesService favoritesService, IUsersService userService,
            IPropertiesService propertiesService)
        {
            _favoritesService = favoritesService;
            _userService = userService;
            _propertiesService = propertiesService;
        }

        [Authorize(Roles = "User,Landlord")]
        [HttpPost]
        public async Task<IActionResult> SaveToFavorite(FavoriteRequest favoriteRequest)
        {
            try
            {
                var user = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value);
                var property = await _propertiesService.Get(favoriteRequest.PropertyId);
                Favorite favorite;
                if (favoriteRequest.IsFavorite)
                {
                    favorite = await _favoritesService.Delete(user, property);
                }
                else
                {
                    favorite = new Favorite()
                    {
                        Property = property,
                        User = user
                    };
                    await _favoritesService.Create(favorite);
                }

                return Ok(favorite);
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
            var user = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value);
            var favorites = await _favoritesService.GetAll(user);

            return favorites is null ? NotFound() : Ok(favorites);
        }
    }
}