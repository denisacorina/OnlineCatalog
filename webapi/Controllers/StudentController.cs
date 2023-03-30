using CatalogApp.Context;
using CatalogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly AppDbContext _context;
        public StudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllStudents")]
        public async Task<List<Student>> GetAllStudents()
        {
            return await _context.Students
                .Include(c => c.Courses)
                .Include(g => g.Grades)
                .ToListAsync();
        }

        [HttpPost]
        [Route("CreateStudent")]
        public async Task<IActionResult> CreateStudent([FromBody] Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return Ok(student);
        }

        [HttpPost]
        [Route("SubscribeToCourses/{studentId}/{courseId}")]
        public async Task<IActionResult> SubscribeToCoursesAsync(Guid studentId, int courseId)
        {
            var student = await _context.Students.Include(c => c.Courses).FirstOrDefaultAsync(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            if (student.Courses.Contains(course))
            {
                return BadRequest("The student is already subscribed to this course.");
            }

            student.Courses.Add(course);

            await _context.SaveChangesAsync();

            var response = new
            {
                Student = student,
                Courses = student.Courses
            };

            return Ok(response);
        }
    }
}
