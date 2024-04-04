using Microsoft.AspNetCore.Identity;

namespace Blocks_api.Entities
{
    public class User : IdentityUser
    {
        public string Provider { get; set; } = null!;
    }
}
