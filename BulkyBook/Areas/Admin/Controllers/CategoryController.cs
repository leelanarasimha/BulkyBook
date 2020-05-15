using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{

    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       
        public IActionResult Index()
        {
            var categories = _unitOfWork.Category.GetAll();

            return View(categories);
        }

        public IActionResult Upsert(int? id) {

            if (id == null)
            {
                var category = new Category();
                return View(category);
            }
            else
            {
                var category = _unitOfWork.Category.Get(id.GetValueOrDefault());
                return View(category);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category) {
            if (!ModelState.IsValid) {
                return View(category);
            }

            if (category.Id == 0)
            {
                _unitOfWork.Category.Add(category);
                
            }
            else
            {
                _unitOfWork.Category.Update(category);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id) {
            var category = _unitOfWork.Category.Get(id);
            if (category == null) {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll() {
            var allObj = _unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }
        #endregion
    }
}