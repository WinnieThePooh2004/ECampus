using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces.Services;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters.TimetableParameters;

namespace UniversityTimetable.Presentation.Controllers
{
    public class ClassesController : Controller
    {

        private readonly IClassService _service;

        public ClassesController(IClassService service)
        {
            _service = service;
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        public async Task<IActionResult> GroupTimetable([FromQuery] GroupTimetableParameters parameters)
        {
            var table = await _service.GetTimetableForGroupAsync(parameters);
            return View(table);
        }

        // GET: Classes/Create
        public IActionResult Create([FromQuery] CreateQueryParameters parameters)
        {
            var @class = new ClassDTO
            {
                GroupId = parameters.GroupId,
                AuditoryId = parameters.AuditoryId,
                TeacherId = parameters.TeacherId,
                Number = parameters.Number,
                DayOfTheWeek = parameters.DayOfWeek
            };
            return View(@class);
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClassType,SubjectName,TeacherId,GroupId,AuditoryId,Number,DayOfTheWeek,WeekDependency")] ClassDTO @class)
        {
            if(!ModelState.IsValid)
            {
                return View(@class);
            }
            await _service.CreateAsync(@class);
            return RedirectToAction(nameof(GroupTimetable), new GroupTimetableParameters { GroupId = @class.GroupId });
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("Id,ClassType,SubjectName,TeacherId,Number,DayOfTheWeek,GroupId,AuditoryId,WeekDependency")] ClassDTO @class)
        {
            if (!ModelState.IsValid)
            {
                return View(@class);
            }
            await _service.UpdateAsync(@class);
            return RedirectToAction(nameof(GroupTimetable), new GroupTimetableParameters { GroupId = @class.GroupId });
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
