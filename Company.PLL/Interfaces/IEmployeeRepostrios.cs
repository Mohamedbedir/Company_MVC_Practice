using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.PLL.Interfaces
{
    public interface IEmployeeRepostrios:IGenericRepostrios<Employee>
    {
       public IQueryable<Employee> GetEmployeesByAddress(string address);
        public IQueryable<Employee> GetEmployeesByName(string name);

    }
}
