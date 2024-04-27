using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using T2_Videoteca_RevillaFernandezHectorRolando.Data;
using T2_Videoteca_RevillaFernandezHectorRolando.Models;

namespace T2_Videoteca_RevillaFernandezHectorRolando.Controllers
{
    public class VideosController : Controller
    {
        private readonly T2_Videoteca_RevillaFernandezHectorRolandoContext _context;

        public VideosController(T2_Videoteca_RevillaFernandezHectorRolandoContext context)
        {
            _context = context;
        }

        // GET: Videos
        public async Task<IActionResult> Index(string searchString,
            DateTime? date,
            decimal? rating,
            string currentFilterName,
            int? currentFilterYear,
            DateTime? currentFilterDate,
            decimal? currentRating,
            string sortOrder,
            int? pageNumber)
        {
            if (_context.Video == null)
            {
                return Problem("Entidad MiVideoteca es null");
            }
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            ViewData["RatingSortParm"] = sortOrder == "Rating" ? "rating_desc" : "Rating";

            if (searchString != null || date != null || (rating != null && rating != 0))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilterName;
                date = currentFilterDate;
                rating = currentRating;
            }

            ViewData["CurrentFilterName"] = searchString;
            ViewData["CurrentFilterDate"] = date;
            ViewData["CurrentFilterRating"] = rating;

            var video = from m in _context.Video
                        select m;

            switch (sortOrder)
            {
                case "name_desc":
                    video = video.OrderByDescending(s => s.nombre);
                    break;
                case "Date":
                    video = video.OrderBy(s => s.fecha);
                    break;
                case "date_desc":
                    video = video.OrderByDescending(s => s.fecha);
                    break;
                case "Rating":
                    video = video.OrderBy(s => s.rating);
                    break;
                case "rating_desc":
                    video=video.OrderByDescending(s => s.rating); 
                    break;
                default:
                    video = video.OrderBy(s => s.nombre);
                    break;
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                video = video.Where(s => s.nombre!.Contains(searchString));
            }
            if (date != null)
            {
                video = video.Where(s => s.fecha.Year == date.Value.Year 
                && s.fecha.Month==date.Value.Month
                && s.fecha.Day==date.Value.Day);
            }
            if (rating != null && rating != 0)
            {
                video = video.Where(s => s.rating == rating);
            }

            int pageSize = 4;

            // return View(await video.ToListAsync());
            return View(await PaginatedList<Video>.CreateAsync(video.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        // GET: Videos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .FirstOrDefaultAsync(m => m.id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // GET: Videos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,nombre,tipo,veces_vista,rating,fecha")] Video video)
        {
            if (ModelState.IsValid)
            {
                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        // GET: Videos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,nombre,tipo,veces_vista,rating,fecha")] Video video)
        {
            if (id != video.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoExists(video.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        // GET: Videos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var video = await _context.Video
                .FirstOrDefaultAsync(m => m.id == id);
            if (video == null)
            {
                return NotFound();
            }

            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var video = await _context.Video.FindAsync(id);
            if (video != null)
            {
                _context.Video.Remove(video);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.id == id);
        }
    }
}
