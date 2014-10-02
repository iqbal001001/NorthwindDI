using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Northwind.Domain;
using Northwind.RepositoryInterface;

namespace Northwind.WebApp.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IEmployeeRepository employeeRepository,IUnitOfWork unitOfWork)
        {
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }


        // GET: Employees
        public ActionResult Index()
        {
            List<Employee> employees = _employeeRepository.Get().ToList();

            return View("Index", employees);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = _employeeRepository.Get().FirstOrDefault(x => x.EmployeeID == id);

                //.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View("Details",employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.ReportsTo = new SelectList(_employeeRepository.Get().ToList(), "EmployeeID", "LastName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ActionName("Create")]
        //[AcceptButton(ButtonName = "Create")]
        [ValidateAntiForgeryToken]
         //MultiButton(Name = "action", Argument = "Create")]
        public ActionResult Create([Bind(Include = "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath")] 
            Employee employee, HttpPostedFileBase file)
        {
            //HttpRequest;
            //if (this.Request != null) { HttpRequest req = this.Request.RequestContext.HttpContext.Request; }
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (!string.IsNullOrEmpty(HttpContext.Request.Form["Upload"]))
                    {
                        //attach the uploaded image to the object before saving to Database
                        employee.Photo = new byte[file.ContentLength];
                        employee.PhotoPath = file.FileName;
                        file.InputStream.Read(employee.Photo, 0, file.ContentLength - 1);
                    }
                }
                else
                {
                    //if (!string.IsNullOrEmpty(req.Form["Create"])) {
                    _employeeRepository.Add(employee);
                    _unitOfWork.SaveChanges();
                    return RedirectToAction("Index");
                    //}
                }
            }

            ViewBag.ReportsTo = new SelectList(_employeeRepository.Get().ToList(), "EmployeeID", "LastName", employee.ReportsTo);
            return View("Create",employee);
        }


      


        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = _employeeRepository.Get().FirstOrDefault(x => x.EmployeeID == id);

            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.ReportsTo = new SelectList(_employeeRepository.Get().ToList(), "EmployeeID", "LastName", employee.ReportsTo);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Region,PostalCode,Country,HomePhone,Extension,Photo,Notes,ReportsTo,PhotoPath")] 
            Employee employee, HttpPostedFileBase file)
        {
            var req = this.Request.RequestContext.HttpContext.Request;
            if (ModelState.IsValid)
            {

                if (file != null)
                {
                    //attach the uploaded image to the object before saving to Database
                    employee.Photo = new byte[file.ContentLength];
                    employee.PhotoPath = file.FileName;
                    file.InputStream.Read(employee.Photo, 0, file.ContentLength - 1);
                }
                else
                {
                    if (!string.IsNullOrEmpty(req.Form["Edit"]))
                    {
                        _employeeRepository.Update(employee);
                        _unitOfWork.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }
            ViewBag.ReportsTo = new SelectList(_employeeRepository.Get().ToList(), "EmployeeID", "LastName", employee.ReportsTo);
            return View("Edit",employee);
        }



        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = _employeeRepository.Get().FirstOrDefault(x => x.EmployeeID == id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = _employeeRepository.Get().FirstOrDefault(x => x.EmployeeID == id);
            _employeeRepository.Delete(employee);
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
