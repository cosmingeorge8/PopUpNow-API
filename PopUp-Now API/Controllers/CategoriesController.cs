using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Route("[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        /**
         * Get a list of all categories
         */
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.Get();
            return categories.Any() ? Ok(categories) : NotFound();
        }
    }
}