using LiquorShop.Areas.Admin.Models;
using LiquorShop.Areas.Identity.Pages.Account;
using LiquorShop.Data;
using LiquorShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;
using X.PagedList;

namespace LiquorShop.Controllers
{
	public class HomeController : Controller
	{
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //	_logger = logger;
        //}

        public IActionResult Index()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			if (claim!=null)
			{
				var count = _context.ShoppingCart.Where(i => i.AppUserId == claim.Value).ToList().Count();
				HttpContext.Session.SetInt32(Other.ShoppingCartSs, count);
			}
			return View();
		}

		[Route(template: "hakkinda")]
		public IActionResult About()
		{
			return View();
		}

		[Route(template: "iletisim")]
		public IActionResult Communication()
		{
			
			return View();
		}

		[HttpPost]
        [Route(template: "iletisim")]
        public IActionResult Communication(Contact contact)
		{
			if (ModelState.IsValid)
			{
				contact.CreatedOn = DateTime.Now;
				
				_context.Add(contact);
				_context.SaveChanges();
				ViewBag.Msg = "Mesajınıza en kısa zamanda dönüş sağlanacaktır";
				return View("Msg");

			}

			ViewBag.Msg = "Mesaj Gönderme isteğiniz başarısız oldu. Lütfen bir süre sonra tekrar deneyin.";
			return View("Msg");
		}

		[Route(template: "blog")]
        public IActionResult Blog(int page=1)
        {
          
            return View(_context.Blogs.OrderByDescending(Blog => Blog.CreatedOn).ToList()/*.ToPagedList(page, 3)*/);

        }
        [Route("{slug}")]
        public IActionResult BlogPost(string slug)
        {
            var post = _context
                .Blogs
                .Where(blog => blog.Slug == slug)
                .FirstOrDefault();


            if (post != null)
            {
                return View(post);
            }

            Response.StatusCode = 404;
            return View("PageNotFound");
        }
        [Route(template: "urunler")]

        public IActionResult Product()
        {

            return View(_context.Products.OrderByDescending(product => product.CategoryId).ToList());

        }

		public IActionResult ProductPost(int id)
		{
			var product = _context.Products
				.FirstOrDefault(product =>product.Id == id);

			ShoppingCart cart = new ShoppingCart(){
				Product = product,
				ProductId = product.Id

			};

			return View(cart);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public IActionResult ProductPost(ShoppingCart shoppingCart)
		{
			shoppingCart.Id = 0;
			if (ModelState.IsValid)
			{
				var claimsIdentity = (ClaimsIdentity)User.Identity;
				var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
				shoppingCart.AppUserId = claim.Value;
				ShoppingCart cart = _context.ShoppingCart.FirstOrDefault(
					u => u.AppUserId == shoppingCart.AppUserId && u.ProductId == shoppingCart.ProductId);
				if (cart==null)
				{
					_context.ShoppingCart.Add(shoppingCart);
				}
				else
				{
					cart.Count += shoppingCart.Count;
				}
				_context.SaveChanges();
				return RedirectToAction(nameof(Index));
			}
			else
			{
				var product = _context.Products
				.FirstOrDefault(i => i.Id == shoppingCart.Id);

				ShoppingCart cart = new ShoppingCart()
				{
					Product = product,
					ProductId = product.Id
				};

			}
			

			return View(shoppingCart);
		}
		
		
		public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}