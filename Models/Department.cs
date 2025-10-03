using System.ComponentModel.DataAnnotations;

namespace Leave_Mangement_System.Models
{
    public class Department
    {
        [Key]
        public int DeptId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
