using CatalogApp.Context;
using CatalogApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {

        private readonly AppDbContext _context;
        public ProfessorController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetAllProfessors")]
        public async Task<List<Professor>> GetAllProfessors()
        {
            return await _context.Professors
                .Include(c => c.Courses)
                .ThenInclude(s => s.Students)
                .ThenInclude(g => g.Grades)
                .ToListAsync();
        }


        [HttpGet("GetGrades/{professorId}")]
        public async Task<ActionResult<IEnumerable<Professor>>> GetGrades(Guid professorId)
        {
            var grades = await _context.Grades
                .Where(g => g.Professor.ProfessorId == professorId)
                .Include(c => c.Course)
                .ThenInclude(s => s.Students)
                .ThenInclude(g => g.Grades)
                .Select(g => new
                {
                    g.Student,
                    g.Course,
                    g.Value
                })
                .ToListAsync();

            if (grades == null || grades.Count == 0)
            {
                return NotFound();
            }

            return Ok(grades);
        }


        [HttpGet]
        [Route("GetProfessorById/{professorId}")]
        public async Task<Professor> GetProfessorById(Guid professorId)
        {
            return await _context.Professors
                .Include(c => c.Courses)
                .ThenInclude(s => s.Students)
                .ThenInclude(g => g.Grades)
                .FirstOrDefaultAsync(p => p.ProfessorId == professorId);
        }

        [HttpPost]
        [Route("CreateProfessor")]
        public async Task<IActionResult> CreateProfessor([FromBody] Professor professor)
        {
            await _context.Professors.AddAsync(professor);
            await _context.SaveChangesAsync();
            return Ok(professor);
        }

        [HttpGet("{professorId}/students")]
        public IActionResult GetStudentsForCourse(Guid professorId, int courseId)
        {
            var professor = _context.Professors
                .Include(p => p.Courses)
                    .ThenInclude(c => c.Students)
                .FirstOrDefault(p => p.ProfessorId == professorId);

            if (professor == null)
            {
                return NotFound();
            }

            var course = professor.Courses.FirstOrDefault(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            var students = course.Students.Select(s => new
            {
                Student = s,
                Grade = s.Grades?.FirstOrDefault(g => g.Course?.CourseId == courseId && g.Professor?.ProfessorId == professorId)
            });
            return Ok(students);
        }

        [HttpPost]
        [Route("AssignProfessorToCourse/{professorId}/{courseId}")]
        public async Task<IActionResult> AssignProfessorToCourse(Guid professorId, int courseId)
        {
            var professor = await _context.Professors.Include(c => c.Courses).FirstOrDefaultAsync(p => p.ProfessorId == professorId);

            if (professor == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.Include(c => c.Professors).FirstOrDefaultAsync(c => c.CourseId == courseId);

            if (course == null)
            {
                return NotFound();
            }

            if (course.Professors.Any())
            {
                return BadRequest("The course already has a professor assigned to it.");
            }

            if (professor.Courses.Contains(course))
            {
                return BadRequest("The professor is already assigned to this course.");
            }

            professor.Courses.Add(course);

            await _context.SaveChangesAsync();

            return Ok(professor);
        }

        [HttpPost]
        [Route("AddGrade/{courseId}/{studentId}/{professorId}/{value}")]
        public IActionResult AddGrade(int courseId, Guid studentId, double value, Guid professorId)
        {
            var course = _context.Courses.Include(c => c.Professors).Include(c => c.Students).FirstOrDefault(c => c.CourseId == courseId && c.Professors.Any(p => p.ProfessorId == professorId));

            if (course == null)
            {
                return NotFound("The course was not found or the professor is not teaching it.");
            }

            var student = course.Students.FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound("The student was not found in this course.");
            }

            var professor = course.Professors.FirstOrDefault(p => p.ProfessorId == professorId);

            if (professor == null)
            {
                return NotFound("The professor was not found for this course.");
            }

            var grade = _context.Grades.FirstOrDefault(g => g.Student.StudentId == studentId && g.Course.CourseId == courseId && g.Professor.ProfessorId == professorId);

            if (grade != null)
            {
                return BadRequest("Grade has already been assigned.");
            }
            else
            {
                grade = new Grade
                {
                    Value = value,
                    Student = student,
                    Course = course,
                    Professor = professor
                };
                _context.Grades.Add(grade);
                _context.SaveChanges();
                return Ok(grade);
            }
        }
    }
}
