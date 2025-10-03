using Leave_Mangement_System.Data;
using Leave_Mangement_System.Models;
using Leave_Mangement_System.Service;
using Leave_Mangement_System.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Leave_Mangement_System.Controllers
{
    [Authorize]
    public class LeaveRequestController : Controller
    {
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        
        private readonly UserManager<ApplicationUser> _userManager;

        public LeaveRequestController(
            
            ILeaveRequestService leaveRequestService,
            IEmployeeService employeeService,
            IDepartmentService departmentService,
           
            UserManager<ApplicationUser> userManager )
        {
           
            _leaveRequestService = leaveRequestService;
            _employeeService = employeeService;
            _departmentService = departmentService;
            _userManager = userManager;
            
        }

        public async Task<IActionResult> Index()
        {
            var currentUserEmail = User.Identity?.Name;
            var isHRPrimary = User.IsInRole("HRPrimary");
            var isManager = User.IsInRole("Manager");

            var currentUser = await _userManager.FindByEmailAsync(currentUserEmail);
            var currentEmployee = _employeeService.GetAll()
                ?.FirstOrDefault(e => e.UserId == currentUser.Id || e.Email == currentUser.Email);

            // 🔍 Debugging: شوف كل الطلبات الموجودة
            var allRequests = _leaveRequestService.GetAll().ToList();
            ViewBag.TotalRequestsInDB = allRequests.Count;
            ViewBag.CurrentUserEmail = currentUserEmail;
            ViewBag.IsHRPrimary = isHRPrimary;
            ViewBag.IsManager = isManager;

            // طلبات المستخدم الحالي
            var myRequests = _leaveRequestService.GetAll()
                .Where(lr => lr.Employee != null && lr.Employee.Email == currentUserEmail)
                .ToList();

            List<LeaveRequest> teamRequests = new();

            if (isManager && currentEmployee != null)
            {
                teamRequests = _leaveRequestService.GetAll()
                    .Where(lr => lr.Employee != null &&
                                 lr.Employee.DeptId == currentEmployee.DeptId &&
                                 lr.Employee.Email != currentUserEmail &&
                                 lr.ManagerApproved == false &&
                                 lr.IsRejected == false)
                    .ToList();
            }
            else if (isHRPrimary)
            {
                // 🔍 اطبع كل الطلبات اللي ممكن تظهر
                var debugRequests = _leaveRequestService.GetAll()
                    .Where(lr => lr.Employee != null)
                    .Select(lr => new {
                        lr.RequestId,
                        lr.Employee.Email,
                        lr.ManagerApproved,
                        lr.HrApproved,
                        lr.IsRejected
                    })
                    .ToList();

                ViewBag.DebugRequests = debugRequests;

                teamRequests = _leaveRequestService.GetAll()
                    .Where(lr => lr.Employee != null &&
                                 lr.Employee.Email != currentUserEmail &&
                                 lr.ManagerApproved == true &&
                                 lr.HrApproved == false &&
                                 lr.IsRejected == false)
                    .ToList();
            }

            var viewModel = new LeaveRequestIndexViewModel
            {
                MyRequests = myRequests,
                TeamRequests = teamRequests
            };

            return View(viewModel);
        }


        public IActionResult Details(int id)
        {
            var leaveRequest = _leaveRequestService.GetById(id);
            if (leaveRequest == null)
                return NotFound();

            var currentUserEmail = User.Identity.Name;
            var isHRPrimary = User.IsInRole("HRPrimary");
            var isManager = User.IsInRole("Manager");

            
            if (!isHRPrimary && !isManager && leaveRequest.Employee.Email != currentUserEmail)
            {
                return Forbid();
            }

            
            if (isManager && !isHRPrimary)
            {
                var currentUser = _userManager.FindByEmailAsync(currentUserEmail).Result;
                var managerEmployee = _employeeService.GetAll()?.FirstOrDefault(e => e.UserId == currentUser.Id || e.Email == currentUser.Email);

                if (managerEmployee != null && leaveRequest.Employee.DeptId != managerEmployee.DeptId)
                {
                    return Forbid();
                }
            }

            return View(leaveRequest);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new LeaveRequest();

            try
            {
                var isHRPrimary = User.IsInRole("HRPrimary");
                var isManager = User.IsInRole("Manager");

                await PrepareViewBagDataEnhanced(isHRPrimary, isManager);

               
                if (!isHRPrimary && !isManager && ViewBag.CurrentEmployeeId != null && (int)ViewBag.CurrentEmployeeId > 0)
                {
                    model.EmpId = (int)ViewBag.CurrentEmployeeId;
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while loading the create form. Please try again.";
                await PrepareViewBagDataEnhanced(false, false);
                return View(model);
            }
        }

      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LeaveRequest model)
        {
            try
            {
                var isHRPrimary = User.IsInRole("HRPrimary");
                var isManager = User.IsInRole("Manager");

                if (!isHRPrimary && !isManager)
                {
                    var currentUser = await _userManager.FindByEmailAsync(User.Identity?.Name ?? "");
                    if (currentUser != null)
                    {
                        var employee = _employeeService.GetAll()
                            ?.FirstOrDefault(e => e.UserId == currentUser.Id || e.Email == currentUser.Email);

                        if (employee != null)
                        {
                            model.EmpId = employee.EmpId;
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Employee profile not found.";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }

                if (model.StartDate != DateTime.MinValue && model.EndDate != DateTime.MinValue)
                {
                    var days = (model.EndDate - model.StartDate).TotalDays + 1;
                    model.NumberOfDays = days > 0 ? (int)days : 0;
                }

                model.HrApproved = false;
                model.ManagerApproved = false;
                model.IsRejected = false;
                model.RequestId = 0;
                model.Employee = null;

                var result = _leaveRequestService.Create(model);

                if (result)
                {
                    TempData["SuccessMessage"] = "Leave Request created successfully!";
                    return RedirectToAction(nameof(Index));
                }

                TempData["ErrorMessage"] = "Error occurred while creating Leave Request.";
                await PrepareViewBagDataEnhanced(isHRPrimary, isManager);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var leaveRequest = _leaveRequestService.GetById(id);
            if (leaveRequest == null)
                return NotFound();

            var isHRPrimary = User.IsInRole("HRPrimary");
            var isManager = User.IsInRole("Manager");
            var currentUserEmail = User.Identity.Name;

            
            if (!isHRPrimary && !isManager)
            {
                if (leaveRequest.Employee.Email != currentUserEmail)
                {
                    return Forbid();
                }

                if (leaveRequest.ManagerApproved || leaveRequest.HrApproved)
                {
                    TempData["ErrorMessage"] = "Cannot edit approved leave requests.";
                    return RedirectToAction(nameof(Index));
                }
            }

            await PrepareViewBagDataEnhanced(isHRPrimary, isManager);

            if (!isHRPrimary && !isManager)
            {
                ViewBag.CurrentEmployeeName = leaveRequest.Employee.EmployeeName;
                ViewBag.CurrentEmployeeId = leaveRequest.EmpId;
                ViewBag.CurrentEmployeeBalance = leaveRequest.Employee.LeaveBalance;
                ViewBag.CurrentEmployeePhone = leaveRequest.Employee.PhoneNumber ?? "";
                ViewBag.CurrentEmployeeAddress = leaveRequest.Employee.Address ?? "";
                ViewBag.CurrentEmployeeJobTitle = leaveRequest.Employee.JobTitle ?? "";
                ViewBag.CurrentEmployeeDeptId = leaveRequest.Employee.DeptId;
                ViewBag.CurrentEmployeeDepartmentName = leaveRequest.Employee.Department?.Name ?? "";
            }

            return View(leaveRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LeaveRequest model, string EmployeeJobTitle, string EmployeePhoneNumber, string EmployeeAddress, int EmployeeDeptId)
        {
            try
            {
                var existingRequest = _leaveRequestService.GetById(model.RequestId);
                if (existingRequest == null)
                    return NotFound();

                var isHRPrimary = User.IsInRole("HRPrimary");
                var isManager = User.IsInRole("Manager");

                if (!isHRPrimary && !isManager)
                {
                    var currentUser = await _userManager.FindByEmailAsync(User.Identity?.Name ?? "");
                    if (currentUser?.Email != existingRequest.Employee.Email)
                        return Forbid();

                    if (existingRequest.ManagerApproved || existingRequest.HrApproved)
                    {
                        TempData["ErrorMessage"] = "Cannot edit approved requests.";
                        return RedirectToAction(nameof(Index));
                    }

                    var employee = _employeeService.GetById(existingRequest.EmpId);
                    if (employee != null)
                    {
                        employee.JobTitle = EmployeeJobTitle;
                        employee.PhoneNumber = EmployeePhoneNumber;
                        employee.Address = EmployeeAddress;
                        employee.DeptId = EmployeeDeptId;
                        _employeeService.Update(employee);
                    }

                    model.EmpId = existingRequest.EmpId;
                }

                if (model.StartDate != DateTime.MinValue && model.EndDate != DateTime.MinValue)
                {
                    var days = (model.EndDate - model.StartDate).TotalDays + 1;
                    model.NumberOfDays = days > 0 ? (int)days : 0;
                }

                existingRequest.LeaveType = model.LeaveType;
                existingRequest.StartDate = model.StartDate;
                existingRequest.EndDate = model.EndDate;
                existingRequest.NumberOfDays = model.NumberOfDays;
                existingRequest.Reason = model.Reason ?? "";

                if (!isHRPrimary && !isManager)
                {
                    existingRequest.HrApproved = false;
                    existingRequest.ManagerApproved = false;
                    existingRequest.IsRejected = false;
                }

                var result = _leaveRequestService.Update(existingRequest);

                if (result)
                {
                    TempData["SuccessMessage"] = "Leave Request updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error updating request.";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        private async Task PrepareViewBagDataEnhanced(bool isHRPrimary, bool isManager)
        {
            try
            {
                if (isHRPrimary || isManager)
                {
                    var employees = _employeeService.GetAll() ?? new List<Employee>();

                    
                    if (isManager && !isHRPrimary)
                    {
                        var currentUser = await _userManager.FindByEmailAsync(User.Identity?.Name ?? "");
                        var managerEmployee = employees.FirstOrDefault(e => e.UserId == currentUser.Id || e.Email == currentUser.Email);

                        if (managerEmployee != null)
                        {
                            employees = employees.Where(e => e.DeptId == managerEmployee.DeptId).ToList();
                        }
                    }

                    ViewBag.Employees = employees
                        .Select(e => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                        {
                            Value = e.EmpId.ToString(),
                            Text = $"{e.EmployeeName} - {e.Department?.Name ?? "No Department"}"
                        }).ToList();
                }
                else
                {
                    ViewBag.CurrentEmployeeName = "Loading...";
                    ViewBag.CurrentEmployeeId = 0;
                    ViewBag.CurrentEmployeeBalance = 0;
                    ViewBag.CurrentEmployeePhone = "";
                    ViewBag.CurrentEmployeeAddress = "";
                    ViewBag.CurrentEmployeeJobTitle = "";
                    ViewBag.CurrentEmployeeDeptId = 0;
                    ViewBag.CurrentEmployeeDepartmentName = "";

                    var currentUser = await _userManager.FindByEmailAsync(User.Identity?.Name ?? "");
                    if (currentUser != null)
                    {
                        var employee = _employeeService.GetAll()?.FirstOrDefault(e => e.UserId == currentUser.Id);

                        if (employee == null)
                        {
                            employee = _employeeService.GetAll()?.FirstOrDefault(e => e.Email == currentUser.Email);

                            if (employee != null)
                            {
                                employee.UserId = currentUser.Id;
                                _employeeService.Update(employee);
                            }
                        }

                        if (employee != null)
                        {
                            ViewBag.CurrentEmployeeName = employee.EmployeeName;
                            ViewBag.CurrentEmployeeId = employee.EmpId;
                            ViewBag.CurrentEmployeeBalance = employee.LeaveBalance;
                            ViewBag.CurrentEmployeePhone = employee.PhoneNumber ?? "";
                            ViewBag.CurrentEmployeeAddress = employee.Address ?? "";
                            ViewBag.CurrentEmployeeJobTitle = employee.JobTitle ?? "";
                            ViewBag.CurrentEmployeeDeptId = employee.DeptId;
                            ViewBag.CurrentEmployeeDepartmentName = employee.Department?.Name ?? "";
                        }
                        else
                        {
                            ViewBag.CurrentEmployeeName = $"Employee Not Found for {currentUser.Email}";
                        }
                    }
                }

                try
                {
                    var departments = _departmentService.GetAll();

                    if (departments != null && departments.Any())
                    {
                        ViewBag.Departments = departments
                            .OrderBy(d => d.Name)
                            .Select(d => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                            {
                                Value = d.DeptId.ToString(),
                                Text = d.Name
                            }).ToList();
                    }
                    else
                    {
                        ViewBag.Departments = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Departments = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                }
            }
            catch (Exception ex)
            {
                ViewBag.CurrentEmployeeName = "Error Loading Employee Data";
                ViewBag.CurrentEmployeeId = 0;
                ViewBag.CurrentEmployeeBalance = 0;
                ViewBag.CurrentEmployeePhone = "";
                ViewBag.CurrentEmployeeAddress = "";
                ViewBag.CurrentEmployeeJobTitle = "";
                ViewBag.CurrentEmployeeDeptId = 0;
                ViewBag.CurrentEmployeeDepartmentName = "";
                ViewBag.Employees = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
                ViewBag.Departments = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetEmployeeData(int employeeId)
        {
            try
            {
                var employee = _employeeService.GetById(employeeId);
                if (employee != null)
                {
                    return Json(new
                    {
                        success = true,
                        phoneNumber = employee.PhoneNumber ?? "",
                        address = employee.Address ?? "",
                        jobTitle = employee.JobTitle ?? "",
                        departmentName = employee.Department?.Name ?? "",
                        deptId = employee.DeptId,
                        leaveBalance = employee.LeaveBalance
                    });
                }
                return Json(new { success = false, message = "Employee not found" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error loading employee data" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var leaveRequest = _leaveRequestService.GetById(id);
            if (leaveRequest == null)
                return NotFound();

            var isHRPrimary = User.IsInRole("HRPrimary");
            var isManager = User.IsInRole("Manager");
            var currentUserEmail = User.Identity.Name;

            if (!isHRPrimary && !isManager)
            {
                if (leaveRequest.Employee.Email != currentUserEmail)
                {
                    return Forbid();
                }

                if (leaveRequest.ManagerApproved || leaveRequest.HrApproved)
                {
                    TempData["ErrorMessage"] = "Cannot delete approved leave requests.";
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(leaveRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leaveRequest = _leaveRequestService.GetById(id);
            if (leaveRequest == null)
                return NotFound();

            var isHRPrimary = User.IsInRole("HRPrimary");
            var isManager = User.IsInRole("Manager");
            var currentUserEmail = User.Identity.Name;

            if (!isHRPrimary && !isManager)
            {
                if (leaveRequest.Employee.Email != currentUserEmail)
                {
                    return Forbid();
                }

                if (leaveRequest.ManagerApproved || leaveRequest.HrApproved)
                {
                    TempData["ErrorMessage"] = "Cannot delete approved leave requests.";
                    return RedirectToAction(nameof(Index));
                }
            }

            var result = _leaveRequestService.Delete(id);
            TempData["SuccessMessage"] = result
                ? "Leave Request deleted successfully!"
                : "Error occurred while deleting Leave Request.";

            return RedirectToAction(nameof(Index));
        }
        //====================================================================================


        [HttpPost]
        [Authorize(Roles = "HRPrimary")]
        public IActionResult HRApprove(int requestId)
        {
            var leaveRequest = _leaveRequestService.GetById(requestId);
            if (leaveRequest == null)
            {
                TempData["ErrorMessage"] = "Leave request not found.";
                return RedirectToAction(nameof(Index));
            }

            if (!leaveRequest.ManagerApproved)
            {
                TempData["ErrorMessage"] = "This request requires manager approval first before HR can approve it.";
                return RedirectToAction(nameof(Index));
            }

            if (leaveRequest.HrApproved)
            {
                TempData["WarningMessage"] = "This request has already been approved by HR.";
                return RedirectToAction(nameof(Index));
            }

     
            leaveRequest.HrApproved = true;
            leaveRequest.IsRejected = false;
            leaveRequest.HrApprovalDate = DateTime.Now;

            var employee = _employeeService.GetById(leaveRequest.EmpId);
            if (employee != null)
            {
                int oldBalance = employee.LeaveBalance;
                employee.LeaveBalance -= leaveRequest.NumberOfDays;

           
                if (employee.LeaveBalance < 0)
                    employee.LeaveBalance = 0;

                _employeeService.Update(employee);

                TempData["SuccessMessage"] = $"Leave request approved successfully by HR. " +
                    $"{leaveRequest.NumberOfDays} days deducted from employee balance. " +
                    $"Previous balance: {oldBalance} days, Current balance: {employee.LeaveBalance} days.";
            }
            else
            {
                TempData["SuccessMessage"] = "Leave request approved successfully by HR.";
            }

            _leaveRequestService.Update(leaveRequest);
            return RedirectToAction(nameof(Index));
        }

        //==============================================================================================
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> ManagerApprove(int requestId)
        {
            var leaveRequest = _leaveRequestService.GetById(requestId);
            if (leaveRequest == null)
            {
                TempData["ErrorMessage"] = "Leave request not found.";
                return RedirectToAction(nameof(Index));
            }

            var currentUser = await _userManager.FindByEmailAsync(User.Identity?.Name ?? "");
            if (currentUser == null)
            {
                TempData["ErrorMessage"] = "User not found.";
                return RedirectToAction(nameof(Index));
            }

            var managerEmployee = _employeeService.GetAll()?
                .FirstOrDefault(e => e.UserId == currentUser.Id || e.Email == currentUser.Email);

            if (managerEmployee == null)
            {
                TempData["ErrorMessage"] = "Manager profile not found. Please contact HR.";
                return RedirectToAction(nameof(Index));
            }

            
            if (leaveRequest.Employee.EmpId == managerEmployee.EmpId)
            {
                TempData["ErrorMessage"] = "You cannot approve your own leave request.";
                return RedirectToAction(nameof(Index));
            }

            if (leaveRequest.Employee.DeptId != managerEmployee.DeptId)
            {
                TempData["ErrorMessage"] = "You can only approve leave requests from employees in your department.";
                return RedirectToAction(nameof(Index));
            }

            if (leaveRequest.ManagerApproved)
            {
                TempData["WarningMessage"] = "This request has already been approved by a manager.";
                return RedirectToAction(nameof(Index));
            }

            leaveRequest.ManagerApproved = true;
            leaveRequest.IsRejected = false;
            leaveRequest.ManagerApprovalDate = DateTime.Now;

            var result = _leaveRequestService.Update(leaveRequest);

            if (result)
            {
                TempData["SuccessMessage"] = $"Leave request approved successfully. The request has been forwarded to HR for final approval.";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating the request.";
            }

            return RedirectToAction(nameof(Index));
        }


        //====================================================================================
        [HttpPost]
        [Authorize(Roles = "Manager,HRPrimary")]
        public async Task<IActionResult> RejectRequest(int requestId)
        {
            var leaveRequest = _leaveRequestService.GetById(requestId);
            if (leaveRequest == null)
            {
                TempData["ErrorMessage"] = "Leave request not found.";
                return RedirectToAction(nameof(Index));
            }

          
            if (User.IsInRole("Manager") && !User.IsInRole("HRPrimary"))
            {
                var currentUser = await _userManager.FindByEmailAsync(User.Identity?.Name ?? "");
                if (currentUser == null)
                {
                    TempData["ErrorMessage"] = "User not found.";
                    return RedirectToAction(nameof(Index));
                }

                var managerEmployee = _employeeService.GetAll()?
                    .FirstOrDefault(e => e.UserId == currentUser.Id || e.Email == currentUser.Email);

                if (managerEmployee == null)
                {
                    TempData["ErrorMessage"] = "Manager profile not found. Please contact HR.";
                    return RedirectToAction(nameof(Index));
                }

                if (leaveRequest.Employee.DeptId != managerEmployee.DeptId)
                {
                    TempData["ErrorMessage"] = "You can only reject leave requests from employees in your department.";
                    return RedirectToAction(nameof(Index));
                }
            }
          
            leaveRequest.IsRejected = true;
            leaveRequest.ManagerApproved = false;
            leaveRequest.HrApproved = false;
            leaveRequest.ManagerApprovalDate = null;
            leaveRequest.HrApprovalDate = null;

            var result = _leaveRequestService.Update(leaveRequest);

            if (result)
            {
                string rejectedBy = User.IsInRole("HRPrimary") ? "HR" : "Manager";
                TempData["SuccessMessage"] = $"Leave request rejected successfully by {rejectedBy}.";
            }
            else
            {
                TempData["ErrorMessage"] = "An error occurred while updating the request.";
            }

            return RedirectToAction(nameof(Index));
        }


        //==================================================================================================
        [HttpGet]
        public JsonResult SearchEmployees(string term)
        {
            var employees = _employeeService.GetAll()
                .Where(e => e.EmployeeName.Contains(term ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                .Select(e => new { label = e.EmployeeName, value = e.EmpId })
                .ToList();

            return Json(employees);
        }

        [HttpDelete]
        [Authorize(Roles = "HRPrimary,Manager")]
        public IActionResult DeleteAjax(int id)
        {
            var isDeleted = _leaveRequestService.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }

        public IActionResult Print(int id)
        {
            var leaveRequest = _leaveRequestService.GetById(id);
            if (leaveRequest == null)
                return NotFound();

            return View(leaveRequest);
        }

        [HttpGet]
        public IActionResult PrintForm(int id)
        {
            try
            {
                var leaveRequest = _leaveRequestService.GetById(id);
                if (leaveRequest == null)
                {
                    TempData["ErrorMessage"] = "Leave request not found.";
                    return RedirectToAction(nameof(Index));
                }

                var isHRPrimary = User.IsInRole("HRPrimary");
                var isManager = User.IsInRole("Manager");
                var currentUser = _userManager.FindByEmailAsync(User.Identity?.Name ?? "").Result;

                if (!isHRPrimary && !isManager)
                {
                    if (leaveRequest.Employee.Email != currentUser?.Email)
                    {
                        return Forbid("You can only print your own leave requests.");
                    }
                }

                var employee = _employeeService.GetById(leaveRequest.EmpId);

                if (employee?.Department == null && employee?.DeptId != null)
                {
                    var department = _departmentService?.GetById(employee.DeptId);
                    if (department != null)
                    {
                        employee.Department = department;
                    }
                }

                leaveRequest.Employee = employee;

                ViewBag.PrintDate = DateTime.Now;
                ViewBag.CompanyName = "شركة البحر الأحمر للمقاولات والتشييد والتنمية";

                var totalDays = (leaveRequest.EndDate - leaveRequest.StartDate).Days + 1;
                var workingDays = leaveRequest.NumberOfDays;
                var weekendDays = totalDays - workingDays;

                ViewBag.WeekendDays = weekendDays;
                ViewBag.PublicHolidays = 0;
                ViewBag.TotalDays = totalDays;

                var balanceBeforeLeave = employee?.LeaveBalance ?? 0;
                if (leaveRequest.HrApproved && leaveRequest.ManagerApproved)
                {
                    balanceBeforeLeave += leaveRequest.NumberOfDays;
                }

                ViewBag.BalanceBeforeLeave = balanceBeforeLeave;
                ViewBag.BalanceAfterLeave = employee?.LeaveBalance ?? 0;

                return View("PrintForm", leaveRequest);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error loading print form: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public IActionResult QuickPrint(int id)
        {
            var result = PrintForm(id);
            if (result is ViewResult viewResult)
            {
                ViewBag.AutoPrint = true;
                return viewResult;
            }
            return result;
        }

        

    }
}