using LiquorShop.Areas.Admin.Models;
using LiquorShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LiquorShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Other.Role_Admin)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
    
        public IActionResult Index()
        {
            var users = _context.AppUser.ToList();
            var role=_context.Roles.ToList();
            var userRoles=_context.UserRoles.ToList();
            foreach (var item in users)
            {
                var roleId = userRoles.FirstOrDefault(i => i.UserId == item.Id).RoleId;
                item.Role = role.FirstOrDefault(u => u.Id == roleId).Name;

            }
            return View(users);
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AppUser == null)
            {
                return NotFound();
            }

            var user = await _context.AppUser
                .FirstOrDefaultAsync(m => m.Id == id.ToString());
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AppUser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.AppUser'  is null.");
            }
            var user = await _context.AppUser.FindAsync(id);
            if (user != null)
            {
                _context.AppUser.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
