using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Northwind.Domain
{
    [MetadataType(typeof(EmployeeMetadata))]
    public partial class Employee
    {
    }
}