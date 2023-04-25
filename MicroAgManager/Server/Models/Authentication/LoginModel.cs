using System.ComponentModel.DataAnnotations;

namespace Server.Models.Authentication
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string email { get; set; } // NOTE: email will be the username, too

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string password { get; set; }

        public bool rememberMe { get; set; }
    }
}
