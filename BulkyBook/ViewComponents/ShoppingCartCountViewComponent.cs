using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Utility;
using BulkyBook.Utility.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BulkyBook.ViewComponents
{
	public class ShoppingCartCountViewComponent : ViewComponent
	{
		private readonly IUnitOfWork _unitOfWork;

		public ShoppingCartCountViewComponent(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;

		}

		public IViewComponentResult Invoke()
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
			var sessionCount = HttpContext.Session.GetObject<string>(SD.ssShoppingCart);
			if (userId != null)
			{
				if (sessionCount == null || sessionCount == "")
				{
					var count = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId.Value).Count();
					HttpContext.Session.SetObject(SD.ssShoppingCart, count);
					sessionCount = count.ToString();
				}
			}
			else
			{
				sessionCount = 0.ToString();
			}




			return View("Default", sessionCount);
		}
	}
}
