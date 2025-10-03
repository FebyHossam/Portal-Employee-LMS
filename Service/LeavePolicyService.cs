using Leave_Mangement_System.Data;
using Leave_Mangement_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Leave_Mangement_System.Service
{
    public class LeavePolicyService : ILeavePolicyService
    {

        private readonly ApplicationDbContext _context;

        public LeavePolicyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<LeavePolicy> GetAll()
        {
            return _context.LeavePolicies
                .Include(p => p.Employees)
                .OrderBy(p => p.PolicyName)
                .ToList();
        }

        public LeavePolicy? GetById(int id)
        {
            return _context.LeavePolicies
                .Include(p => p.Employees)
                .ThenInclude(e => e.Department)
                .FirstOrDefault(p => p.PolicyId == id);
        }

        public bool Create(LeavePolicy policy)
        {
            try
            {
                policy.Employees = new List<Employee>();
                _context.LeavePolicies.Add(policy);
                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating leave policy: {ex.Message}");
                return false;
            }
        }

        public bool Update(LeavePolicy policy)
        {
            try
            {
                var existingPolicy = _context.LeavePolicies.Find(policy.PolicyId);
                if (existingPolicy == null) return false;

                existingPolicy.PolicyName = policy.PolicyName;
                existingPolicy.RegularDays = policy.RegularDays;
                existingPolicy.ExceptionDays = policy.ExceptionDays;

                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating leave policy: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var policy = _context.LeavePolicies.Find(id);
                if (policy == null) return false;

                // Check if policy has employees
                var hasEmployees = _context.Employees.Any(e => e.PolicyId == id);
                if (hasEmployees) return false;

                _context.LeavePolicies.Remove(policy);
                var result = _context.SaveChanges();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting leave policy: {ex.Message}");
                return false;
            }
        }

        public bool Exists(int id)
        {
            return _context.LeavePolicies.Any(p => p.PolicyId == id);
        }

    }
}
