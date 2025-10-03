using Leave_Mangement_System.Models;
using Leave_Mangement_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Mangement_System.Controllers
{
    [Authorize(Roles = "HRPrimary")]
    public class LeavePolicyController : Controller
    {
        private readonly ILeavePolicyService _leavePolicyService;

        public LeavePolicyController(ILeavePolicyService leavePolicyService)
        {
            _leavePolicyService = leavePolicyService;
        }

        public IActionResult Index()
        {
            var leavePolicies = _leavePolicyService.GetAll();
            return View(leavePolicies);
        }

        public IActionResult Details(int id)
        {
            var leavePolicy = _leavePolicyService.GetById(id);
            if (leavePolicy is null)
                return NotFound();

            return View(leavePolicy);
        }

        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Create(LeavePolicy model)
        {
            ModelState.Remove("RegularDays");
            ModelState.Remove("ExceptionDays");
            ModelState.Remove("Employees");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _leavePolicyService.Create(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Leave Policy created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while creating Leave Policy.";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Edit(int id)
        {
            var leavePolicy = _leavePolicyService.GetById(id);
            if (leavePolicy is null)
                return NotFound();

            return View(leavePolicy);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Edit(LeavePolicy model)
        {
            ModelState.Remove("RegularDays");
            ModelState.Remove("ExceptionDays");
            ModelState.Remove("Employees");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _leavePolicyService.Update(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Leave Policy updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while updating Leave Policy.";
                return View(model);
            }
        }

        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Delete(int id)
        {
            var leavePolicy = _leavePolicyService.GetById(id);
            if (leavePolicy is null)
                return NotFound();

            return View(leavePolicy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _leavePolicyService.Delete(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Leave Policy deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while deleting Leave Policy.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult DeleteAjax(int id)
        {
            var isDeleted = _leavePolicyService.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }
    }
}
