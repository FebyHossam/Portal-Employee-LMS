using Leave_Mangement_System.Models;

namespace Leave_Mangement_System.Service
{
    public interface IDepartmentService
    {
        List<Department> GetAll();
        Department? GetById(int id);
        bool Create(Department department);
        bool Update(Department department);
        bool Delete(int id);
        
    }
}
