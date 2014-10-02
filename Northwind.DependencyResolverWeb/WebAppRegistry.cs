
using Northwind.Data;
//using Northwind.Data.Context;
using Northwind.RepositoryInterface;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;

namespace Northwind.DependencyResolverWeb
{

    public class WebAppRegistry : Registry
    {
        public WebAppRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IDbContextFactory>().HybridHttpOrThreadLocalScoped().Use<NorthwindDbContextFactory>();
            For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<NorthwindUnitOfWork>();
            For<IEmployeeRepository>().HybridHttpOrThreadLocalScoped().Use<EmployeeRepository>();
           

        }
    }

    
}
