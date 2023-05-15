using BackEnd.Models.Authentication;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.BusinessLogic.Authentication
{
    public class RefreshTokenCommand:IRequest<TokenModel>
    {
        [Required(ErrorMessage = "Token Is Required")]
        public TokenModel? Token { get; set; }
    }
}
