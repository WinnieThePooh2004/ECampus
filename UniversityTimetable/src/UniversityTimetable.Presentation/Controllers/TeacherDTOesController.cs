using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Presentation.Controllers
{
    public class TeacherDTOesController : Controller
    {
        public TeacherDTOesController()
        {
        }

        // GET: TeacherDTOes
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: TeacherDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View();
        }

        // GET: TeacherDTOes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TeacherDTOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,ScienceDegree,DepartmentId,DepartmentName")] TeacherDTO teacherDTO)
        {
            return View(teacherDTO);
        }

        // GET: TeacherDTOes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }

        // POST: TeacherDTOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,ScienceDegree,DepartmentName")] TeacherDTO teacherDTO)
        {
            return View(teacherDTO);
        }

        // GET: TeacherDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }

        // POST: TeacherDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
