using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Models
{
    public class Department
    {
        public int Id { get; set; } // by convention is PK and Identity 1,1

        [Required(ErrorMessage ="the code is required !!")]
        public string Code { get; set; }
        [Required(ErrorMessage ="the name is required !!")]
        [MaxLength(50)]
        public string Name { get; set; }
        public DateTime DateOfCreation { get; set; }

        public ICollection<Employee> employees { get; set; } = new HashSet<Employee>();
        //navigational property many
    }
}
