using System.ComponentModel.DataAnnotations;

namespace Company.PL.ViewModels
{
    public class ResetPasswordViewModel
    {
		[Required(ErrorMessage = "New Password Is Required")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [Compare("NewPassword", ErrorMessage = "ConfirmPassword Does not match Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
