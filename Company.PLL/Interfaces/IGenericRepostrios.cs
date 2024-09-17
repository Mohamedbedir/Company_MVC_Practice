using Company.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.PLL.Interfaces
{
    public interface IGenericRepostrios<T>
    {
        // using task before data type to using Async
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        void Update(T item); // مش هتتعمل async عشان مش معتمده ع حاجه بتم الاول
        void Delete(T item);  // مش هتتعمل async عشان مش معتمده ع حاجه بتم الاول

        Task Add(T item);
    }
}
