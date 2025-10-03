using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Leave_Mangement_System.Models
{
    public class ApplicationUser : IdentityUser
    {

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        public bool IsManager { get; set; }
        public bool IsHR { get; set; }

        public bool MustChangePassword { get; set; } = false;

        public Employee? Employee { get; set; }
    }
}
