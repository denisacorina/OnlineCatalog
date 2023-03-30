using System.ComponentModel.DataAnnotations;

namespace CatalogApp.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int Semester { get; set; }
        public ICollection<Student>? Students { get; set; }
        public ICollection<Professor>? Professors { get; set; }
        public ICollection<Grade>? Grades { get; set; }
    }
}
