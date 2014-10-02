using System;
using System.Collections;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
//using MvcContrib.TestHelper;
using MvcContrib;
using MvcContrib.TestHelper;
using Northwind.Domain;
using Northwind.RepositoryInterface;
using Northwind.WebApp.Controllers;
//using NSubstitute;
//using Rhino.Mocks;
using Rhino.Mocks;
using ModelStateDictionary = System.Web.ModelBinding.ModelStateDictionary;



using System.Data.Entity.Infrastructure;
using System.Threading.Tasks; 

namespace NorthwindSimple.WebApp.Tests.Unit
{
    /// <summary>
    /// Summary description for EmployerControllerTest
    /// </summary>
    [TestClass]
    public class EmployerControllerTest
    {
        private IQueryable<Employee> _data;
        private Mock<DbSet<Employee>> _mockSet;
        private Mock<IEmployeeRepository> _mockER;
        private Mock<IUnitOfWork> _mockUW;

        public EmployerControllerTest()
        {
             _data = new List<Employee> 
            { 
                new Employee {EmployeeID = 1,  FirstName = "a",LastName = "b" }, 
                new Employee {EmployeeID = 2,  FirstName = "a",LastName = "b" },
                new Employee {EmployeeID = 3,  FirstName = "a",LastName = "b" },
            }.AsQueryable();


           _mockSet = new Mock<DbSet<Employee>>();
           _mockSet.As<IQueryable<Employee>>().Setup(m => m.Provider).Returns(_data.Provider);
           _mockSet.As<IQueryable<Employee>>().Setup(m => m.Expression).Returns(_data.Expression);
           _mockSet.As<IQueryable<Employee>>().Setup(m => m.ElementType).Returns(_data.ElementType);
           _mockSet.As<IQueryable<Employee>>().Setup(m => m.GetEnumerator()).Returns(_data.GetEnumerator());
          

            _mockER = new Mock<IEmployeeRepository>();
            _mockER.Setup(t => t.Get()).Returns(_mockSet.Object);
            _mockER.Setup(t => t.Get(x => x.EmployeeID == It.IsAny<int>())).Returns(_mockSet.Object);
            _mockER.Setup(t => t.Add(It.IsAny<Employee>())).Callback((Employee employee) =>
            {
                var newListEmployee = new List<Employee> {employee};
                _data = _data.Concat(newListEmployee);
            }).Verifiable(); 
            //_mockER.Verify(mr => mr.Update(It.IsAny<Employee>()), Times.Once());
            _mockER.Setup(t => t.Delete(It.IsAny<int>()));
            _mockER.Setup(t => t.Delete(It.IsAny<Employee>()));
            _mockER.Setup(t => t.Update(It.IsAny<Employee>()));

            _mockUW = new Mock<IUnitOfWork>();
            _mockUW.Setup(t => t.SaveChanges());
        }
      

        //reference : http://msdn.microsoft.com/en-au/data/dn314429.aspx
        //reference : http://www.codeproject.com/Articles/47603/Mock-a-database-repository-using-Moq
        //reference : http://stackoverflow.com/questions/15805812/mock-an-update-method-returning-a-void-with-moq

        [TestMethod]
        public void Default_Action_Returns_Index_View()
        {
            // Arrange     
            var employeesController = new EmployeesController(_mockER.Object, null);

            const string expectedViewName = "Index";
            //Act
            var result = employeesController.Index() as ViewResult;

           //Assert
            Assert.IsNotNull(result, "Should have returned a ViewResult");
            result.AssertViewRendered().WithViewData<IEnumerable<Employee>>();
            Assert.AreEqual(expectedViewName, result.ViewName, "View name should have been {0}", expectedViewName);
            }

 
           [TestMethod]
                public void Default_Action_Returns_Index_View_Using_MvcContrib_TestHelper()
                {
                    // Arrange              
                    var employeesController = new EmployeesController(_mockER.Object, null);

                    // Act
                    var result = employeesController.Index();

                    // Assert
                    result.AssertViewRendered().ForView("Index");
                    result.AssertViewRendered().WithViewData<IEnumerable<Employee>>();
                }
  
          
                [TestMethod]
                public void The_Add_Customer_Action_Returns_RedirectToRouteResult_When_The_Customer_Model_Is_Valid()
                {
                    // Arrange  
                    const string expectedRouteName = "EmployeeCreated";
                    var employee = new Employee()
                    {
                      EmployeeID = 1,  FirstName = "a",LastName = "b"   
                    };

                    var employeesController = new EmployeesController(_mockER.Object, _mockUW.Object);
                    employeesController.ModelState.Clear();
                    // Act
                    var result = employeesController.Create(employee,null) as RedirectToRouteResult;

                    // Assert
                    Assert.AreEqual(4, _data.Count());
                    Assert.IsNotNull(result, "Should have returned a RedirectToRouteResult");
                   // Assert.AreEqual(expectedRouteName, result.RouteName, "Route name should have been {0}", expectedRouteName);

                }

                [TestMethod]
                public void The_Add_Customer_Action_Returns_ViewResult_When_The_Customer_Model_Is_Invalid()
                {
                    // Arrange  
                    const string expectedRouteName = "EmployeeCreated";
                    var employee = new Employee()
                    {
                        EmployeeID = 1,
                        FirstName = "a",
                        LastName = "b"
                    };

                    var employeesController = new EmployeesController(_mockER.Object, _mockUW.Object);
                    employeesController.ModelState.AddModelError("A Error", "Message");
                    // Act
                    var result = employeesController.Create(employee, null) as RedirectToRouteResult;

                    // Assert
                    Assert.AreEqual(3, _data.Count());
                    Assert.IsNull(result, "Should have returned a RedirectToRouteResult");
                      }

               







    }  
}
