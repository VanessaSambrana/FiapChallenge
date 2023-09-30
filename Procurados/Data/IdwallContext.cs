using Procurados.Models;
using Microsoft.EntityFrameworkCore;

namespace Procurados.Data
{
    public class IdwallContext : DbContext
    {
        public IdwallContext(DbContextOptions<IdwallContext> opts)
            : base(opts)
        {
        }

        public DbSet<IdwallUser> User { get; set; }
    }
}

        

