using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCore_01.Database;
using NetCore_01.Models;

namespace NetCore_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticoliController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            using (BlogContext db = new BlogContext())
            {
                List<Post> articoli = db.Posts.Include(articolo => articolo.Tags).ToList<Post>();

                return Ok(articoli);
            }
        }

        
    }
}
