using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using SalaryManagementSystem;
using PagedList;

namespace SalaryManagementSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private SalaryManagementEntities db = new SalaryManagementEntities();
        private string uriStr = "http://localhost/SalaryManagement/api/";
        
        // GET: Employees
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, string currentMinSalFilter, 
            string currentMaxSalFilter, string minSalaryString, string maxSalaryString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortingIDParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.SortingNameParm = sortOrder == "name" ? "name_desc" : "name";
            ViewBag.SortingLoginNameParm = sortOrder == "loginname" ? "loginname_desc" : "loginname";
            ViewBag.SortingSalaryParm = sortOrder == "salary" ? "salary_desc" : "salary";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            if (minSalaryString != null || maxSalaryString != null)
                page = 1;
            else
            {
                if (minSalaryString == null)
                    minSalaryString = currentMinSalFilter;
                if (maxSalaryString == null)
                    maxSalaryString = currentMaxSalFilter;
            }

            ViewBag.CurrentMinSalFilter = minSalaryString;
            ViewBag.CurrentMaxSalFilter = maxSalaryString;

            List<Employee> employees = GetEmployees();

            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.EmployeeID.Contains(searchString)
                                       || s.LoginName.Contains(searchString)
                                       || s.Name.Contains(searchString)).ToList();
            }
            if (!String.IsNullOrEmpty(minSalaryString) && Decimal.TryParse(minSalaryString, out decimal minSal))
            {
                employees = employees.Where(s => s.Salary >= Decimal.Parse(minSalaryString)).ToList();
            }
            if (!String.IsNullOrEmpty(maxSalaryString) && Decimal.TryParse(maxSalaryString, out decimal maxSal))
            {
                employees = employees.Where(s => s.Salary <= Decimal.Parse(maxSalaryString)).ToList();
            }

            switch (sortOrder)
            {
                case "id_desc":
                    employees = employees.OrderByDescending(s => s.EmployeeID).ToList();
                    break;
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.Name).ToList();
                    break;
                case "name":
                    employees = employees.OrderBy(s => s.Name).ToList();
                    break;
                case "loginname_desc":
                    employees = employees.OrderByDescending(s => s.LoginName).ToList();
                    break;
                case "loginname":
                    employees = employees.OrderBy(s => s.LoginName).ToList();
                    break;
                case "salary_desc":
                    employees = employees.OrderByDescending(s => s.Salary).ToList();
                    break;
                case "salary":
                    employees = employees.OrderBy(s => s.EmployeeID).ToList();
                    break;
                default:
                    employees = employees.OrderBy(s => s.EmployeeID).ToList();
                    break;
            }

            return View(GetEmployeesPagelist(employees, page));
        }

        private IPagedList<Employee> GetEmployeesPagelist(List<Employee> employees, int? page)
        {
            int pageSize = 30;
            int pageNumber = (page ?? 1);
            return employees.ToPagedList(pageNumber, pageSize);
        }

        private List<Employee> GetEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uriStr);
                var responseTask = client.GetAsync("Users");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<Employee>>();
                    readTask.Wait();

                    employees = readTask.Result;
                }
                else
                {
                    employees = new List<Employee>();

                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }
            return employees;
        }

        // GET: Employees/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = new Employee();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(uriStr);
                var responseTask = client.GetAsync("Users/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Employee>();
                    readTask.Wait();

                    emp = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                if (emp == null)
                {
                    return HttpNotFound();
                }
            }

            return View(emp);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EmployeeID,LoginName,Name,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(uriStr);
                    var postTask = client.PostAsJsonAsync<Employee>("Users", employee);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "Error. " + result.Content.ReadAsStringAsync().Result);
                    }                    
                }
            }

            return View(employee);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.ToList().Find(x => x.EmployeeID == id);    
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EmployeeID,LoginName,Name,Salary")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(uriStr + "Users/");
                    var postTask = client.PutAsJsonAsync<Employee>(employee.EmployeeID, employee);
                    postTask.Wait();
                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("CustomError", "Error. " + result.Content.ReadAsStringAsync().Result);
                    }
                }
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            using (var client = new HttpClient())
            {
                Employee emp = new Employee();

                client.BaseAddress = new Uri(uriStr);
                var responseTask = client.DeleteAsync("Users/" + id);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<Employee>();
                    readTask.Wait();

                    emp = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }

                if (emp == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }


        // POST: Employees/Index
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase upload)
        {
            if (ModelState.IsValid && upload != null)
            {
                using (var client = new HttpClient())
                {
                    using (var formData = new MultipartFormDataContent())
                    {
                        byte[] fileByte;
                        using (var reader = new BinaryReader(upload.InputStream))
                        {
                            fileByte = reader.ReadBytes(upload.ContentLength);
                        }

                        formData.Add(new StreamContent(new MemoryStream(fileByte)), "image", upload.FileName);

                        client.BaseAddress = new Uri(uriStr);
                        var postTask = client.PostAsync("Users/Upload", formData);
                        postTask.Wait();
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            ModelState.AddModelError("CustomError", "Upload success.");
                        }
                        else
                        {
                            ModelState.AddModelError("CustomError", "Error. " + result.Content.ReadAsStringAsync().Result);
                        }
                    }                    
                }                
            }
            else if(upload == null){
                ModelState.AddModelError("CustomError", "Error. Please select a file.");
            }
            return View(GetEmployeesPagelist(GetEmployees(), 1));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
