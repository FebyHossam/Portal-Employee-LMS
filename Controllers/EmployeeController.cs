using Leave_Mangement_System.Models;
using Leave_Mangement_System.Service;
using Leave_Mangement_System.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Leave_Mangement_System.Controllers
{
    [Authorize(Roles = "HRPrimary")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;

        public EmployeeController(
            IEmployeeService employeeService,
            UserManager<ApplicationUser> userManager,
            IEmailService emailService)
        {
            _employeeService = employeeService;
            _userManager = userManager;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var employees = _employeeService.GetAll();
            return View(employees);
        }

        public IActionResult Details(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee is null)
                return NotFound();
            return View(employee);
        }

        // ====================== CREATE ======================
        [HttpGet]
        [Authorize(Roles = "HRPrimary")]
        public IActionResult Create()
        {
            ViewBag.Departments = _employeeService.GetDepartmentSelectList();
            ViewBag.Policies = _employeeService.GetPolicySelectList();
            return View(new CreateEmployeeViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public async Task<IActionResult> Create(CreateEmployeeViewModel model)
        {
            ModelState.Remove("Department");
            ModelState.Remove("LeavePolicy");
            ModelState.Remove("LeaveRequests");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _employeeService.GetDepartmentSelectList();
                ViewBag.Policies = _employeeService.GetPolicySelectList();
                return View(model);
            }

            try
            {
                string generatedPassword = GenerateRandomPassword();

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.EmployeeName,
                    IsManager = model.Manager,
                    IsHR = model.Hr,
                    EmailConfirmed = true,
                    MustChangePassword = true  // ✅ ده اللي كان ناقص!
                };

                var userResult = await _userManager.CreateAsync(user, generatedPassword);

                if (!userResult.Succeeded)
                {
                    foreach (var error in userResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    ViewBag.Departments = _employeeService.GetDepartmentSelectList();
                    ViewBag.Policies = _employeeService.GetPolicySelectList();
                    return View(model);
                }

                var roles = new List<string> { "Employee" };
                if (model.Manager) roles.Add("Manager");
                if (model.Hr) roles.Add("HR");
                await _userManager.AddToRolesAsync(user, roles);

                var employee = new Employee
                {
                    EmployeeName = model.EmployeeName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    JobTitle = model.JobTitle,
                    Address = model.Address,
                    DeptId = model.DeptId,
                    PolicyId = model.PolicyId,
                    Manager = model.Manager,
                    Hr = model.Hr,
                    LeaveBalance = model.LeaveBalance,
                    ManagerName = model.ManagerName,
                    ManagerEmail = model.ManagerEmail,
                    UserId = user.Id,
                };

                var employeeResult = _employeeService.Create(employee);

                if (employeeResult)
                {
                    TempData["GeneratedPassword"] = generatedPassword;
                    TempData["EmployeeEmail"] = model.Email;
                    TempData["SuccessMessage"] = $"Employee '{model.EmployeeName}' created successfully!";
                    return RedirectToAction(nameof(Details), new { id = employee.EmpId });
                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    TempData["ErrorMessage"] = "Error occurred while creating employee record.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            ViewBag.Departments = _employeeService.GetDepartmentSelectList();
            ViewBag.Policies = _employeeService.GetPolicySelectList();
            return View(model);
        }




        // ====================== EDIT ======================
        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Edit(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee is null)
                return NotFound();

            ViewBag.Departments = _employeeService.GetDepartmentSelectList();
            ViewBag.Policies = _employeeService.GetPolicySelectList();
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee model)
        {
            ModelState.Remove("Department");
            ModelState.Remove("LeavePolicy");
            ModelState.Remove("LeaveRequests");
            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = _employeeService.GetDepartmentSelectList();
                ViewBag.Policies = _employeeService.GetPolicySelectList();
                return View(model);
            }

            var result = _employeeService.Update(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Employee updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while updating employee.";
                ViewBag.Departments = _employeeService.GetDepartmentSelectList();
                ViewBag.Policies = _employeeService.GetPolicySelectList();
                return View(model);
            }
        }

        // ====================== DELETE ======================
        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Delete(int id)
        {
            var employee = _employeeService.GetById(id);
            if (employee is null)
                return NotFound();
            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var employee = _employeeService.GetById(id);
                if (employee != null && !string.IsNullOrEmpty(employee.UserId))
                {
                    var user = await _userManager.FindByIdAsync(employee.UserId);
                    if (user != null)
                        await _userManager.DeleteAsync(user);
                }

                var result = _employeeService.Delete(id);
                if (result)
                    TempData["SuccessMessage"] = "Employee and associated user account deleted successfully!";
                else
                    TempData["ErrorMessage"] = "Error occurred while deleting employee.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }


        // ====================== RESET PASSWORD ======================
        [HttpPost]
        [Authorize(Policy = "HRPrimaryOnly")]
        public async Task<IActionResult> ResetPassword(int employeeId)
        {
            try
            {
                var employee = _employeeService.GetById(employeeId);
                if (employee == null || string.IsNullOrEmpty(employee.UserId))
                {
                    TempData["ErrorMessage"] = "Employee not found or not linked to user account.";
                    return RedirectToAction(nameof(Index));
                }

                var user = await _userManager.FindByIdAsync(employee.UserId);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "User account not found.";
                    return RedirectToAction(nameof(Index));
                }

                string newPassword = GenerateRandomPassword();

                // إزالة الباسورد القديم وإضافة الجديد
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, newPassword);

                user.MustChangePassword = true;
                await _userManager.UpdateAsync(user);
                // حفظ الباسورد الجديد مؤقتًا للـ HRPrimary
                TempData["GeneratedPassword"] = newPassword;
                TempData["SuccessMessage"] = $"Password reset successfully for {employee.EmployeeName}. Share it securely.";

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error resetting password: {ex.Message}";
            }

            return RedirectToAction(nameof(Details), new { id = employeeId });
        }


        // ====================== HELPERS ======================
        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

       
    }
}
