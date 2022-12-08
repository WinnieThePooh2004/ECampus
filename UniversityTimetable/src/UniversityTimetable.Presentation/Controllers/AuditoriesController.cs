using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Api.Models;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Api.Controllers
{
    public class AuditoriesController : Controller
    {
        private readonly IService<AuditoryDTO, AuditoryParameters> _service;

        public AuditoriesController(IService<AuditoryDTO, AuditoryParameters> service)
        {
            _service = service;
        }

        // GET: Auditories
        public async Task<IActionResult> Index(AuditoryParameters parameters)
        {
            return View(IndexModel.Create(await _service.GetByParametersAsync(parameters), parameters));
        }

        // GET: Auditories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // GET: Auditories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Auditories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Building")] AuditoryDTO auditory)
        {
            if (!ModelState.IsValid)
            {
                return View(auditory);
            }
            await _service.CreateAsync(auditory);
            return RedirectToAction(nameof(Index));
        }

        // GET: Auditories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Auditories/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name,Building")] AuditoryDTO auditory)
        {
            if (!ModelState.IsValid)
            {
                return View(auditory);
            }
            await _service.UpdateAsync(auditory);
            return RedirectToAction(nameof(Index));
        }

        // GET: Auditories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Auditories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
