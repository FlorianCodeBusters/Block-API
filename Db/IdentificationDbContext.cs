using Blocks_api.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blocks_api.Db
{
    public class IdentificationDbContext : IdentityDbContext<User>
    {

        public IdentificationDbContext(DbContextOptions<IdentificationDbContext> options)
            : base(options)
        {
        }
    }
}
