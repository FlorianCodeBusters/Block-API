namespace Blocks_api
{
    public static class Const
    {
        public const int UsernameMinLength = 0;

        public const string PasswordRegex = "^(?=.*\\d{1})(?=.*[a-z]{1})(?=.*[A-Z]{1})(?=.*[!@#$%^&*{|}?~_=+.-]{1})(?=.*[^a-zA-Z0-9])(?!.*\\s).{6,24}$";

        public const string UsernameLengthValidationErrorMessage = $"Username must be at least 0 characters long.";

        public const string EmailValidationErrorMessage = "Invalid email address.";

        public const string PassWordValidationErrorMessage = "Password must have more than 6 characters, min. 1 uppercase, min. 1 lowercase, min. 1 special characters.";

        public class LoginProviders
        {
            public const string Google = "GOOGLE";
            public const string Facebook = "FACEBOOK";
            public const string Password = "PASSWORD";
        }
    }
}
