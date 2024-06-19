using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = Roles.Roles.AdminCaesar)]
    public class AdminCaesarsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminCaesarsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminCaesars
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Caesars.Include(c => c.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminCaesars/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Caesars == null)
            {
                return NotFound();
            }

            var caesar = await _context.Caesars
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caesar == null)
            {
                return NotFound();
            }

            return View(caesar);
        }

        // GET: AdminCaesars/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AdminCaesars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Key,EncryptedText,AppUserId")] Caesar caesar)
        {
            if (ModelState.IsValid)
            {
                caesar.Id = Guid.NewGuid();
                _context.Add(caesar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", caesar.AppUserId);
            return View(caesar);
        }

        // GET: AdminCaesars/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Caesars == null)
            {
                return NotFound();
            }

            var caesar = await _context.Caesars.FindAsync(id);
            if (caesar == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", caesar.AppUserId);
            return View(caesar);
        }

        // POST: AdminCaesars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Key,EncryptedText,AppUserId")] Caesar caesar)
        {
            if (id != caesar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(caesar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaesarExists(caesar.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", caesar.AppUserId);
            return View(caesar);
        }

        // GET: AdminCaesars/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Caesars == null)
            {
                return NotFound();
            }

            var caesar = await _context.Caesars
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (caesar == null)
            {
                return NotFound();
            }

            return View(caesar);
        }

        // POST: AdminCaesars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Caesars == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Caesars'  is null.");
            }
            var caesar = await _context.Caesars.FindAsync(id);
            if (caesar != null)
            {
                _context.Caesars.Remove(caesar);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CaesarExists(Guid id)
        {
          return (_context.Caesars?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
