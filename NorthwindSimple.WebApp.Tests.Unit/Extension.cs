using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Northwind.Domain;

namespace NorthwindSimple.WebApp.Tests.Unit
{
    static class Extension
    {
        //public static IQueryable<Employee> FilterHairColors(this IQueryable<Employee> subQuery, Employee[] employees)
        //{
        //    var result = new Employee[] { }.AsQueryable();
        //    var contains = new Dictionary<string, Expression<Func<Employee, bool>>>();

        //    //contains.Add("1", p => p.HairColor_bright);
        //    //contains.Add("2", p => p.HairColor_brown);
        //    //contains.Add("3", p => p.HairColor_dark);
        //    //contains.Add("4", p => p.HairColor_red);

        //    foreach (var employee in employees)
        //    {
        //        result = subQuery.Union(result);
        //    }

        //    return result;
        //}
    }
}
