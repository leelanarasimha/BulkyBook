using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
                
        }
        public IActionResult Index()
        {
            var Product = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return View(Product);
        }

        public IActionResult Upsert(int? id) {
            var ProductViewModel = new ProductViewModel() {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem() {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                })
            };

            if (id == null)
            {
                return View(ProductViewModel);

            }
            else
            {
                ProductViewModel.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
                if (ProductViewModel.Product == null) {
                    return NotFound();
                }
                return View(ProductViewModel);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductViewModel ProductVM) {
            if (!ModelState.IsValid) {
                ProductVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                });
                ProductVM.CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem()
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                });
                return View(ProductVM);
            }

            //get wwwroot folder path
            var webRootPath = _webHostEnvironment.WebRootPath;

            //getfiles uploaded in form
            var files = HttpContext.Request.Form.Files;


            if (files.Count > 0) {
                //files present
                var fileName = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
                var uploadPath = Path.Combine(webRootPath, @"images\products");
                var fileExtension = Path.GetExtension(files[0].FileName);

                //upload file
                if (ProductVM.Product.ImageUrl != null)
                {
                    //file already exists need to delete file first


                    //get existed image path
                    var imagePath = Path.Combine(webRootPath, ProductVM.Product.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(imagePath)) {
                        System.IO.File.Delete(imagePath);
                    }

                }

                //upload file to the image path
                using (var fileStreams = new FileStream(Path.Combine(uploadPath, fileName + fileExtension), FileMode.Create)) {

                    files[0].CopyTo(fileStreams);
                }

                ProductVM.Product.ImageUrl = @"\images\products\" + fileName + fileExtension;

            } else {
                if (ProductVM.Product.Id != 0) {
                    var objFromDb = _unitOfWork.Product.Get(ProductVM.Product.Id);
                    ProductVM.Product.ImageUrl = objFromDb.ImageUrl;
                }
            }

           

            if (ProductVM.Product.Id == 0) {
                //Adding product
                _unitOfWork.Product.Add(ProductVM.Product);
            } else {
                _unitOfWork.Product.Update(ProductVM.Product);
            }
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}