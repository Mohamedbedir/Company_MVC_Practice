using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class ForgetPasswordViewModel
    {


        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress(ErrorMessage ="Email not valid")]
        public string Email { get; set; }
    }
}
