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

        /**
         * Save a property to favorites
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpPost]
        public async Task<IActionResult> SaveToFavorite(FavoriteRequest favoriteRequest)
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
                favorite = new Favorite
                {
                    Property = property,
                    User = user
                };
                await _favoritesService.Create(favorite);
            }

            return Ok(favorite);
        }

        /**
         * Get a favorite object by id
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpGet("{favoriteId}")]
        public async Task<IActionResult> Get(int favoriteId)
        {
            return Ok(await _favoritesService.Get(favoriteId));
        }

        /**
         * Get all the favorites for this user
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value);
            return Ok(await _favoritesService.GetAll(user));
        }
    }
}