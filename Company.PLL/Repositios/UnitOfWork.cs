using Company.DAL.Contexts;
using Company.PLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.PLL.Repositios
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext context;

        public IEmployeeRepostrios EmployeeRepostrios { get; set ; }
        public IDepatmentRepostrios DepartmentRepostrios { get ; set ; }

        public UnitOfWork(CompanyDbContext context) 
        {
            EmployeeRepostrios = new EmployeeReposatory(context);
            DepartmentRepostrios= new DepartmentReposatory(context);
            this.context = context;
        }

        public async Task<int> Complete()
          =>await context.SaveChangesAsync();

        public void Dispose()
        {
           context.Dispose();
        }



        //Benefits of unit of work
        //1- collect the biasness logic off app in on class
        //2- unit of work (DBContext) اللي بيكلم  Database
        //3- close the connection that opened with database (only used in unit of work)  IDisposable
    }
}
