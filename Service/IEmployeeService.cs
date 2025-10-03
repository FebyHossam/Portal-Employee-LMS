using Leave_Mangement_System.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Leave_Mangement_System.Service
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();

        Employee? GetById(int id);
        bool Create(Employee employee);
        bool Update(Employee employee);
        bool Delete(int id);

        IEnumerable<SelectListItem> GetDepartmentSelectList();
        IEnumerable<SelectListItem> GetPolicySelectList();
        Employee GetByIdWithDepartment(int id);
        Department GetDepartmentById(int departmentId);
    }
}
