using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Authentication
{
    public class AuthenticationCommand : IRequest<LoginResult>
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email address is not valid.")]
        public string Email { get; set; } // NOTE: email will be the username, too

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
