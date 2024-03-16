using System.ComponentModel.DataAnnotations;

namespace Blocks_api.Dtos
{
    public class RegisterRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Email { get; set; }
    }
}
