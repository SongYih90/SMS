﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using LumenWorks.Framework.IO.Csv;
using SalaryManagementSystem;

namespace SalaryManagementSystem.Controllers
{
    public class UsersController : ApiController
    {
        private SalaryManagementEntities db = new SalaryManagementEntities();
        
        public UsersController(SalaryManagementEntities repository)
        {
            db = repository;
        }

        public UsersController()
        {
        }

        // GET: api/Users
        public IQueryable<Employee> GetEmployees()
        {
            return db.Employees;
        }

        // GET: api/Users/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult GetEmployee(string id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmployee(string id, Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            if (db.Employees.ToList().FindAll(x => x.LoginName == employee.LoginName && x.EmployeeID != employee.EmployeeID).Count() > 0)
            {
                return BadRequest("Login Name belonging to another ID already exist in the system. ");
            }

            db.SetModified(employee);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(Employee))]
        public IHttpActionResult PostEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string errMsg = string.Empty;
            if (db.Employees.ToList().FindAll(x => x.EmployeeID == employee.EmployeeID).Count() > 0)
            {
                errMsg += "Employee ID already exist in system. ";
            }

            if (db.Employees.ToList().FindAll(x => x.LoginName == employee.LoginName).Count() > 0)
            {
                errMsg += "Login Name already exist in system. ";
            }
            
            if (errMsg != string.Empty)
            {
                return BadRequest(errMsg);
            }

            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (EmployeeExists(employee.EmployeeID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(Employee))]
        public IHttpActionResult DeleteEmployee(string id)
        {
            Employee employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound();
            }

            db.Employees.Remove(employee);
            db.SaveChanges();

            return Ok(employee);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmployeeExists(string id)
        {
            return db.Employees.ToList().Count(e => e.EmployeeID == id) > 0;
        }
        
        // POST: api/Users/Upload
        [Route("api/Users/Upload")]
        [HttpPost]
        public IHttpActionResult Upload()
        {
            HttpPostedFile upload = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files[0] : null;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string errorMsg = string.Empty;

                if (upload != null && upload.ContentLength > 0)
                {
                    if (upload.FileName.EndsWith(".csv"))
                    {
                        try
                        {
                            Stream stream = upload.InputStream;
                            DataTable csvTable = new DataTable();
                            try
                            {
                                using (CsvReader csvReader =
                                    new CsvReader(new StreamReader(stream), true))
                                {
                                    csvTable.Load(csvReader);
                                }
                            }
                            catch (Exception)
                            {
                                return BadRequest("Corrupted data, there are rows with too many or too few columns");
                            }

                            List<Employee> employeeDetails = new List<Employee>();
                            int rowCount = 1;

                            if (csvTable.Rows.Count == 0)
                            {
                                errorMsg = "File is empty.";
                            }

                            foreach (DataRow row in csvTable.Rows)
                            {
                                Employee emp = new Employee();
                                emp.EmployeeID = row["EmployeeID"].ToString();
                                emp.LoginName = row["LoginName"].ToString();
                                emp.Name = row["Name"].ToString();
                                if (Decimal.TryParse(row["Salary"].ToString(), out decimal result) && result >= 0)
                                    emp.Salary = Decimal.Parse(row["Salary"].ToString());
                                else
                                    errorMsg += "Row " + rowCount + " does not have the correct value for salary. ";
                                
                                if (employeeDetails.FindAll(x => x.LoginName == emp.LoginName && x.EmployeeID != emp.EmployeeID).Count() > 0)
                                    errorMsg += "Row " + rowCount + " Login Name belonging to another ID already exist in the current file. ";

                                if (db.Employees.AsNoTracking().ToList().FindAll(x => x.LoginName == emp.LoginName && x.EmployeeID != emp.EmployeeID).Count() > 0)
                                    errorMsg += "Row " + rowCount + " Login Name belonging to another ID already exist in the system. ";

                                if (emp.EmployeeID == string.Empty || emp.LoginName == string.Empty || emp.Name == string.Empty)
                                    errorMsg += "Row " + rowCount + " has empty data. ";

                                employeeDetails.Add(emp);
                                rowCount++;
                            }

                            if (errorMsg == string.Empty)
                            {
                                foreach (Employee emp in employeeDetails)
                                {
                                    if (db.Employees.AsNoTracking().ToList().Exists(x => x.EmployeeID == emp.EmployeeID))
                                        db.Entry(emp).State = EntityState.Modified;
                                    else
                                        db.Employees.Add(emp);
                                    db.SaveChanges();
                                }
                                return Ok();
                            }
                            else
                            {
                                return BadRequest(errorMsg);
                            }
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    else
                    {
                        return BadRequest("Please upload only CSV file.");
                    }
                }
                else
                {
                    return BadRequest("File is empty.");
                }
            }
            catch (DbUpdateException)
            {
                throw;
            }
        }
    }
}