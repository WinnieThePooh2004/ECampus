using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Presentation.Models;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Presentation.Controllers
{
    public class GroupsController : Controller
    {
        private readonly IService<GroupDTO, GroupParameters> _service;

        public GroupsController(IService<GroupDTO, GroupParameters> service)
        {
            _service = service;
        }

        // GET: Groups
        public async Task<IActionResult> Index([FromQuery] GroupParameters parameters)
        {
            return View(IndexModel.Create(await _service.GetByParametersAsync(parameters), parameters));
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // GET: Groups/Create
        public IActionResult Create([FromQuery] int departmentId)
        {
            var group = new GroupDTO { DepartmentId = departmentId };
            return View(group);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentName,DepartmentId")] GroupDTO group)
        {
            Console.WriteLine(group.DepartmentId);
            if (!ModelState.IsValid || group.DepartmentId == 0)
            {
                return View(group);
            }
            await _service.CreateAsync(group);
            return RedirectToAction(nameof(Index), new { DepartmentId = group.DepartmentId });
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] GroupDTO group)
        {
            if (!ModelState.IsValid)
            {
                return View(group);
            }
            await _service.CreateAsync(group);
            return RedirectToAction(nameof(Index), new { DepartmentId = group.DepartmentId });
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, [FromQuery] int departmentId)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index), new { DepartmentId = departmentId });
        }
    }
}
