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
        /* Injected dependencies */
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
            return Ok(await _categoryService.Get());
        }
    }
}