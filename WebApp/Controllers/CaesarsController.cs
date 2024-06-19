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
    public class CaesarsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        
        public CaesarsController(ApplicationDbContext context, IServiceProvider serviceProvider, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _roleManager = roleManager;
            _userManager = userManager;
            
        }

        
        // GET: Caesars
        // See data only for the current user
        public async Task<IActionResult> Index()
        {
            CreateRoles.CreateCaesarAdmin(_serviceProvider, _userManager, _roleManager).Wait();

            
            
            var res = await _context
                .Caesars
                .Where(c => c.AppUserId == GetUserId())
                .Select(c => new CaesarIndexViewModel()
                {
                    Id = c.Id,
                    Key = c.Key,
                    EncryptedText = c.EncryptedText, 
                })
                .ToListAsync();
                
            return View(res);
        }

        
        // GET: Caesars/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
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
            
            var caesarVM = new CaesarIndexViewModel
            {
                Id = caesar.Id,
                Key = caesar.Key,
                EncryptedText = caesar.EncryptedText,
            };

            return View(caesarVM);
        }

        
        // GET: Caesars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Caesars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaesarCreateViewModel caesarVM)
        {
            // now we cannot use anymore direct models
            if (ModelState.IsValid)
            {
                var caesar = new Caesar()
                {
                    Key = caesarVM.Key,
                    EncryptedText = caesarVM.EncryptedText,
                    AppUserId = GetUserId(),
                };
                _context.Add(caesar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(caesarVM);
        }

        // Gets user id
        public string GetUserId()
        {
            return User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
        }
        
        
        [Authorize(Roles = Roles.Roles.AdminCaesar)]
        // GET: Caesars/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caesar = await _context.Caesars.FindAsync(id);
            if (caesar == null)
            {
                return NotFound();
            }

            var caesarVM = new CaesarIndexViewModel
            {
                Id = caesar.Id,
                Key = caesar.Key,
                EncryptedText = caesar.EncryptedText,
            };
            
            return View(caesarVM);
        }

        [Authorize(Roles = Roles.Roles.AdminCaesar)]
        // POST: Caesars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CaesarIndexViewModel caesarVM)
        {
            if (id != caesarVM.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var caesar = await _context.Caesars.FindAsync(id);
                    if (caesar == null)
                    {
                        return NotFound();
                    }
                    caesar.Key = caesarVM.Key;
                    caesar.EncryptedText = caesarVM.EncryptedText;
                    _context.Update(caesar);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaesarExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(caesarVM);
        }

        [Authorize(Roles = Roles.Roles.AdminCaesar)]
        // GET: Caesars/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
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

            var caesarVM = new CaesarIndexViewModel
            {
                Id = caesar.Id,
                Key = caesar.Key,
                EncryptedText = caesar.EncryptedText,
            };
            
            return View(caesarVM);
        }

        [Authorize(Roles = Roles.Roles.AdminCaesar)]
        // POST: Caesars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
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
