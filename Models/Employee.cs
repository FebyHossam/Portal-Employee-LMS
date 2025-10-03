using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leave_Mangement_System.Models
{
    public class Employee
    {
        [Key]
        public int EmpId { get; set; }

        [Required]
        [StringLength(100)]
        public string EmployeeName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        [StringLength(200)]
        public string? Address { get; set; }   

        [StringLength(100)]
        public string? JobTitle { get; set; }
        public bool Manager { get; set; }
        public bool Hr { get; set; }
        public int LeaveBalance { get; set; }

        [StringLength(100)]
        public string? ManagerName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string? ManagerEmail { get; set; }

    


        public int DeptId { get; set; }
        [ForeignKey("DeptId")]
        public Department Department { get; set; }

        public int PolicyId { get; set; }
        [ForeignKey("PolicyId")]
        public LeavePolicy LeavePolicy { get; set; }

        

       
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();


    }
}