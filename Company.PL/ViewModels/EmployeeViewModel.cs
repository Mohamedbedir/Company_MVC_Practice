using Company.DAL.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Http;

namespace Company.PL.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "the max Length is 50 chars")]
        [MinLength(5, ErrorMessage = "the Min Length is 50 chars")]
        public string Name { get; set; }
        [Range(20, 60, ErrorMessage = "the range for age must between 20 , 60")]
        public int Age { get; set; }
        [RegularExpression("^[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$",
            ErrorMessage = "Address must Be Like 123-Street-City-Country")]
        public string Address { get; set; }
        [DataType(DataType.Currency)]
        public decimal Salary { get; set; }

        public bool IsActive { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        public DateTime hireDate { get; set; }

        public IFormFile Image { get; set; }
        public string ImageName { get; set; }



        //[ForeignKey(nameof(department))] //بتامن نفسك انه هيعمل FK  لل بروبيرتي دي
        public int? DepartmentId { get; set; } //FK
        //Fk optional  : onDelete Restrict -->  مش هيمسح كل اللي موجودين  ف الديبارت منت لو مسحته

        //Fk Required  : onDelete CasCade --> هيمسح كل اللي موجودين  ف الديبارت منت لو مسحته

        //[InverseProperty] بتخصص كل علاقه مع اللي بيقبلها من الكلاس التاني
        //navigational property one

        public Department department {  get; set; }
    }
}
