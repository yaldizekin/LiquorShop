using LiquorShop.Areas.Admin.Models;
using LiquorShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace LiquorShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Other.Role_Admin)]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public OrderDetailVM OrderVM { get; set; }
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeadersList;
            orderHeadersList = _context.OrderHeader.ToList();
            return View(orderHeadersList);
        }
        public IActionResult Pending()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeadersList;
            orderHeadersList = _context.OrderHeader.Where(i=>i.OrderStatus==Other.Pending);
            return View(orderHeadersList);
        }
        public IActionResult Approved()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeadersList;
            orderHeadersList = _context.OrderHeader.Where(i => i.OrderStatus == Other.Approved);
            return View(orderHeadersList);
        }
        public IActionResult Shipped()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            IEnumerable<OrderHeader> orderHeadersList;
            orderHeadersList = _context.OrderHeader.Where(i => i.OrderStatus == Other.Shipped);
            return View(orderHeadersList);
        }
        public IActionResult Details(int id)
        {
            IEnumerable<OrderHeader> orderHeadersList;
            OrderVM = new OrderDetailVM
            {
                OrderHeader= _context.OrderHeader.FirstOrDefault(i=>i.Id==id),
                OrderDetail = _context.OrderDetail.Where(x => x.OrderId == id).Include(x => x.Product)
                
            };
            
            return View(OrderVM);
        }
        [Route("admin/order/delete/{id}")]
        public IActionResult Delete(int id)
        {
            _context.OrderHeader.Remove(
                    _context.OrderHeader.Find(id)
                );
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }
    }
}
