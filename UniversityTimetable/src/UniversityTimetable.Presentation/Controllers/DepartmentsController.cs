using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Api.Models;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Api.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IService<DepartmentDTO, DepartmentParameters> _service;
        public DepartmentsController(IService<DepartmentDTO, DepartmentParameters> service)
        {
            _service = service;
        }

        // GET: Departments
        public async Task<IActionResult> Index(DepartmentParameters parameters)
        {
            return View(IndexModel.Create(await _service.GetByParametersAsync(parameters), parameters));
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // GET: Departments/Create
        public IActionResult Create([FromQuery] int facultacyId)
        {
            var department = new DepartmentDTO{ FacultacyId = facultacyId };
            return View(department);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,IsDeleted,FacultacyId")] DepartmentDTO department)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(department);
            return RedirectToAction("Index", new { FacultacyId = department.FacultacyId });
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,Name,IsDeleted,FacultacyId")] DepartmentDTO department)
        {
            await _service.UpdateAsync(department);
            return RedirectToAction(nameof(Index), new { FacultacyId = department.FacultacyId });
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
