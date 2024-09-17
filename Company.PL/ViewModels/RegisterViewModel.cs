using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="First Name Is Required")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name Is Required")]

        public string LName { get; set; }
        [Required(ErrorMessage = "E-Mail Is Required")]
        [EmailAddress(ErrorMessage ="Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [Compare("Password",ErrorMessage ="ConfirmPassword Does not match Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public bool IsAgree { get; set; }


    }
}
