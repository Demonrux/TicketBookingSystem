using Microsoft.EntityFrameworkCore;
using DatabaseModels;

namespace DatabaseContext
{
    public class ApplicationContext : DbContext
    {
        private readonly string _connectionString;
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }

        public ApplicationContext(DatabaseConfig config)
        {
            _connectionString = config.ConnectionString;
            Database.EnsureCreated(); 
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("Строка подключения не установлена.");
            }

            optionsBuilder.UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ticket>()
                .HasOne<Visitor>()
                .WithMany()
                .HasForeignKey(tickets => tickets.VisitorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne<Exhibition>()
                .WithMany()
                .HasForeignKey(tickets => tickets.ExhibitionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}