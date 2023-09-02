using System.ComponentModel.DataAnnotations;

namespace BackEnd.Authentication
{
    public class UserRegistrationModel : LoginModel
    {
        [Required(ErrorMessage = "Confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Confimation does not match.")]
        public string confirmpwd { get; set; }
    }
}
