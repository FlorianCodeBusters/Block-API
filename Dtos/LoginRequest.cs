using System.ComponentModel.DataAnnotations;

namespace Blocks_api.Dtos
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
