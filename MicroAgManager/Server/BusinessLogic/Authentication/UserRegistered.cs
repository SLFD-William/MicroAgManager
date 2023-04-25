using Domain.Entity;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Server.BusinessLogic.Authentication
{
    public class UserRegistered:INotification
    {
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public RegisterUserCommand Registration { get; set; }
        public UserRegistered(ApplicationUser user, RegisterUserCommand registration)
        {
            User = user;
            Registration = registration;
        }
    }
}
