using System.ComponentModel.DataAnnotations;

namespace RunningGroupsWebApp.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email address is required.")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
