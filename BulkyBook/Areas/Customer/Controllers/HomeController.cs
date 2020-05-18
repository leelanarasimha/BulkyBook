using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BulkyBook.Utility.Extensions;
using BulkyBook.Utility;

namespace BulkyBook.Areas.Customer.Controllers
{
	[Area("Customer")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			var products = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId != null) {
				var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count();
				HttpContext.Session.SetObject(SD.ssShoppingCart, count);
			}
			return View(products);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		public IActionResult Details(int id) {
			var product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id, includeProperties: "Category,CoverType");
			var shoppingcart = new ShoppingCart()
			{
				Product = product,
				ProductId = product.Id
			};
			return View(shoppingcart);
		}

		[HttpPost]
		[Authorize]
		public IActionResult Details(ShoppingCart ShoppingCart) {
			ShoppingCart.Id = 0;
			if (ModelState.IsValid) {
				ShoppingCart.ApplicationUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

				var cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
					u => u.ProductId == ShoppingCart.ProductId && u.ApplicationUserId == ShoppingCart.ApplicationUserId,
					includeProperties: "Product");

				var productFromDb = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == ShoppingCart.ProductId, includeProperties: "Category");


				if (cartFromDb == null)
				{
					//no records exists in database
					_unitOfWork.ShoppingCart.Add(ShoppingCart);
				}
				else
				{
					//record exist in database
					var countFromDb = cartFromDb.Count;
					var totalCount = countFromDb + ShoppingCart.Count;
					cartFromDb.Count = totalCount;
					_unitOfWork.ShoppingCart.Update(cartFromDb);
				}

				var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == ShoppingCart.ApplicationUserId).Count();
				HttpContext.Session.SetObject(SD.ssShoppingCart, count);

				
			}
			_unitOfWork.Save();



			return RedirectToAction(nameof(Details), new { id = ShoppingCart.ProductId});
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
