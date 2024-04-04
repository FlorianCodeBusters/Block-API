using System.ComponentModel.DataAnnotations;

namespace Blocks_api.Dtos
{
    public class SocialLoginRequest
    {
        [MinLength(Const.UsernameMinLength, ErrorMessage = Const.UsernameLengthValidationErrorMessage)]
        public string? Email { get; set; }

        [Required] 
        public string? Provider { get; set; }

        [Required] 
        public string? AccessToken { get; set; }
    }
}
