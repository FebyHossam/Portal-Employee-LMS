using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Leave_Mangement_System.Models
{
    public class LeaveRequest
    {
        [Key]
        public int RequestId { get; set; }

        public int EmpId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int NumberOfDays { get; set; }
        [Required]
        [StringLength(50)]
        public string LeaveType { get; set; } = "Normal"; 
        [StringLength(500)]
        public string? Reason { get; set; } 

        public bool HrApproved { get; set; }

        public bool ManagerApproved { get; set; }

        public bool IsRejected { get; set; }


        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }
        public DateTime? ManagerApprovalDate { get; set; }
        public DateTime? HrApprovalDate { get; set; }

    }
}