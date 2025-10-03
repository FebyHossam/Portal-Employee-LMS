using Leave_Mangement_System.Data;
using Leave_Mangement_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Leave_Mangement_System.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _context;
        public DepartmentService(ApplicationDbContext context)
        {
            _context = context;
        }


        public  List<Department> GetAll()
        {
            return _context.Departments
                 .Include(d => d.Employees)
                 .OrderBy(d => d.Name)
                 .ToList();
        }


        public Department? GetById(int id)
        {
            return _context.Departments
                 .Include(d => d.Employees)
                 .FirstOrDefault(d => d.DeptId == id);
        }

        public bool Create(Department department)
        {
            try
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(Department department)
        {
            try
            {
                _context.Departments.Update(department);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var department = _context.Departments.Find(id);
                if (department == null) return false;

                
                var hasEmployees = _context.Employees.Any(e => e.DeptId == id);
                if (hasEmployees) return false;

                _context.Departments.Remove(department);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

       
    }
}
