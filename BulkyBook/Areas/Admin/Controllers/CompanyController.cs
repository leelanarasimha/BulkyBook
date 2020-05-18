using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]

    [Authorize(Roles =SD.Role_Admin+","+SD.Role_Employee)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var companies = _unitOfWork.Company.GetAll();
            return View(companies);
        }

        public IActionResult Upsert(int? id) {
            var company = new Company();
            if (id != null) {
                company = _unitOfWork.Company.Get(id.GetValueOrDefault());
                if (company == null) {
                    return NotFound();

                }
            }

            return View(company);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company) {
            if (!ModelState.IsValid) {
                return View(company);
            }

            if (company.Id != 0)
            {
                
                //update
                _unitOfWork.Company.Update(company);
            }
            else
            {
                _unitOfWork.Company.Add(company);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}