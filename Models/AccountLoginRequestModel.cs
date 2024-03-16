using System.ComponentModel.DataAnnotations;

namespace Blocks_api.Models
{
    public class AccountLoginRequestModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime Expire { get; set; }
    }
}
