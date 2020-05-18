using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var users = _unitOfWork.ApplicationUser.getUserWithRole();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LockUser() {
            var id =  HttpContext.Request.Form["Id"].ToString();
            var userdetails = _unitOfWork.ApplicationUser.GetUserById(id);

            if (userdetails != null) {
                var todaysDate = DateTimeOffset.Now;
                if (userdetails.LockoutEnd == null || userdetails.LockoutEnd <= todaysDate)
                {
                    userdetails.LockoutEnd = DateTimeOffset.Now.AddYears(1000);
                }
                else
                {
                    userdetails.LockoutEnd = null;
                }

                _unitOfWork.ApplicationUser.Update(userdetails);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));

        }
    }
}