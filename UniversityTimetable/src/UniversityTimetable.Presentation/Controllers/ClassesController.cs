using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.Models;

namespace UniversityTimetable.Presentation.Controllers
{
    public class ClassesController : Controller
    {
        public ClassesController()
        {
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View();
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassType,IsDeleted,SubjectName,TeacherId,GroupId,AuditoryId")] Class @class)
        {
            return View();
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassType,IsDeleted,SubjectName,TeacherId,GroupId,AuditoryId")] Class @class)
        {
            return View();
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
