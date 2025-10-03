using Leave_Mangement_System.Data;
using Leave_Mangement_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Leave_Mangement_System.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Employee> GetAll()
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.LeavePolicy)
                .OrderBy(e => e.EmployeeName)
                .ToList();
        }

        public Employee? GetById(int id)
        {
            return _context.Employees
                .Include(e => e.Department)
                .Include(e => e.LeavePolicy)
                .Include(e => e.LeaveRequests)
                .FirstOrDefault(e => e.EmpId == id);
        }

        public bool Create(Employee employee)
        {
            try
            {
                employee.Department = null!;
                employee.LeavePolicy = null!;
                employee.User = null!;
                employee.LeaveRequests = new List<LeaveRequest>();

                var deptExists = _context.Departments.Any(d => d.DeptId == employee.DeptId);
                var policyExists = _context.LeavePolicies.Any(p => p.PolicyId == employee.PolicyId);

                if (!deptExists || !policyExists)
                {
                    Console.WriteLine($"Department exists: {deptExists}, Policy exists: {policyExists}");
                    return false;
                }

                _context.Employees.Add(employee);
                var result = _context.SaveChanges();

                Console.WriteLine($"SaveChanges returned: {result}");
                Console.WriteLine($"Employee created with ID: {employee.EmpId}");

                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Create: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }

        public bool Update(Employee employee)
        {
            try
            {
                var existingEmployee = _context.Employees.Find(employee.EmpId);
                if (existingEmployee == null) return false;

                existingEmployee.EmployeeName = employee.EmployeeName;
                existingEmployee.Email = employee.Email;
                existingEmployee.DeptId = employee.DeptId;
                existingEmployee.PolicyId = employee.PolicyId;
                existingEmployee.Manager = employee.Manager;
                existingEmployee.Hr = employee.Hr;
                existingEmployee.LeaveBalance = employee.LeaveBalance;
                existingEmployee.ManagerName = employee.ManagerName;
                existingEmployee.ManagerEmail = employee.ManagerEmail;
                existingEmployee.UserId = employee.UserId;

                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Update: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var employee = _context.Employees.Find(id);
                if (employee == null) return false;

                _context.Employees.Remove(employee);
                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Delete: {ex.Message}");
                return false;
            }
        }

        public IEnumerable<SelectListItem> GetDepartmentSelectList()
        {
            return _context.Departments
                .Select(d => new SelectListItem { Value = d.DeptId.ToString(), Text = d.Name })
                .OrderBy(d => d.Text)
                .ToList();
        }

        public IEnumerable<SelectListItem> GetPolicySelectList()
        {
            return _context.LeavePolicies
                .Select(p => new SelectListItem { Value = p.PolicyId.ToString(), Text = p.PolicyName })
                .OrderBy(p => p.Text)
                .ToList();
        }

        public Employee GetByIdWithDepartment(int id)
        {
            try
            {
                return _context.Employees
                    .Include(e => e.Department)
                    .FirstOrDefault(e => e.EmpId == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdWithDepartment: {ex.Message}");
                return GetById(id);
            }
        }

        public Department GetDepartmentById(int departmentId)
        {
            try
            {
                return _context.Departments.Find(departmentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDepartmentById: {ex.Message}");
                return new Department { DeptId = departmentId, Name = "غير محدد" };
            }
        }
    }
}
