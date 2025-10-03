using Leave_Mangement_System.Models;

namespace Leave_Mangement_System.ViewModels
{
    public class LeaveRequestIndexViewModel
    {
        public List<LeaveRequest> MyRequests { get; set; } = new();
        public List<LeaveRequest> TeamRequests { get; set; } = new();
    }
}
