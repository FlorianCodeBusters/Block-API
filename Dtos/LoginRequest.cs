using System.ComponentModel.DataAnnotations;

namespace Blocks_api.Dtos
{
    public class LoginRequest
    {
        [MinLength(Const.UsernameMinLength, ErrorMessage = Const.UsernameLengthValidationErrorMessage)]
        public string? Username { get; set; }

        [RegularExpression(Const.PasswordRegex, ErrorMessage = Const.PassWordValidationErrorMessage)]
        public string? Password { get; set; }
    }
}
