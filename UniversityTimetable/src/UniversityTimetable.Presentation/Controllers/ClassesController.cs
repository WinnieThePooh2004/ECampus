﻿using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Presentation.Models;
using UniversityTimetable.Shared.DataTransferObjects;
using UniversityTimetable.Shared.Interfaces;
using UniversityTimetable.Shared.Models;
using UniversityTimetable.Shared.QueryParameters;

namespace UniversityTimetable.Presentation.Controllers
{
    public class ClassesController : Controller
    {

        private readonly IService<ClassDTO, ClassParameters> _service;

        public ClassesController(IService<ClassDTO, ClassParameters> service)
        {
            _service = service;
        }

        // GET: Classes
        public async Task<IActionResult> Index([FromQuery] ClassParameters parameters)
        {
            return View(IndexModel.Create(await _service.GetByParametersAsync(parameters), parameters));

        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View(await _service.GetByIdAsync(id));
        }

        // GET: Classes/Create
        public IActionResult Create([FromQuery] int teacherId, [FromQuery] int groupId, [FromQuery] int auditoryId)
        {
            var @class = new ClassDTO{ GroupId = groupId, AuditoryId = auditoryId, TeacherId = teacherId };
            return View(@class);
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassType,SubjectName,TeacherId,GroupId,AuditoryId")] ClassDTO @class)
        {
            if(!ModelState.IsValid)
            {
                return View(@class);
            }
            await _service.CreateAsync(@class);
            return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassType,SubjectName,TeacherId,GroupId,AuditoryId")] ClassDTO @class)
        {
            if (!ModelState.IsValid)
            {
                return View(@class);
            }
            await _service.UpdateAsync(@class);
            return RedirectToAction(nameof(Index));
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
