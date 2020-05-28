using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SalaryManagementSystem.Controllers;

namespace SalaryManagementSystem.Tests.Controllers
{
    [TestClass]
    public class UsersControllerTest
    {
        [TestMethod]
        public void GetEmployees()
        {
            // Arrange
            var mockRepository = new Mock<SalaryManagementEntities>();
            mockRepository.Setup(x => x.Employees.Find("e1001"))
                .Returns(new Employee { EmployeeID = "e1001", LoginName = "TestUser", Name = "TestUserName", Salary = new decimal(999.99) });

            var controller = new UsersController(mockRepository.Object);

            // Act
            IQueryable<Employee> actionResult = controller.GetEmployees();

            // Assert
            Assert.IsNotNull(actionResult);
        }

        [TestMethod]
        public void GetEmployee()
        {
            // Arrange
            var mockRepository = new Mock<SalaryManagementEntities>();
            mockRepository.Setup(x => x.Employees.Find("e1001"))
                .Returns(new Employee { EmployeeID = "e1001", LoginName = "TestUser", Name = "TestUserName", Salary = new decimal(999.99) });

            var controller = new UsersController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.GetEmployee("e1001");
            var contentResult = actionResult as OkNegotiatedContentResult<Employee>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("e1001", contentResult.Content.EmployeeID);
        }

        [TestMethod]
        public void PutEmployee()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Employee>>();
            var mockRepository = new Mock<SalaryManagementEntities>();
            mockRepository.Setup(c => c.Employees).Returns(mockSet.Object);
            mockRepository.Setup(c => c.SetModified(It.IsAny<Employee>()));

            var controller = new UsersController(mockRepository.Object);

            // Act
            Employee emp = new Employee { EmployeeID = "e1001", LoginName = "TestUser", Name = "TestUserName2", Salary = new decimal(999.99) };
            var actionResult = controller.PutEmployee("e1001", emp);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, ((StatusCodeResult)actionResult).StatusCode);
        }

        [TestMethod]
        public void PostEmployee()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Employee>>();
            var mockRepository = new Mock<SalaryManagementEntities>();
            mockRepository.Setup(c => c.Employees).Returns(mockSet.Object);
            var controller = new UsersController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.PostEmployee(new Employee { EmployeeID = "e1001", LoginName = "TestUser", Name = "TestUserName", Salary = new decimal(999.99) });
            var createdResult = actionResult as CreatedAtRouteNegotiatedContentResult<Employee>;

            // Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual("DefaultApi", createdResult.RouteName);
            Assert.AreEqual("e1001", createdResult.RouteValues["id"]);
        }

        [TestMethod]
        public void DeleteEmployee()
        {
            // Arrange
            var mockSet = new Mock<DbSet<Employee>>();
            var mockRepository = new Mock<SalaryManagementEntities>();
            mockRepository.Setup(c => c.Employees).Returns(mockSet.Object);
            mockRepository.Setup(x => x.Employees.Find("e1001"))
                .Returns(new Employee { EmployeeID = "e1001", LoginName = "TestUser", Name = "TestUserName", Salary = new decimal(999.99) });

            var controller = new UsersController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = controller.DeleteEmployee("e1001");
            var contentResult = actionResult as OkNegotiatedContentResult<Employee>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual("e1001", contentResult.Content.EmployeeID);
        }
    }
}
