using Microsoft.AspNetCore.Mvc;
using NuGet.DependencyResolver;
using UniversityTimetable.Presentation.Models;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Presentation.Controllers
{
    public class TeachersController : Controller
    {

        private readonly IService<TeacherDTO, TeacherParameters> _service;

        public TeachersController(IService<TeacherDTO, TeacherParameters> service)
        {
            _service = service;
        }

        // GET: Teachers
        public async Task<IActionResult> Index([FromQuery] TeacherParameters parameters)
        {
            return View(IndexModel.Create(await _service.GetByParametersAsync(parameters), parameters));
        }

        // GET: Teachers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // GET: Teachers/Create
        public IActionResult Create([FromQuery] int depatmentId)
        {
            var teacher = new TeacherDTO { DepartmentId = depatmentId };
            return View(teacher);
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,ScienceDegree,DepartmentId")] TeacherDTO teacher)
        {
            if(!ModelState.IsValid)
            {
                return View(teacher);
            }
            await _service.CreateAsync(teacher);
            return RedirectToAction(nameof(Index), new { departmentId = teacher.DepartmentId });
        }

        // GET: Teachers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,FirstName,LastName,ScienceDegree,DepartmentName")] TeacherDTO teacher)
        {
            if (!ModelState.IsValid)
            {
                return View(teacher);
            }
            await _service.UpdateAsync(teacher);
            return RedirectToAction(nameof(Index), new { departmentId = teacher.DepartmentId });
        }

        // GET: Teachers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id, [FromQuery] int departmentId)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { departmentId = departmentId });
        }
    }
}
