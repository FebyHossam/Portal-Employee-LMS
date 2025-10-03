using Leave_Mangement_System.Models;

namespace Leave_Mangement_System.Service
{
    public interface ILeavePolicyService
    {
        List<LeavePolicy> GetAll();

        LeavePolicy? GetById(int id);
        bool Create(LeavePolicy leavePolicy);
        bool Update(LeavePolicy leavePolicy);
        bool Delete(int id);
    }
}
