using EFCoreImageUploadAssignment2.DAL;
using EFCoreImageUploadAssignment2.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreImageUploadAssignment2.Controllers
{
    public class DepartmentController : Controller
    {
        AppDbContext _db;

        public DepartmentController(AppDbContext db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<Department> dept = (from prd in _db.Departments where prd.DeptId > 0 select prd).ToList();
            return View(dept);
        }

        public IActionResult CreateDepartment()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateDepartment(Department dept)
        {
            ModelState.Remove("DeptId");
            if (ModelState.IsValid)
            {
                _db.Departments.Add(dept);
                _db.SaveChanges();
                ModelState.Clear();
                RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult EditDepartment(int Id)
        {
            //List<Department> dept = (from prd in _db.Departments where prd.DeptId == id select prd).ToList();
            //IEnumerable<Department> dept = (from prd in _db.Departments where prd.DeptId == id select prd).AsEnumerable();

            Department Dept= _db.Departments.Find(Id);
            return View("CreateDepartment", Dept);
        }


        [HttpPost]
        public IActionResult EditDepartment(Department dept)
        {
            //var deptRecord =
            //      (from e in _db.Departments
            //       where e.DeptId == dept.DeptId
            //       select e).SingleOrDefault();
            if (ModelState.IsValid)
            {
                _db.Departments.Update(dept);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }


        public JsonResult DeleteDepartment(int Id)
        {
            var itemRemove = _db.Departments.SingleOrDefault(x => x.DeptId == Id);
            if (itemRemove != null)
            {
                _db.Departments.Remove(itemRemove);
                _db.SaveChanges();
                return Json("success");
            }
            return Json("fail");

        }




    }
}
