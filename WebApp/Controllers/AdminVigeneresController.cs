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
    [Authorize(Roles = Roles.Roles.AdminVigenere)]
    public class AdminVigeneresController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminVigeneresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AdminVigeneres
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vigeneres.Include(v => v.AppUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AdminVigeneres/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.Vigeneres == null)
            {
                return NotFound();
            }

            var vigenere = await _context.Vigeneres
                .Include(v => v.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vigenere == null)
            {
                return NotFound();
            }

            return View(vigenere);
        }

        // GET: AdminVigeneres/Create
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: AdminVigeneres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Key,EncryptedText,AppUserId")] Vigenere vigenere)
        {
            if (ModelState.IsValid)
            {
                vigenere.Id = Guid.NewGuid();
                _context.Add(vigenere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", vigenere.AppUserId);
            return View(vigenere);
        }

        // GET: AdminVigeneres/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.Vigeneres == null)
            {
                return NotFound();
            }

            var vigenere = await _context.Vigeneres.FindAsync(id);
            if (vigenere == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", vigenere.AppUserId);
            return View(vigenere);
        }

        // POST: AdminVigeneres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Key,EncryptedText,AppUserId")] Vigenere vigenere)
        {
            if (id != vigenere.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vigenere);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VigenereExists(vigenere.Id))
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
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", vigenere.AppUserId);
            return View(vigenere);
        }

        // GET: AdminVigeneres/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.Vigeneres == null)
            {
                return NotFound();
            }

            var vigenere = await _context.Vigeneres
                .Include(v => v.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vigenere == null)
            {
                return NotFound();
            }

            return View(vigenere);
        }

        // POST: AdminVigeneres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.Vigeneres == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vigeneres'  is null.");
            }
            var vigenere = await _context.Vigeneres.FindAsync(id);
            if (vigenere != null)
            {
                _context.Vigeneres.Remove(vigenere);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VigenereExists(Guid id)
        {
          return (_context.Vigeneres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
