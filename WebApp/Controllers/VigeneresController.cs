using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Models;
using WebApp.Roles;
using WebApp.ViewModels;

namespace WebApp.Controllers
{
    [Authorize]
    
    public class VigeneresController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public VigeneresController(ApplicationDbContext context, IServiceProvider serviceProvider, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        // GET: Vigeneres
        // See data only for the current user
        public async Task<IActionResult> Index()
        {
            CreateRoles.CreateVigenereAdmin(_serviceProvider, _userManager, _roleManager).Wait();

            
            
            var res = await _context
                .Vigeneres
                .Where(v => v.AppUserId == GetUserId())
                .Select(v => new VigenereIndexViewModel()
                {
                    Id = v.Id,
                    Key = v.Key,
                    EncryptedText = v.EncryptedText,
                })
                .ToListAsync();
            
            return View(res);
        }

        
        // GET: Vigeneres/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
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

            var vigenereVM = new VigenereIndexViewModel
            {
                Id = vigenere.Id,
                Key = vigenere.Key,
                EncryptedText = vigenere.EncryptedText,
            };
            
            return View(vigenereVM);
        }

        // GET: Vigeneres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vigeneres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VigenereCreateViewModel vigenereVM)
        {
            // cannot use anymore direct models
            if (ModelState.IsValid)
            {
                var vigenere = new Vigenere()
                {
                    Key = vigenereVM.Key,
                    EncryptedText = vigenereVM.EncryptedText,
                    AppUserId = GetUserId(),

                };
                _context.Add(vigenere);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vigenereVM);
        }

        
        // Gets user id
        public string GetUserId()
        {
            return User.Claims.First(v => v.Type == ClaimTypes.NameIdentifier).Value;
        }
        
        
        
        // GET: Vigeneres/Edit/5
        [Authorize(Roles = Roles.Roles.AdminVigenere)]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vigenere = await _context.Vigeneres.FindAsync(id);
            if (vigenere == null)
            {
                return NotFound();
            }
            
            var vigenereVM = new VigenereIndexViewModel
            {
                Id = vigenere.Id,
                Key = vigenere.Key,
                EncryptedText = vigenere.EncryptedText,
            };
            
            return View(vigenereVM);
        }

        
        // POST: Vigeneres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = Roles.Roles.AdminVigenere)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, VigenereIndexViewModel vigenereVM)
        {
            if (id != vigenereVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var vigenere = await _context.Vigeneres.FindAsync(id);
                    if (vigenere == null)
                    {
                        return NotFound();
                    }
                    vigenere.Key = vigenereVM.Key;
                    vigenere.EncryptedText = vigenereVM.EncryptedText;
                    _context.Update(vigenere);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));              
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VigenereExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(vigenereVM);
        }

        
        // GET: Vigeneres/Delete/5
        [Authorize(Roles = Roles.Roles.AdminVigenere)]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
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

            var vigenereVM = new VigenereIndexViewModel
            {
                Id = vigenere.Id,
                Key = vigenere.Key,
                EncryptedText = vigenere.EncryptedText,
            };
            
            return View(vigenereVM);
        }

        
        // POST: Vigeneres/Delete/5
        [Authorize(Roles = Roles.Roles.AdminVigenere)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

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
