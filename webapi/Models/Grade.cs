using System.ComponentModel.DataAnnotations;

namespace CatalogApp.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }
        [Range(1, 10)]
        public double Value { get; set; }
        public Student? Student { get; set; }
        public Course? Course { get; set; }
        public Professor? Professor { get; set; }
    }
}
