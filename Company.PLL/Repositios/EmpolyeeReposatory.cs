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
    public class EmployeeReposatory :GenericReposatory<Employee> ,IEmployeeRepostrios
    {
        private readonly CompanyDbContext _context;

        public EmployeeReposatory(CompanyDbContext context):base(context) 
        {
            this._context = context;
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
            =>_context.Employees.Where(E=>E.Address == address);

        public IQueryable<Employee> GetEmployeesByName(string name)
            => _context.Employees.Where(e => e.Name.Contains(name));
        //            => _context.Employees.Where(e => e.Name.ToLower().Contains(name.ToLower()));

    }
}
