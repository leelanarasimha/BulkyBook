using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var coverTypes = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll);
            return View(coverTypes);
        }

        public IActionResult Upsert(int? id) {
            var coverType = new CoverType();
            
            if (id != null) {
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            }

            return View(coverType);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType) {
            if (!ModelState.IsValid) {
                return View();
            }

            var parameter = new DynamicParameters();
            parameter.Add("@Name", coverType.Name);
            if (coverType.Id == 0) {
                _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Create, parameter);
            } else {
                parameter.Add("@Id", coverType.Id);
                _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, parameter);
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
            
        }

        public IActionResult Delete(int id) {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            var coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameters);
            if (coverType == null) {
                return NotFound();
            }

            _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete, parameters);
            _unitOfWork.Save();


            return RedirectToAction(nameof(Index));
        }
    }
}