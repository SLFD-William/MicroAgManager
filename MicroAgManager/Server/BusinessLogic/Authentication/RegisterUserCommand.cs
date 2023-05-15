using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Authentication
{
    public class RegisterUserCommand : LoginUserCommand
    {
        [Required(ErrorMessage = "Confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confimation does not match.")]
        public string? ConfirmPassword { get; set; }
        [Display(Description = "Farm Name")]
        [Required(ErrorMessage = "Farm Name is required.")]
        public string? Name { get; set; }
    }
}
