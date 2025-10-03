using Leave_Mangement_System.Data;
using Leave_Mangement_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Leave_Mangement_System.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LeaveRequest> GetAll()
        {
            return _context.LeaveRequests
                .Include(lr => lr.Employee)
                .ThenInclude(e => e.Department)
                .OrderByDescending(lr => lr.StartDate)
                .ToList();


        }

        public LeaveRequest? GetById(int id)
        {
            return _context.LeaveRequests
                .Include(lr => lr.Employee)
                .ThenInclude(e => e.Department)
                .FirstOrDefault(lr => lr.RequestId == id);
        }

        public bool Create(LeaveRequest leaveRequest)
        {
            try
            {
               
                leaveRequest.Employee = null!;

                
                if (leaveRequest.NumberOfDays <= 0)
                {
                    leaveRequest.NumberOfDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;
                }

                _context.LeaveRequests.Add(leaveRequest);
                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating leave request: {ex.Message}");
                return false;
            }
        }

        public bool Update(LeaveRequest leaveRequest)
        {
            try
            {
                var existingRequest = _context.LeaveRequests.Find(leaveRequest.RequestId);
                if (existingRequest == null) return false;

                existingRequest.EmpId = leaveRequest.EmpId;
                existingRequest.LeaveType = leaveRequest.LeaveType;
                existingRequest.StartDate = leaveRequest.StartDate;
                existingRequest.EndDate = leaveRequest.EndDate;
                existingRequest.NumberOfDays = leaveRequest.NumberOfDays;
                existingRequest.Reason = leaveRequest.Reason;
                existingRequest.HrApproved = leaveRequest.HrApproved;
                existingRequest.ManagerApproved = leaveRequest.ManagerApproved;
                existingRequest.IsRejected = leaveRequest.IsRejected;

                if (existingRequest.NumberOfDays <= 0)
                {
                    existingRequest.NumberOfDays = (existingRequest.EndDate - existingRequest.StartDate).Days + 1;
                }

                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating leave request: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var leaveRequest = _context.LeaveRequests.Find(id);
                if (leaveRequest == null) return false;

                _context.LeaveRequests.Remove(leaveRequest);
                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting leave request: {ex.Message}");
                return false;
            }
        }

        public List<Employee> SearchEmployees(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return new List<Employee>();

            return _context.Employees
                .Include(e => e.Department)
                .Where(e => e.EmployeeName.Contains(searchTerm) || e.Email.Contains(searchTerm))
                .Take(10)
                .ToList();
        }

        public Employee? GetEmployeeById(int empId)
        {
            return _context.Employees
                .Include(e => e.Department)
                .FirstOrDefault(e => e.EmpId == empId);
        }

        public IEnumerable<SelectListItem> GetEmployeeSelectList()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Select(e => new SelectListItem
                {
                    Value = e.EmpId.ToString(),
                    Text = $"{e.EmployeeName} - {e.Department.Name}"
                })
                .OrderBy(e => e.Text)
                .ToList();
        }
    }
}