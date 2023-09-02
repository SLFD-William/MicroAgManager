using System.ComponentModel.DataAnnotations;

namespace BackEnd.Authentication
{
    public class RegisterUserCommand : LoginUserCommand
    {
        [Required(ErrorMessage = "Confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Confimation does not match.")]
        public string? ConfirmPassword { get; set; }
        public Guid? TenantId { get; set; }
    }
}
