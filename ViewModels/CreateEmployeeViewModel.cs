using System.ComponentModel.DataAnnotations;

namespace Leave_Mangement_System.ViewModels
{
    public class CreateEmployeeViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }
        public string? JobTitle { get; set; }
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Department")]
        public int DeptId { get; set; }

        [Required]
        [Display(Name = "Leave Policy")]
        public int PolicyId { get; set; }

        [Display(Name = "Leave Balance")]
        [Range(0, 365)]
        public int LeaveBalance { get; set; } = 0;

        [StringLength(100)]
        [Display(Name = "Manager Name")]
        public string? ManagerName { get; set; }

        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "Manager Email")]
        public string? ManagerEmail { get; set; }

        [Display(Name = "Is Manager")]
        public bool Manager { get; set; }

        [Display(Name = "Is HR")]
        public bool Hr { get; set; }
    }
}
