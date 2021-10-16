using EFCoreImageUploadAssignment2.DAL;
using EFCoreImageUploadAssignment2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace EFCoreImageUploadAssignment2.Controllers
{
    public class EmployeeController : Controller
    {
        AppDbContext _db;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmployeeController(AppDbContext db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            this._hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Sk: Get Employee record
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var EmployeeData = (from emp in _db.Employees
                                join dep in _db.Departments on emp.DeptId equals dep.DeptId
                                join img in _db.Image on emp.ImageId equals img.ImageId into ps
                                from p in ps.DefaultIfEmpty()
                                select new EmployeeViewModel
                                {
                                    EmpId = emp.EmpId,
                                    Name = emp.Name,
                                    Address = emp.Address,
                                    DepartmentName = dep.Name,
                                    ImagePath = p.ImagePath != null ? p.ImagePath : "~/image/NoImgPlaceholder.PNG",
                                    ImageId = p.ImageId != null ? p.ImageId : 0

                                }).ToList();

            return View(EmployeeData);
        }

        /// <summary>
        /// Bind department drop down and display create form
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewBag.Departments = _db.Departments.ToList();

            return View();
        }

        /// <summary>
        /// Save employee record
        /// </summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Create(Employee Emp)
        {
            Image img = new Image();
            ModelState.Remove("EmpId");
            if (ModelState.IsValid)
            {
                /*If Image uploaded */
                if (Emp.Image != null)
                {
                    string uniqueFileName = UploadedFile(Emp);
                    img.ImageName = Emp.Image.ImageFile.FileName;
                    img.ImagePath = "~/image/" + uniqueFileName;
                    _db.Image.Add(img);
                    _db.SaveChanges();
                    Emp.ImageId = img.ImageId;
                }

                _db.Employees.Add(Emp);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.Clear();
            ViewBag.Departments = _db.Departments.ToList();
            return View();
        }

        /// <summary>
        /// Display Edit Mode
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {

            ViewBag.Departments = _db.Departments.ToList();
            Employee Emp = _db.usp_GetEmployee(id);
            string ImagePath = _db.Image.Where(x => x.ImageId == Emp.ImageId).Select(u => u.ImagePath).SingleOrDefault();
            ViewBag.ImagePath = ImagePath;
            return View("Create", Emp);
        }

        /// <summary>
        /// SK: Insert/update Employee and Image record
        /// </summary>
        /// <param name="Emp"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Edit(Employee Emp)
        {

            if (ModelState.IsValid)
            {
                string ImagePath = "";

                /*Get Image record */
                var ImgRecord = (from e in _db.Image
                                 where e.ImageId == Emp.ImageId
                                 select e).SingleOrDefault();

                if (ImgRecord != null)
                {
                    ImagePath = ImgRecord.ImagePath;
                }

                string _imageToBeDeleted = Path.Combine(_hostEnvironment.WebRootPath, "", ImagePath);

                /* Delete from folder */
                if ((System.IO.File.Exists(_imageToBeDeleted.Replace("~", ""))))
                {
                    System.IO.File.Delete(_imageToBeDeleted.Replace("~", ""));
                }
                Image img = new Image();

                string uniqueFileName = UploadedFile(Emp);
                img.ImageName = Emp.Image == null ? "" : Emp.Image.ImageFile.FileName;
                img.ImagePath = uniqueFileName == null ? "" : "~/image/" + uniqueFileName;
                img.ImageId = (int)Emp.ImageId;
                int _ImageId = _db.UpdateImage(img.ImageId, img.ImageName, img.ImagePath);
                //_db.Image.Update(img);
                //_db.SaveChanges();
                if (_ImageId > 0)
                {
                    Emp.ImageId = _ImageId;
                }

                _db.Employees.Update(Emp);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = _db.Departments.ToList();
            return View("Create", Emp);
        }


        /// <summary>
        /// Sk: upload Image into folder
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string UploadedFile(Employee model)
        {
            string uniqueFileName = null;

            if (model.Image != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Image.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Image.ImageFile.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        /// <summary>
        /// SK: Delete employee/Image record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult DeleteEmployee(int id)
        {
            var emp_Record = _db.Employees.SingleOrDefault(x => x.EmpId == id);

            var img_Record = _db.Image.SingleOrDefault(x => x.ImageId == emp_Record.ImageId);

           
            /* Delete Image record */
            if (img_Record != null)
            {
                string ImagePath = img_Record.ImagePath == null ? "" : img_Record.ImagePath;

                string _imageToBeDeleted = Path.Combine(_hostEnvironment.WebRootPath, "", ImagePath);
                /* Delete from folder */
                if ((System.IO.File.Exists(_imageToBeDeleted.Replace("~", ""))))
                {
                    System.IO.File.Delete(_imageToBeDeleted.Replace("~", ""));
                }
                _db.Image.Remove(img_Record);
            }

            /* Delege EMployee record*/
            if (emp_Record != null)
            {
                _db.Employees.Remove(emp_Record);
                _db.SaveChanges();
                return Json("success");
            }
            return Json("fail");

        }
    }

}