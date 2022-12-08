using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.QueryParameters;
using UniversityTimetable.Presentation.Models;
using UniversityTimetable.Shared.Interfaces.Services;

namespace UniversityTimetable.Presentation.Controllers
{
    public class FacultaciesController : Controller
    {
        private IService<FacultacyDTO, FacultacyParameters> _service;
        public FacultaciesController(IService<FacultacyDTO, FacultacyParameters> service)
        {
            _service = service;
        }

        // GET: Facultacies
        public async Task<IActionResult> Index([FromQuery] FacultacyParameters parameters)
        {
            return View(IndexModel.Create(await _service.GetByParametersAsync(parameters), parameters));
        }

        // GET: Facultacies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // GET: Facultacies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Facultacies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] FacultacyDTO facultacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await _service.CreateAsync(facultacy);
            return RedirectToAction("Index");
        }

        // GET: Facultacies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Facultacies/Edit
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Name")] FacultacyDTO facultacy)
        {
            if (!ModelState.IsValid)
            {
                return View(facultacy);
            }
            await _service.UpdateAsync(facultacy);
            return RedirectToAction("Index");
        }

        // GET: Facultacies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // POST: Facultacies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
