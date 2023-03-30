using CatalogApp.Context;
using CatalogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {

        private readonly AppDbContext _context;
        public CoursesController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("api/[controller]/GetCourses")]
        public async Task<List<Course>> GetCourses()
        {
            return await _context.Courses.ToListAsync();
        }

        [HttpPost]
        [Route("api/[controller]/CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return Ok(course);
        }
    }
}
