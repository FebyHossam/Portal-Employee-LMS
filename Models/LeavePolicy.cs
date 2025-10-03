using System.ComponentModel.DataAnnotations;

namespace Leave_Mangement_System.Models
{
    public class LeavePolicy
    {
        [Key]
        public int PolicyId { get; set; }

        [Required]
        [StringLength(100)]
        public string PolicyName { get; set; }
        public int RegularDays { get; set; }

        public int ExceptionDays { get; set; }


        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

    }
}
