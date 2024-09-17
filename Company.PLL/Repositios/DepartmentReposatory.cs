using Company.DAL.Contexts;
using Company.DAL.Models;
using Company.PLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.PLL.Repositios
{
    public class DepartmentReposatory :GenericReposatory<Department> ,IDepatmentRepostrios
    {
        public DepartmentReposatory(CompanyDbContext context):base(context)
        {
            
        }
    }
}
