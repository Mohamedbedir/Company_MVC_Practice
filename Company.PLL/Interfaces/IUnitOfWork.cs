using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.PLL.Interfaces
{
    public interface IUnitOfWork :IDisposable
    {
        public IEmployeeRepostrios EmployeeRepostrios { get; set; }
        public IDepatmentRepostrios DepartmentRepostrios { get; set; }

        public Task<int> Complete();

       
    }
}
