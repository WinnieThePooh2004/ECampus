using Microsoft.AspNetCore.Mvc;
using UniversityTimetable.Shared.DataTransferObjects;

namespace UniversityTimetable.Presentation.Controllers
{
    public class GroupDTOesController : Controller
    {
        public GroupDTOesController()
        {
        }

        // GET: GroupDTOes
        public async Task<IActionResult> Index()
        {
            return View();
        }

        // GET: GroupDTOes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            return View();
        }

        // GET: GroupDTOes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: GroupDTOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DepartmentName,DepartmentId")] GroupDTO groupDTO)
        {
            return View();
        }

        // GET: GroupDTOes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return View();
        }

        // POST: GroupDTOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DepartmentName,DepartmentId")] GroupDTO groupDTO)
        {
            return View(groupDTO);
        }

        // GET: GroupDTOes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            return View();
        }

        // POST: GroupDTOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
