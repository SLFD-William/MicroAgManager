using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models.Authentication
{
    public class UserRegistrationModel : LoginModel
    {
        [Required(ErrorMessage = "Confirmation is required.")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "Confimation does not match.")]
        public string confirmpwd { get; set; }
        [Display(Description ="Farm Name")]
        [Required(ErrorMessage = "Farm Name is required.")]
        public string Name { get; set; }
    }
}
