using Microsoft.EntityFrameworkCore;

namespace Models
{
    public class FitnessContext : DbContext
    {
        public DbSet<Klijent> Klijenti { get; set; }
        public DbSet<Teretana> Teretane { get; set; }
        public DbSet<Trening> Treninzi { get; set; }
        public DbSet<Spoj> KlijentiTreninzi { get; set; }

        public FitnessContext(DbContextOptions options) : base(options)
        {

        }
    }
}