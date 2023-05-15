using MediatR;
using System.ComponentModel.DataAnnotations;

namespace BackEnd.Authentication
{
    public class RefreshTokenCommand:IRequest<TokenModel>
    {
        [Required(ErrorMessage = "Token Is Required")]
        public TokenModel? Token { get; set; }
    }
}
