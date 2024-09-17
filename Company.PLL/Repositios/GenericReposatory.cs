using Company.DAL.Contexts;
using Company.DAL.Models;
using Company.PLL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.PLL.Repositios
{
    public class GenericReposatory<T> :IGenericRepostrios<T> where T : class
    {
        private readonly CompanyDbContext _context;

        public GenericReposatory(CompanyDbContext context)
        {
            _context = context;
        }

        public void Delete(T item)
        => _context.Set<T>().Remove(item);
  

        public async Task<T> Get(int id)
        {
            return await _context.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(d=>d.department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Add(T item)
        =>await _context.Set<T>().AddAsync(item);
        //هنا ال thread بيقلها استني انت هنا اشتغلي براحتك وانا هشوف خاجه تاني عبال متخلصي

        public void Update(T item)
        =>_context.Set<T>().Update(item);

    }
}
