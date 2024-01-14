using LiquorShop.Data;
using LiquorShop.Areas.Admin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Slugify;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Authorization;
using SendGrid.Helpers.Mail;

namespace LiquorShop.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = Other.Role_Admin)]

	public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        
        public HomeController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("admin")]
        public IActionResult Index()
        {


            return View(_context.Contacts.OrderByDescending(message => message.CreatedOn).ToList());
        }
        public IActionResult Blog()
        {


            return View(_context.Blogs.OrderByDescending(post => post.CreatedOn).ToList());
        }
        public IActionResult Categories()
        {


            return View(_context.Categories.OrderByDescending(cat => cat.Id).ToList());
        }
        public IActionResult Products()
        {


            return View(_context.Products.OrderByDescending(product => product.Id).ToList());
        }



        public IActionResult AddBlog()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddBlog(Blog blog)
        {
            if (CheckIfSlugExists(blog.Slug))
            {
                ViewBag.Msg = "Aynı slug olduğu için ekleme yapılmadı";
                return View("Msg");
            }
            if (blog.Image != null)
            {
                var extension = Path.GetExtension(blog.Image.FileName);
                var newimagename = Guid.NewGuid() + extension;
                var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "backup", "images", newimagename);
                var stream = new FileStream(location, FileMode.Create);

                //b.Image.CopyTo(stream);
                using (var image = Image.Load(blog.Image.OpenReadStream()))
                {
                    var newWidth = 800;

                    image.Mutate(x => x
                        .Resize(new ResizeOptions
                        {
                            Size = new Size(newWidth /*newHeight*/),
                            Mode = ResizeMode.Max // Bu boyutlandırma modunu isteğinize göre değiştirin
                        }));

                    image.Save(stream, new JpegEncoder()); // Çıktıyı stream'e kaydet, formatı ayarlayabilirsiniz
                }
                blog.ImagePath = newimagename;

            }
            _context.Add(blog);
            _context.SaveChanges();

            ViewBag.Msg = "İçerik Eklendi";
            return View("Msg");
        }

        public IActionResult EditBlog(int id)
        {

            var blog = _context.Blogs.SingleOrDefault(i => i.Id == id);


            return View(blog);
        }

        [HttpPost]
        public IActionResult EditBlog(Blog e)
        {

            var existingBlog = _context.Blogs.SingleOrDefault(i => i.Id == e.Id);


           

            existingBlog.Title = e.Title;
            existingBlog.Summary = e.Summary;
            existingBlog.Content = e.Content;
            existingBlog.Slug = existingBlog.Slug;
            existingBlog.CreatedOn = DateTime.Now;

            _context.Update(existingBlog);
            _context.SaveChanges();
            ViewBag.Msg = "İçerik Güncellendi";
            return View("Msg");



        }


        [HttpPost]
        public IActionResult GenerateSlug(string title)
        {
            SlugHelper.Config config = new SlugHelper.Config();
            config.CharacterReplacements.Add("ı", "i");
            SlugHelper helper = new SlugHelper(config);

            string slug = helper.GenerateSlug(title);

            return Content(slug);
        }

        public bool CheckIfSlugExists(string slug)
        {
            if (_context.Blogs.Any(p => p.Slug == slug))
            {
                return true;
            }

            return false;
        }

        [HttpPost]
        public IActionResult CheckSlugExists(string slug)
        {

            if (CheckIfSlugExists(slug))
            {
                return Json(
                   new
                   {
                       exists = true
                   }
                );
            }

            return Json(
                   new
                   {
                       exists = false
                   }
                );
        }

        [Route("admin/deleteblog/{id}")]
        public IActionResult DeleteBlog(int id)
        {
            _context.Blogs.Remove(
                    _context.Blogs.Find(id)
                );
            _context.SaveChanges();


            ViewBag.Msg = "İçerik Silindi";
            return View("Msg");
        }

        public IActionResult Messages(int id)
        {
            var contact = _context.Contacts.SingleOrDefault(i => i.Id == id);
            return View(contact);
           
        }

        [Route("admin/deletemsg/{id}")]
        public IActionResult DeleteMsg(int id)
        {
            _context.Contacts.Remove(
                    _context.Contacts.Find(id)
                );
            _context.SaveChanges();


            ViewBag.Msg = "İleti Silindi";

            return View("Msg");
        }
    }
  
}
