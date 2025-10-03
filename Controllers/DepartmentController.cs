using Leave_Mangement_System.Models;
using Leave_Mangement_System.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Leave_Mangement_System.Controllers
{
    [Authorize(Roles = "HRPrimary")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Index()
        {
            var departments = _departmentService.GetAll();
            return View(departments);
        }


        
        public IActionResult Details(int id)
        {
            var department = _departmentService.GetById(id);
            if (department is null)
                return NotFound();
            return View(department);
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
        public IActionResult Create(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _departmentService.Create(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Department created successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while creating department.";
                return View(model);
            }
        }


        
        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Edit(int id)
        {
            var department = _departmentService.GetById(id);
            if (department is null)
                return NotFound();

            return View(department);
        }



       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Edit(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _departmentService.Update(model);
            if (result)
            {
                TempData["SuccessMessage"] = "Department updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while updating department.";
                return View(model);
            }
        }



        
        [HttpGet]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult Delete(int id)
        {
            var department = _departmentService.GetById(id);
            if (department is null)
                return NotFound();

            return View(department);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "HRPrimaryOnly")]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _departmentService.Delete(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Department deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Cannot delete department. It may have employees or an error occurred.";
            }
            return RedirectToAction(nameof(Index));
        }

       
        [HttpDelete]
        public IActionResult DeleteAjax(int id)
        {
            var isDeleted = _departmentService.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }

    }
}
