using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Conciliator.App.Models;
using Conciliator.App.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Conciliator.App.Controllers
{
    public class ExtractsController : Controller
    {
        private readonly IExtractRepository _extractRepository;
        private readonly IExtractService _extractService;

        public ExtractsController(IExtractRepository extractRepository,
                                  IExtractService extractService)
        {
            _extractRepository = extractRepository;
            _extractService = extractService;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                var lastExtract = await _extractRepository.GetLastExtract();
                startDate = lastExtract != null ? lastExtract.DatePosted : new DateTime(2014, 1, 1);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now;
            }

            ViewData["startDate"] = startDate.Value.ToString("yyyy-MM-dd");
            ViewData["endDate"] = endDate.Value.ToString("yyyy-MM-dd");

            var result = await _extractRepository.SearchByDates(startDate, endDate);

            return View(result);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null) return NotFound();

            var Extract = await _extractRepository.GetById(id);

            if (Extract == null) return NotFound();

            return View(Extract);
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            if (files == null || size < 0)
            {
                return NotFound();
            }

            await _extractService.ProcessConciliation(files);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null) return NotFound();

            var Extract = await _extractRepository.GetById(id);

            if (Extract == null) return NotFound();

            return View(Extract);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, Extract extract)
        {
            if (id != extract.Id) return NotFound();

            if (!ModelState.IsValid) return View(extract);

            try
            {
                await _extractRepository.Update(extract);
                await _extractRepository.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_extractRepository.GetById(extract.Id) == null)
                {
                    return NotFound();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null) return NotFound();

            var Extract = await _extractRepository.GetById(id);

            if (Extract == null) return NotFound();

            return View(Extract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _extractRepository.Remove(id);
            await _extractRepository.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [Route("error/{id:length(3,3)}")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
