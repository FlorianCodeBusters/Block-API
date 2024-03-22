using System.ComponentModel.DataAnnotations;
using Blocks_api.Dtos;

namespace Blocks_api.Dtos
{
    public class RegisterRequest
    {
        [MinLength(Const.UsernameMinLength, ErrorMessage = Const.UsernameLengthValidationErrorMessage)]
        public string? Username { get; set; }
        [RegularExpression(Const.PasswordRegex, ErrorMessage = Const.PassWordValidationErrorMessage)]
        public string? Password { get; set; }
        [EmailAddress(ErrorMessage = Const.EmailValidationErrorMessage)]
        public string? Email { get; set; }
    }
}
