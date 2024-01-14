using LiquorShop.Areas.Admin.Models;
using LiquorShop.Models;
using LiquorShop.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Iyzipay.Model;
using Iyzipay.Request;
using Iyzipay;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace LiquorShop.Controllers
{
	[Authorize]
	public class CartController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<IdentityUser> _userManager;
		[BindProperty]
		public ShoppingCartVM ShoppingCartVM { get; set; }
		[BindProperty]
		public OrderDetailVM OrderVM { get; set; }
		public CartController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}

		public IActionResult ShoppingBag()
		{
		

			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM = new ShoppingCartVM()
			{
				OrderHeader = new OrderHeader(),
				ListCart = _context.ShoppingCart.Where(i => i.AppUserId == claim.Value).Include(i => i.Product)
			};

			ShoppingCartVM.OrderHeader.TotalPrice = 0;
			ShoppingCartVM.OrderHeader.AppUser = _context.AppUser.FirstOrDefault(i => i.Id == claim.Value);

			foreach (var item in ShoppingCartVM.ListCart)
			{

					ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
				
			}
			return View(ShoppingCartVM);
		}
		public IActionResult Summary()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM = new ShoppingCartVM()
			{
				OrderHeader = new OrderHeader(),
				ListCart = _context.ShoppingCart.Where(i => i.AppUserId == claim.Value).Include(i => i.Product)
			};
			foreach (var item in ShoppingCartVM.ListCart)
			{
				item.Price = item.Product.Price;
				ShoppingCartVM.OrderHeader.TotalPrice += (item.Count * item.Product.Price);
			}
			return View(ShoppingCartVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Summary(ShoppingCartVM model)
		{

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			ShoppingCartVM.ListCart = _context.ShoppingCart.Where(i => i.AppUserId == claim.Value).Include(i => i.Product);
			ShoppingCartVM.OrderHeader.OrderStatus = Other.Pending;
			ShoppingCartVM.OrderHeader.AppUserId = claim.Value;
			ShoppingCartVM.OrderHeader.OrderTime = DateTime.Now;
			_context.OrderHeader.Add(ShoppingCartVM.OrderHeader);
			_context.SaveChanges();

			foreach (var item in ShoppingCartVM.ListCart)
			{
				item.Price = item.Product.Price;
				OrderDetail orderDetail = new OrderDetail()
				{
					ProductId = item.ProductId,
					OrderId = ShoppingCartVM.OrderHeader.Id,
					Price = item.Price,
					Count = item.Count,
				};
				ShoppingCartVM.OrderHeader.TotalPrice += item.Count * item.Product.Price;
				model.OrderHeader.TotalPrice += item.Count * item.Product.Price;
				_context.Add(orderDetail);

			}
			var payment = PaymentProcess(model);
			_context.ShoppingCart.RemoveRange(ShoppingCartVM.ListCart);

			_context.SaveChanges();
			HttpContext.Session.SetInt32(Other.ShoppingCartSs, 0);

            return RedirectToAction("OrderSent");
		}

		private Payment PaymentProcess(ShoppingCartVM model)
		{
			Options options = new Options();
			options.ApiKey = "sandbox-zWp97DLzFk5XqvBxLHreAGT2Xpzi7pJk";
			options.SecretKey = "sandbox-ewIInSzjPRQJFKmadxPwHAkCT4yksWgT";
			options.BaseUrl = "https://sandbox-api.iyzipay.com";

			CreatePaymentRequest request = new CreatePaymentRequest();
			request.Locale = Locale.TR.ToString();
			request.ConversationId = new Random().Next(1111,9999).ToString();
			request.Price = model.OrderHeader.TotalPrice.ToString();
			request.PaidPrice = model.OrderHeader.TotalPrice.ToString();
            request.Currency = Currency.TRY.ToString();
			request.Installment = 1;
			request.BasketId = "B67832";
			request.PaymentChannel = PaymentChannel.WEB.ToString();
			request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = model.OrderHeader.CardName;
            paymentCard.CardNumber = model.OrderHeader.CardNo;
            paymentCard.ExpireMonth = model.OrderHeader.ExpirationMonth;
            paymentCard.ExpireYear = model.OrderHeader.ExpirationYear;
            paymentCard.Cvc = model.OrderHeader.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
			buyer.Id = model.OrderHeader.Id.ToString();
			buyer.Name = model.OrderHeader.Name;
			buyer.Surname = model.OrderHeader.Surname;
			buyer.GsmNumber = model.OrderHeader.PhoneNumber;
			buyer.Email = "email@email.com";
			buyer.IdentityNumber = "74300864791";
			buyer.LastLoginDate = "2015-10-05 12:43:35";
			buyer.RegistrationDate = "2013-04-21 15:12:09";
			buyer.RegistrationAddress = model.OrderHeader.Adress;
			buyer.Ip = "85.34.78.112";
			buyer.City = model.OrderHeader.City;
			buyer.Country ="Turkey";
			buyer.ZipCode = model.OrderHeader.ZipCode;
			request.Buyer = buyer;

			

			List<BasketItem> basketItems = new List<BasketItem>();
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

			foreach (var item in _context.ShoppingCart.Where(i=>i.AppUserId==claim.Value).Include(i=>i.Product))
			{
				basketItems.Add(new BasketItem() {
					Id = item.Id.ToString(),
					Name = item.Product.Name,
					Category1 =item.Product.CategoryId.ToString(),
					ItemType=BasketItemType.PHYSICAL.ToString(),
					Price=(item.Price*item.Count).ToString()

				});
			}
            request.BasketItems = basketItems;

			return Payment.Create(request, options);

		}

		public IActionResult OrderSent()
		{
			return View();
		}
	
		[Route("cart/deletecart/{id}")]
		public IActionResult DeleteCart(int id)
		{
			_context.ShoppingCart.Remove(
					_context.ShoppingCart.Find(id)
				);
			_context.SaveChanges();
			return View();

		}
		
		[HttpPost]
		[ActionName("Increase")]
		public IActionResult Increase(int cartId)
		{
			var cart = _context.ShoppingCart.FirstOrDefault(i => i.Id == cartId);

			if (cart != null)
			{
				cart.Count += 1;
				_context.SaveChanges();

				// Burada AJAX isteği ile başarılı bir artırma işlemi gerçekleştiğini belirten bir JSON cevabı döndürüyoruz.
				return Json(new { success = true, updatedCount = cart.Count, updatedTotalPrice = CalculateTotalPrice() });
			}

			// Eğer cart bulunamazsa veya bir hata oluşursa hata durumunu döndürebilirsiniz.
			return Json(new { success = false, error = "Ürün bulunamadı veya bir hata oluştu." });
		}
		[HttpPost]
		[ActionName("Decrease")]
		public IActionResult Decrease(int cartId)
		{
			var cart = _context.ShoppingCart.FirstOrDefault(i => i.Id == cartId);

			if (cart != null && cart.Count > 0)
			{
				cart.Count -= 1;
				_context.SaveChanges();

			return Json(new { success = true, updatedCount = cart.Count, updatedTotalPrice = CalculateTotalPrice() });
			}

			return Json(new { success = false, error = "Ürün bulunamadı veya ürün adeti sıfıra ulaştı." });
		}

		private double CalculateTotalPrice()
		{
			
			 var totalPrice = _context.ShoppingCart.Sum(item => item.Count * item.Product.Price);
			 return totalPrice;
		}
		public IActionResult MyOrders()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			IEnumerable<OrderHeader> orderHeadersList;
			orderHeadersList = _context.OrderHeader.Where(i => i.AppUserId == claim.Value).Include(i => i.AppUser);
			return View(orderHeadersList);
		}
		public IActionResult MyOrderDetail(int id)
		{
			IEnumerable<OrderHeader> orderHeadersList;
			OrderVM = new OrderDetailVM
			{
				OrderHeader = _context.OrderHeader.FirstOrDefault(i => i.Id == id),
				OrderDetail = _context.OrderDetail.Where(x => x.OrderId == id).Include(x => x.Product)

			};

			return View(OrderVM);
		}

	}
}
