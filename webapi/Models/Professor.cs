using System.ComponentModel.DataAnnotations;

namespace CatalogApp.Models
{
    public class Professor
    {
        [Key]
        public Guid ProfessorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Course>? Courses { get; set; }
        public ICollection<Grade>? Grades { get; set; }
    }
}
