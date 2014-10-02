using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Northwind.Domain;
using Northwind.RepositoryInterface;
//using Rhino.Mocks;

    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

namespace NorthwindSimple.WebApp.Tests.Unit
{
    public class Employeex

    {
        public int EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }
    }

    public class ExampleContext : DbContext
    {
        public ExampleContext()
            : base("ExampleConnectionStringName")
        {
        }

        public virtual IDbSet<Employee> Employees { get; set; }
    }

//    public class ExampleContext : DbContext
//    {
//        public NORTHWNDdbContext()
//            : base("name=NORTHWNDdbContext")
//        {
//        }
//        public ExampleContext() : base(“ExampleConnectionStringName”)

//    {

//    }

//      public virtual IDbSet<Customer> Customers { get; set; }

//}


    public class EmployeeRepositoryTests : IRepository<Employee>

    {

        private ExampleContext _MockContext;

        public IQueryable<Employee> MockData;

        public IDbSet<Employee> MockSet;

        // [SetUp]

        public ExampleContext Context()
        {
            return _MockContext;
        }

        public EmployeeRepositoryTests()

        {

            _MockContext = MockRepository.GenerateMock<ExampleContext>();

            MockSet = MockRepository.GenerateMock<IDbSet<Employee>, IQueryable>();

            MockData = new List<Employee>

            {

                new Employee
                {
                     EmployeeID = 119,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    BirthDate = new DateTime(1981, 1, 1)
                },

                new Employee
                {
                    EmployeeID = 120,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    BirthDate = new DateTime(1981, 1, 1)
                },

                new Employee
                {
                    EmployeeID = 118,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    BirthDate = new DateTime(1971, 10, 10)
                }

            }.AsQueryable();

            // These were hard to find

            MockSet.Stub(m => m.AsQueryable().Provider).Return(MockData.Provider);

            MockSet.Stub(m => m.AsQueryable().Expression).Return(MockData.Expression);

            MockSet.Stub(m => m.AsQueryable().ElementType).Return(MockData.ElementType);

            MockSet.Stub(m => m.AsQueryable().GetEnumerator()).Return(MockData.GetEnumerator());


            _MockContext.Stub(x => x.Employees).PropertyBehavior();

            _MockContext.Employees = MockSet;

        }

        IQueryable<Employee> IRepository<Employee>.Get(System.Linq.Expressions.Expression<Func<Employee, bool>> filter)
        {
            throw new NotImplementedException();
        }

        IQueryable<Employee> IRepository<Employee>.Get()
        {
            throw new NotImplementedException();
        }

        void IRepository<Employee>.Add(Employee entity)
        {
            throw new NotImplementedException();
        }

        void IRepository<Employee>.Delete(object id)
        {
            throw new NotImplementedException();
        }

        void IRepository<Employee>.Delete(Employee entityToDelete)
        {
            throw new NotImplementedException();
        }

        void IRepository<Employee>.Update(Employee entityToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
