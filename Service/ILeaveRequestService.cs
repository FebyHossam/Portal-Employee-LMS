using Leave_Mangement_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Leave_Mangement_System.Service
{
    public interface ILeaveRequestService
    {
        List<LeaveRequest> GetAll();
        LeaveRequest? GetById(int id);
        bool Create(LeaveRequest leaveRequest);
        bool Update(LeaveRequest leaveRequest);
        bool Delete(int id);

        List<Employee> SearchEmployees(string searchTerm);
        Employee? GetEmployeeById(int empId);
        IEnumerable<SelectListItem> GetEmployeeSelectList();
    }
}
